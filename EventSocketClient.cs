using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FsBridge.Helpers;
using FsBridge.Protocol.Commands;
using FsBridge.Protocol.Events;
using NetCoreServer;

namespace FsBridge
{

    public enum EventSocketClientState
    {
        Closed,
        Connecting,
        Connected,
        Authenticating,
        AuthenticationFailed,
        Settings,
        SettingsFailed,
        Closing
    }

    public class EventSocketClient : TcpClient
    {
        public delegate void OnEventDelegate(EventBase evnt);
        public event OnEventDelegate OnEvent;

        static string DumpFilePath = "d:\\fsbridgedump.txt";
        MessageParser _msgParser = new MessageParser();

        EventSocketClientState _state = EventSocketClientState.Closed;
        int? _fixedRcvSegmentSize;
        public EventSocketClient(string address, int port) : base(address, port)
        {
            base.OptionKeepAlive = true;
            base.OptionNoDelay = true;
            //base.OptionReceiveBufferSize = 32;
            base.OptionTcpKeepAliveRetryCount = 16;
            base.ConnectAsync();

#if DEBUG
            if (File.Exists (DumpFilePath)) File.Delete (DumpFilePath);
#endif

        }

        protected override void OnConnecting()
        {
            SetClientState(EventSocketClientState.Connecting);
            base.OnConnecting();
        }

        protected override void OnConnected()
        {
            SetClientState(EventSocketClientState.Connected);
            base.OnConnected();
        }

        protected override void OnDisconnected()
        {
            SetClientState(EventSocketClientState.Closed);
            base.OnDisconnected();
        }

        protected override void OnDisconnecting()
        {
            SetClientState(EventSocketClientState.Closing);
            base.OnDisconnecting();
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            
#if DEBUG
            File.AppendAllText(DumpFilePath, Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
#endif
            _msgParser.Append(new ArraySegment<byte>(buffer, (int)offset, (int)size));
            while (_msgParser.ReadMessage(out var msg, out var msgType))
            {
                try
                {

                    switch (msgType)
                    {
                        case MessageType.AuthRequest:
                            SetClientState(EventSocketClientState.Authenticating);
                            SendCommand(new AuthenticateCommand() { Password = "ClueCon" });
                            break;
                        case MessageType.CommandReply:
                            if (this._state == EventSocketClientState.Authenticating)
                            {
                                var authResponse = MessageParser.GetCommandReply(msg);
                                if (authResponse.Result == CommandReplyResult.Ok)
                                {
                                    SetClientState(EventSocketClientState.Settings);
                                    SendCommand(new EventCommand() { });
                                }
                            }
                            break;
                        case MessageType.Event:
                            switch (Newtonsoft.Json.JsonConvert.DeserializeObject<EventBase>(msg).EventName)
                            {
                                case EventType.Heartbeat:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<HeartbeatEvent>(msg));
                                    break;
                                case EventType.Reschedule:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<RescheduleEvent>(msg));
                                    break;
                            }
                            break;
                    }
                }
                catch (Exception xce)
                {
                    Console.WriteLine("EX");
                }
            }
            
            base.OnReceived(buffer, offset, size);
        }

        internal void SetClientState(EventSocketClientState state)
        {
            this._state = state;
        }

        internal bool SendCommand(CommandBase command)
        {
            return base.SendAsync(Encoding.UTF8.GetBytes(command.EncodeCommand()));
        }
    }
}
