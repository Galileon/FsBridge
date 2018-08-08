using FsConnect.Protocol;
using FsConnect.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static FsConnect.FreeswitchClient;

namespace FsConnect
{
    internal class FreeswitchSocketConnection
    {

        #region Delegates And Events

        internal delegate void OnFsMessageReceivedDelegate(EventBase message);

        internal event OnFsMessageReceivedDelegate OnFsMessageReceived;

        internal delegate void OnConnectionStateChangedDelegate(EventSocketState state);

        internal event OnConnectionStateChangedDelegate OnConnectionStateChanged;

        #endregion

        #region Properties

        AxCommon.Log.AxLogger Log { get; set; }

        EventSocketState _state;
        internal EventSocketState State
        {
            get
            {
                return this._state;
            }
            set
            {
                if (this._state != value)
                {
                    this._state = value;
                    OnConnectionStateChanged?.Invoke(value);
                }
            }
        }

        internal EventSocketConnectionType ConnectionType { get; set; }

        Socket workSocket = null;

        // Size of receive buffer.
        const int ReceiverBufferSize = 4096;
        // Receive buffer.
        const int MaxMessageSize = 1024 * 32;

        byte[] buffer = new byte[ReceiverBufferSize];

        internal string Host { get; set; }

        internal int Port { get; set; }

        internal CommandResponseHandler ResponseHandler;

        StringBuilder messageHandler = new StringBuilder(MaxMessageSize);

        /// <summary>
        /// Instance of connection to freeswitch
        /// </summary>
        public string InstanceId { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Lock when sending command to peak a result immediatelly
        /// </summary>
        private object _commandSendLock = new object();

        private FreeswitchClient Client { get; set; }

        #endregion

        #region Statics 

        private const string FreeswitchEventSocketEnabledEvents = "CHANNEL_CALLSTATE DTMF CHANNEL_EXECUTE_COMPLETE RECORD_STOP";

        private const string FreeswitchCommandSocketEnabledEvents = "BACKGROUND_JOB";

        private static object TraceFileSync = new object();

        #endregion

        #region .ctor

        internal FreeswitchSocketConnection(EventSocketConnectionType connectionType, FreeswitchClient client)
        {
            this.ConnectionType = connectionType;
            this.Log = client.Log;
            this.Client = client;
        }

        #endregion

        #region Contruct / Destroy

        internal void Construct()
        {
            if (workSocket != null && workSocket.Connected) workSocket.Close();
            workSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            if (ResponseHandler == null) ResponseHandler = new CommandResponseHandler();

            JsonConvert.DefaultSettings = (() =>
            {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(new StringEnumConverter { CamelCaseText = true });
                return settings;
            });
        }

        #endregion

        internal void Connect()
        {
            try
            {
                Construct();

                workSocket.NoDelay = true;
                workSocket.Blocking = false;
                workSocket.ReceiveTimeout = 10;
                //workSocket.ReceiveBufferSize = 128;
                //workSocket.SendBufferSize = 128;
                workSocket.BeginConnect(Host, Port, ConnectCallBack, workSocket);
            }
            catch (Exception cex)
            {
                //Log.ErrorException("Unable to connect", cex);
                this.State = EventSocketState.Disconnected;
            }

        }

        void BeginReceive()
        {
            workSocket.BeginReceive(buffer, 0, ReceiverBufferSize - 1, SocketFlags.Partial, OnDataReceivedCallBack, workSocket);
        }

        private void ProcessAuthRequest(FreeswitchMessageBase authRequest)
        {
            Task.Run(() =>
            {
                if (this.CallCommandSync($"auth {Password}", "Authenticate").Result != CommandReply.CommandResult.OK)
                {

                }
                else
                {
                    if (this.ConnectionType == EventSocketConnectionType.Event) Task.Run(() =>
                    {
                        if (this.CallCommandSync($"event json {FreeswitchEventSocketEnabledEvents}", "enable events").Result == CommandReply.CommandResult.OK) this.State = EventSocketState.Connected;
                        //if (this.CallCommandSync($"event json ALL", "enable events").Result == CommandReply.CommandResult.OK) this.State = EventSocketState.Connected;
                    });

                    if (this.ConnectionType == EventSocketConnectionType.Command) Task.Run(() =>
                    {
                        if (this.CallCommandSync($"event json {FreeswitchCommandSocketEnabledEvents}", "enable events").Result == CommandReply.CommandResult.OK) this.State = EventSocketState.Connected;
                        //if (this.CallCommandSync($"event json ALL", "enable events").Result == CommandReply.CommandResult.OK) this.State = EventSocketState.Connected;
                    });
                }
            });
        }

        private void ProcessCommandReply(FreeswitchMessageBase commandReply)
        {
            ResponseHandler.AppendResponse(commandReply as CommandReply);
        }

        private bool ProcessJsonContent(string content)
        {
            /*
            Console.WriteLine();
            Console.WriteLine(fetchedMessage);
            Console.WriteLine();
            */

            var ob = Newtonsoft.Json.JsonConvert.DeserializeObject<EventBase>(content);
            switch (ob.EventType)
            {
                case EventType.ApiEvent:
                    ob = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiEvent>(content);
                    break;
                case EventType.BackgroundEvent:
                    ob = Newtonsoft.Json.JsonConvert.DeserializeObject<BackgroundJobEvent>(content);
                    this.ResponseHandler.AppendResponse(new CommandReply() { CallId = ob.CallId, RequestId = (ob as BackgroundJobEvent).RequestId, Cause = (ob as BackgroundJobEvent).Cause, Result = (ob as BackgroundJobEvent).Error ? CommandReply.CommandResult.Failed : CommandReply.CommandResult.OK });
                    break;
                case EventType.ChannelCallState:
                    ob = Newtonsoft.Json.JsonConvert.DeserializeObject<CallStateEvent>(content);
                    break;
                case EventType.ChannelExecuteCompleteEvent:
                    ob = Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelExecuteCompleteEvent>(content);
                    break;
                case EventType.DtmfEvent:
                    ob = Newtonsoft.Json.JsonConvert.DeserializeObject<DTMFEvent>(content);
                    break;
                case EventType.PlaybackStopEvent:
                    ob = Newtonsoft.Json.JsonConvert.DeserializeObject<PlaybackCompletedEvent>(content);
                    break;
                case EventType.RecordStopEvent:
                    ob = Newtonsoft.Json.JsonConvert.DeserializeObject<RecordCompletedEvent>(content);
                    break;
            }

            Log.Debug("<OnMessageReceived> Received: {0}", ob);
            if (ob != null && this.OnFsMessageReceived != null) this.OnFsMessageReceived(ob);
            return true;
        }

        private bool ReadFromMessageHandler(out string message)
        {
            message = string.Empty;
            var stringVal = messageHandler.ToString();
            var indexOfEnd = stringVal.IndexOf("\n\n");
            if (indexOfEnd == -1) return false;
            message = stringVal.Substring(0, indexOfEnd);
            return true;
        }

        private string ReadFromMessageHandler(int length)
        {
            var stringVal = messageHandler.ToString();
            return stringVal.Substring(0, length);
        }

        StringBuilder LogKeeper = new StringBuilder();

        void TraceLog(string me, bool splitLine = true)
        {

            if (!this.Client.TraceEnabled || string.IsNullOrEmpty(this.Client.TraceFile)) return;

            LogKeeper.AppendLine();
            if (splitLine) LogKeeper.AppendLine("-------------------------------------------------------------------------------");
            LogKeeper.Append($"{DateTime.Now.ToString ("HH:mm:ss.fff")} - {this.ConnectionType}: {me}");

            lock (TraceFileSync)
            {

                try
                {
                    if (LogKeeper.Length > 0) System.IO.File.AppendAllText(this.Client.TraceFile, LogKeeper.ToString(), Encoding.UTF8);
                    LogKeeper.Clear();
                }
                catch (Exception ex)
                {
                    if (LogKeeper.Length > 128 * 128)
                    {
                        Log.Fatal($"<TraceLog> There is some problem at trace to file: {this.Client.TraceFile} so trace log will be cleared.");
                        this.LogKeeper.Clear();
                    }
                }
            }
        }

        private bool FetchMessage()
        {
            string stringVal = "";
            stringVal = messageHandler.ToString();
            if (stringVal.IndexOf("\n\n") == -1) return false;

            while (ReadFromMessageHandler(out string messagePart))
            {

                var message = MessageRecognizer.GetMessageType(messagePart, out bool dynamicSize, out int contentLenght);
                switch (message.MsgType)
                {
                    case MessageType.AuthRequest:
                        ProcessAuthRequest(message);
                        messageHandler.Remove(0, messagePart.Length + 2);
                        TraceLog(messagePart);
                        break;
                    case MessageType.CommandReply:
                        ProcessCommandReply(message);
                        messageHandler.Remove(0, messagePart.Length + 2);
                        TraceLog(messagePart);
                        break;
                    case MessageType.Event:
                        if (dynamicSize)
                        {
                            if (messageHandler.Length >= messagePart.Length + 2 + contentLenght)
                            {
                                messageHandler.Remove(0, messagePart.Length + 2);
                                var content = ReadFromMessageHandler(contentLenght);
                                TraceLog(messagePart);
                                TraceLog(content, false);
                                try
                                {
                                    ProcessJsonContent(content);
                                }catch (Exception cex)
                                {

                                }
                                messageHandler.Remove(0, contentLenght);
                            }
                            else // No data
                                return true;
                        }
                        else
                        {
                            TraceLog(messagePart);
                            messageHandler.Remove(0, messagePart.Length + 2);
                        }
                        break;
                }
            }

            return true;
        }

        internal CommandReply CallCommandSync(string command, string shortCommandName = "", bool waitForReply = true, string logCommand = "")
        {
            var _sw = new Stopwatch();
            _sw.Start();

            var requestId = Guid.NewGuid();

            if (string.IsNullOrEmpty(shortCommandName)) shortCommandName = command.FirstLine();

            lock (this._commandSendLock)
            {
                var toSenc = string.Format($"{command}\nRequest-Id: {requestId.ToString()}\nJob-UUID: {requestId.ToString()}\n\n"); ;

                //toSenc = toSenc.Replace("[", @"{");
                //toSenc = toSenc.Replace("]", @"}");
                var buffer = Encoding.UTF8.GetBytes(toSenc);
                var sentbytes = 0;

                TraceLog(toSenc);

                while (sentbytes != buffer.Length)
                {
                    sentbytes += workSocket.Send(buffer, sentbytes, buffer.Length - sentbytes, SocketFlags.None);
                }

                if (sentbytes != buffer.Length)
                {
                    return new CommandReply() { Result = CommandReply.CommandResult.Failed, RequestId = requestId };
                }

                Log.Debug($"<CallCommandSync> {shortCommandName} RequestId: {requestId} Called and waiting for reply. {logCommand}");
            }

            if (!waitForReply) return new CommandReply() { Result = CommandReply.CommandResult.OK, RequestId = requestId };

            TraceLog($">>>>>>>> Waiting For Reply for Command: {shortCommandName} in {_sw.Elapsed.TotalMilliseconds} Request: {requestId} ");

            var response = this.ResponseHandler.WaitForReply(requestId);// new CommandReply() { Result = CommandReply.CommandResult.OK, RequestId = requestId.ToString() };

            if (response == null)
            {
                response = new CommandReply() { Result = CommandReply.CommandResult.Failed, RequestId = requestId };
                Console.WriteLine($"Reply TIMEOUT: {requestId} Command: {shortCommandName}");
                Log.Warn($"<CallCommandSync> {shortCommandName} RequestId: {requestId} TimeOut!");
            }
            else
            {
                Console.WriteLine($"Reply: {requestId} Command: {shortCommandName} Result: {response.Result} {response.Cause}");
                Log.Debug($"<CallCommandSync> {shortCommandName} RequestId: {requestId} Response: {response.Result} {response.Cause}");
            }

            _sw.Stop();
            TraceLog($">>>>>>>> Called Command: {shortCommandName} in {_sw.Elapsed.TotalMilliseconds} Request: {requestId} ");

            return response;
        }

        #region Socket Events

        private void ConnectCallBack(IAsyncResult result)
        {
            try
            {
                var client = (Socket)result.AsyncState;
                if (client == null || !client.Connected)
                {
                    State = EventSocketState.Disconnected;
                    Connect();
                    return;
                }

                client.EndConnect(result);
                BeginReceive();
                //State = EventSocketState.Connected;

            }
            catch (Exception e)
            {
                //                Log.ErrorException("ConnectCallBack Exception", e);
            }
        }

        private void OnDataReceivedCallBack(IAsyncResult result)
        {

            var client = (Socket)result.AsyncState;
            try
            {
                int bytesRead = client.EndReceive(result);
                var dataReaded = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                messageHandler.Append(dataReaded);
                FetchMessage();
                BeginReceive();
            }
            catch (Exception cex)
            {
                Log.Error("<OnDataReceivedCallBack> Exception", cex);
                //OnSocketDisconnected();
            }
        }

        #endregion

    }
}
