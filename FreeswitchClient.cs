using AxCommon.Log;
using FsConnect.Protocol;
using FsConnect.Protocol.Commands;
using FsConnect.Protocol.Events;
using FsConnect.Util;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsConnect
{

    public class FreeswitchClient
    {

        #region Log

        internal AxLogger Log;

        #endregion

        #region Enums

        public enum EventSocketState
        {
            Disconnected,
            Connecting,
            Connected,
        }

        #endregion

        #region Properties

        /// <summary>
        /// Instance of connection to freeswitch
        /// </summary>
        public int InstanceId { get; set; }

        public string Password { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string TraceFile { get; set; }

        public bool TraceEnabled { get; set; }

        FreeswitchSocketConnection EventSocketConnection { get; set; }

        FreeswitchSocketConnection CommandSocketConnection { get; set; }

        private string lastCallId;

        EventSocketState _state;
        EventSocketState State
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
                    OnConnectionStateChanged?.Invoke(this, value);
                }
            }
        }

        /// <summary>
        /// Context used to transfer calls
        /// </summary>
        public string TransferContext { get; set; }

        #endregion

        #region Delegates And Events

        public delegate void OnConnectionStateChangedDelegate(FreeswitchClient client, EventSocketState state);

        public delegate void OnCallStateChanged2EventDelegate(FreeswitchClient client, Guid callId, FsCallState newCallState, FsEventCause? cause);

        public delegate void OnCallStateChangedEventDelegate(FreeswitchClient client, CallStateEvent csEvent);

        public delegate void OnPlayOperationCompletedEventDelegate(FreeswitchClient client, PlaybackCompletedEvent playbackCompleted);

        public delegate void OnRecordOperationCompletedEventDelegate(FreeswitchClient client, RecordCompletedEvent recordCompleted);

        public delegate void OnBlindTransferOperationCompletedDelegate(FreeswitchClient client, BlindTransferCompletedEvent transferCompleted);

        public delegate void OnErrorDelegate(FreeswitchClient client, Exception exception);

        public delegate void OnDtmfDetectedEventDelegate(FreeswitchClient client, DTMFEvent dtmfEvent);



        public event OnConnectionStateChangedDelegate OnConnectionStateChanged;

        public event OnCallStateChangedEventDelegate OnCallStateChangedEvent;

        public event OnCallStateChanged2EventDelegate OnCallStateChanged2Event;

        public event OnPlayOperationCompletedEventDelegate OnPlayOperationCompletedEvent;

        public event OnRecordOperationCompletedEventDelegate OnRecordOperationCompletedEvent;

        public event OnBlindTransferOperationCompletedDelegate OnBlindTransferOperationCompleted;

        public event OnDtmfDetectedEventDelegate OnDtmfDetectedEvent;

        public event OnErrorDelegate OnError;

        #endregion

        #region .ctor 

        public FreeswitchClient(string freeswtichHost, int freeswitchPort, string freeswitchParssword, int instanceId = 0, string loggerName = "FreeswitchClient")
        {
            this.Log = AxLogManager.GetInstance().GetLogger($"{loggerName}");
            this.Host = freeswtichHost;
            this.Port = freeswitchPort;
            this.Password = freeswitchParssword;
            this.InstanceId = instanceId;
            Construct();
        }

        #endregion

        private void Construct()
        {

            if (EventSocketConnection == null)
            {
                this.EventSocketConnection = new FreeswitchSocketConnection(EventSocketConnectionType.Event, this) { Host = this.Host, Port = this.Port, Password = this.Password };
                this.EventSocketConnection.OnConnectionStateChanged += EventSocketConnection_OnConnectionStateChanged;
                this.EventSocketConnection.OnFsMessageReceived += EventSocketConnection_OnFsMessageReceived;
            }

            if (CommandSocketConnection == null)
            {
                this.CommandSocketConnection = new FreeswitchSocketConnection(EventSocketConnectionType.Command, this) { Host = this.Host, Port = this.Port, Password = this.Password, ConnectionType = EventSocketConnectionType.Command };
                this.CommandSocketConnection.OnConnectionStateChanged += CommandSocketConnection_OnConnectionStateChanged;
                this.CommandSocketConnection.OnFsMessageReceived += CommandSocketConnection_OnFsMessageReceived;
            }
        }

        private void CommandSocketConnection_OnFsMessageReceived(EventBase fsEvent)
        {
            var backgroundJobEvent = fsEvent as BackgroundJobEvent;
            if (backgroundJobEvent != null)
            {
                Log.Debug($"<BackgroundJobEvent><{backgroundJobEvent.RequestId}> {backgroundJobEvent}");

                if (backgroundJobEvent.Command == "originate" && backgroundJobEvent.Error)
                {
                    this.OnCallStateChanged2Event?.Invoke(this,Guid.Parse (backgroundJobEvent.CommandArgs.GetFreeswitchCommandParamsValue("origination_uuid")),FsCallState.Disconnected , backgroundJobEvent.Cause);
                }

                
            }
            
            //origination_uuid

        }


        private void CommandSocketConnection_OnConnectionStateChanged(EventSocketState state)
        {
            OnFsConStateChanged();
        }

        private void EventSocketConnection_OnConnectionStateChanged(EventSocketState state)
        {
            OnFsConStateChanged();
        }

        private void OnFsConStateChanged()
        {
            if (this.EventSocketConnection.State == EventSocketState.Connecting || this.CommandSocketConnection.State == EventSocketState.Connecting) this.State = EventSocketState.Connecting;
            if (this.EventSocketConnection.State == EventSocketState.Connected && this.EventSocketConnection.State == EventSocketState.Connected) this.State = EventSocketState.Connected;

        }

        private void EventSocketConnection_OnFsMessageReceived(EventBase fsEvent)
        {

            try
            {
                switch (fsEvent.EventType)
                {
                    case EventType.BackgroundEvent:
                        var backgroundJobEvent = fsEvent as BackgroundJobEvent;
                        //Log.Debug($"<BackgroundJobEvent><{backgroundJobEvent.RequestId}> {backgroundJobEvent}");
                        break;

                    case EventType.ChannelCallState:
                        if ((fsEvent as CallStateEvent).Direction == FsCallDirection.Inbound) this.lastCallId = fsEvent.CallId;
                        if (this.OnCallStateChangedEvent != null) OnCallStateChangedEvent(this, fsEvent as CallStateEvent);
                        break;

                    case EventType.DtmfEvent:
                        if (OnDtmfDetectedEvent != null) OnDtmfDetectedEvent(this, fsEvent as DTMFEvent);
                        break;

                    case EventType.RecordStopEvent:
                        if (OnRecordOperationCompletedEvent != null) OnRecordOperationCompletedEvent(this, fsEvent as RecordCompletedEvent);
                        break;

                    case EventType.ChannelExecuteCompleteEvent:
                        var excetueCompleteEvent = fsEvent as ChannelExecuteCompleteEvent;

                        //Console.WriteLine($"ExecCompleted: {excetueCompleteEvent.Application}");

                        switch (excetueCompleteEvent.Application)
                        {

                            #region Playback

                            case "playback":

                                var pce = new PlaybackCompletedEvent() { CallId = excetueCompleteEvent.CallId };

                                switch (excetueCompleteEvent.Response)
                                {
                                    case "FILE NOT FOUND":
                                        pce.PlayResult = FsPlaybackStatus.FileNotFound;
                                        break;
                                    case "BREAK":
                                        pce.PlayResult = FsPlaybackStatus.Break;
                                        break;
                                    case "FILE PLAYED":
                                        pce.PlayResult = FsPlaybackStatus.Done;
                                        break;
                                    case "ERROR":
                                        pce.PlayResult = FsPlaybackStatus.Error;
                                        break;
                                    case "PLAYBACK ERROR": // HMMM
                                        pce.PlayResult = FsPlaybackStatus.Disconnected;
                                        break;
                                }

                                if (this.OnPlayOperationCompletedEvent != null) OnPlayOperationCompletedEvent(this, pce);

                                break;

                            #endregion

                            #region blind Transfer

                            case "transfer":
                                var tce = new BlindTransferCompletedEvent() { CallId = excetueCompleteEvent.CallId, TransferResult = FsBlindTrasnferStatus.Transferred };
                                if (this.OnBlindTransferOperationCompleted != null) OnBlindTransferOperationCompleted(this, tce);
                                break;

                                #endregion

                        }
                        break;

                }
            }
            catch (Exception ce)
            {
                //   Console.WriteLine(ce.Message);
            }
        }

        public void Connect()
        {
            Log.Debug($"<Connect> Connecting ... Host: {this.Host} Port: {this.Port} ... ");
            try
            {
                Construct();
                State = EventSocketState.Connecting;
                EventSocketConnection.Connect();
                CommandSocketConnection.Connect();
            }
            catch (Exception cex)
            {
                Log.ErrorException("<Connect> Unable to connect", cex);
                this.State = EventSocketState.Disconnected;
            }
        }

        public void Close()
        {
            try
            {
                Log.Debug($"<Close> Close called");
                //if (this.workSocket != null && this.workSocket.Connected) this.workSocket.Disconnect(false);
                //if (this.ResponseHandler != null) this.ResponseHandler.Clear();
            }
            catch (Exception cex)
            {

            }
        }

        #region Basic Call Methods

        public CommandReply PickupCall(Guid callId)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            CommandSocketConnection.CallCommandSync(SetCommand.Encode(callId, "park_after_bridge", "true"), "PickupCall");
            CommandSocketConnection.CallCommandSync(SetCommand.Encode(callId, "hangup_after_bridge", "false"), "PickupCall");
            return CommandSocketConnection.CallCommandSync(AnswerCommand.Encode(callId.ToString()), "Pickup");
        }

        public CommandReply MakeCall(Guid callId, string destination, string ani, bool AutoAnswer = false, int timeOut = 30, Guid? bridgeWithCallId = null)
        {
            var res = CommandSocketConnection.CallCommandSync(MakeCallCommand.Encode(this, callId, destination, ani, "", AutoAnswer, timeOut, bridgeWithCallId), "MakeCall", true,
                $" Dnis: {destination} Ani: {ani} AutoAnswer: {AutoAnswer} NoAnswer: {timeOut}");
            return res;
        }

        public CommandReply Hold(Guid callId)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(HoldCommand.Encode(callId), "Hold");
        }

        public CommandReply Retrieve(Guid callId)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(RetrieveCommand.Encode(callId), "Retrieve");
        }

        public CommandReply Hangup(Guid callId, SipResponseCode code)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(HangupCommand.Encode(callId, code), "Hangup", true, $" CallId: {callId} SipCause: {code}");
        }

        public CommandReply BlindTransfer(Guid callId, string destination)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(BlindTransferCommand.Encode(this, callId, destination), "BlindTransfer", true, $" CallId: {callId} Destination: {destination}");
        }


        #endregion

        #region Call Audio

        public CommandReply MuteCall(Guid callId, bool mute)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(MuteCommand.Encode(callId, mute), "Mute");
        }


        #endregion

        #region Dtmf

        public CommandReply SendDtmf(Guid callId, string dtmfString)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(SendDtmfCommand.Encode(callId, dtmfString), "SendDtmf");
        }

        #endregion

        #region Voice Playing

        public CommandReply PlayFile(Guid callId, string filePath, string terminateDtmf = "", AudioDestination destination = AudioDestination.Both)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            if (string.IsNullOrEmpty(terminateDtmf)) terminateDtmf = "none";
            filePath = filePath.Replace(@"\", @"\\");
            if (CommandSocketConnection.CallCommandSync(SetCommand.Encode(callId, "playback_terminators", terminateDtmf), "playback_set_terminators", true).Result != CommandReply.CommandResult.OK) return new CommandReply() { Result = CommandReply.CommandResult.Failed };
            return CommandSocketConnection.CallCommandSync(PlaybackCommand.Encode(callId, filePath, destination), "PlayFile");
        }

        public CommandReply StopPlay(Guid callId)
        {
            //return new CommandReply() { Result = CommandReply.CommandResult.OK };
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(StopPlaybackCommand.Encode(callId), "StopPlay");
        }


        public CommandReply Displace(Guid callId, string filePath)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            filePath = filePath.Replace(@"\", @"\\");
            return CommandSocketConnection.CallCommandSync(DisplaceCommand.Encode(callId, filePath), "Displace");
        }

        public CommandReply StopDisplace(Guid callId)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(StopDisplaceCommand.Encode(callId), "StopDisplace");
        }

        public CommandReply PlayTone(Guid callId, FsTone tone, int howManyTimes = 10, AudioDestination destination = AudioDestination.Both)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            string toneParam = "";
            switch (tone)
            {
                case FsTone.Busy:
                    toneParam = $"tone_stream://L={howManyTimes};%(500,600,440)";
                    break;
                case FsTone.Unavaileable:
                    toneParam = $"tone_stream://L={howManyTimes};%(274,50,913.8);%(274,50,1370.6);%(380,1500,1776.7);";
                    break;
                case FsTone.Ringback:
                    toneParam = $"tone_stream://L={howManyTimes};%(1000, 2500, 400);";
                    break;
                    
                default:
                    toneParam = $"tone_stream://L={howManyTimes};%(500,550,440)";
                    break;
            }

            return CommandSocketConnection.CallCommandSync(PlaybackCommand.Encode(callId, toneParam, destination), "PlayTone");
        }

        #endregion

        #region Voice Recording

        public CommandReply Record(Guid callId, string filePath, bool stereo = false)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            // FAKE
            CommandSocketConnection.CallCommandSync(SetCommand.Encode(callId, "RECORD_STEREO", stereo.ToString()), "Record-SetStereo", false);
            return CommandSocketConnection.CallCommandSync(RecordCommand.Encode(callId, filePath), "Record", stereo);
        }

        public CommandReply StopRecord(Guid callId)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(StopRecordCommand.Encode(callId), "StopRecord");
        }

        #endregion

        #region Channel Management

        public CommandReply Bridge(Guid callId, Guid secondCallId)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            //if (CommandSocketConnection.CallCommandSync(SetCommand.Encode(secondCallId, "park_after_bridge", "true")).Result != CommandReply.CommandResult.OK) return new CommandReply() { Result = CommandReply.CommandResult.Failed };
            return CommandSocketConnection.CallCommandSync(BridgeCommand.Encode(callId, secondCallId), "Bridge", true, $"PrimaryCallId: {callId} SeconaryCallId: {secondCallId}");
        }

        public CommandReply Bridge(Guid callId, Guid secondCallId, Guid thirdCallId)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            if (CommandSocketConnection.CallCommandSync(SetCommand.Encode(secondCallId, "park_after_bridge", "true")).Result != CommandReply.CommandResult.OK) return new CommandReply() { Result = CommandReply.CommandResult.Failed };
            if (CommandSocketConnection.CallCommandSync(SetCommand.Encode(secondCallId, "hangup_after_bridge", "false")).Result != CommandReply.CommandResult.OK) return new CommandReply() { Result = CommandReply.CommandResult.Failed };
            return CommandSocketConnection.CallCommandSync(BridgeCommand.Encode(callId, secondCallId, thirdCallId), "Bridge");
        }

        public CommandReply StopBridge(Guid callId)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(EndBridgeCommand.Encode(callId), "StopBridge", true, $"CallId: {callId}");
        }

        public CommandReply ThreeWay(string callId, string secondCallId)
        {
            if (string.IsNullOrEmpty(callId)) callId = lastCallId;
            return CommandSocketConnection.CallCommandSync(ThreeWayCommand.Encode(callId, secondCallId), "ThreeWay");
        }

        public CommandReply SetMedia(Guid callId, bool enable)
        {
            if (callId == Guid.Empty && !string.IsNullOrEmpty(lastCallId)) callId = Guid.Parse(lastCallId);
            return CommandSocketConnection.CallCommandSync(MediaCommand.Encode(callId, enable), "SetMedia");
        }

        #endregion

        #region Helpers

        internal string GetOriginatePhoneNumber(string destNumber)
        {
            var userCalling = !destNumber.Contains("@") && !destNumber.Contains("%");
            var num = $"sofia/mediastack/{destNumber}";
            if (userCalling) num = $"user/{destNumber}";
            return num;
        }

        #endregion

    }

}
