using FsConnect.CallModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect
{
    /// <summary>
    /// New improoved version of telephony client 
    /// </summary>
    public class SwitchClient
    {

        #region Properties

        internal SwitchClientSettings Settings { get; set; }

        public Dictionary<int, FreeswitchClient> Clients = new Dictionary<int, FreeswitchClient>();

        private SwitchCallDigitBuffers DigitBuffers { get; set; }

        public SwitchCallCollection Calls = new SwitchCallCollection();

        #endregion

        #region Events

        public event SwitchClientEvents.OnConnectionStateChangedDelegate OnConnectionStateChanged;

        public event SwitchClientEvents.OnCallStateChangedDelegate OnCallStateChanged;

        public event SwitchClientEvents.OnPlayOperationCompletedDelegate OnPlayOperationCompleted;

        public event SwitchClientEvents.OnRecordOperationCompletedDelegate OnRecordOperationCompleted;

        public event SwitchClientEvents.OnDtmfPressedDelegate OnDtmfPressed;

        #endregion

        #region .ctor

        public SwitchClient(SwitchClientSettings settings)
        {
            this.DigitBuffers = new SwitchCallDigitBuffers(this);
            this.Settings = settings;
        }

        #endregion

        #region Methods

        public void CreateConnections()
        {
            Settings.Connections.ForEach(_ =>
            {
                var fsClient = new FreeswitchClient(_.SocketAddress, _.SocketPort, _.Password, _.SwitchId);
                fsClient.OnConnectionStateChanged += FsClient_OnConnectionStateChanged;
                fsClient.OnCallStateChangedEvent += FsClient_OnCallStateChangedEvent;
                fsClient.OnCallStateChanged2Event += FsClient_OnCallStateChanged2Event;
                fsClient.OnPlayOperationCompletedEvent += FsClient_OnPlayOperationCompletedEvent;
                fsClient.OnDtmfDetectedEvent += FsClient_OnDtmfDetectedEvent;
                fsClient.OnRecordOperationCompletedEvent += FsClient_OnRecordOperationCompletedEvent;

                fsClient.TraceEnabled = this.Settings.TraceEnabled;
                fsClient.TraceFile = this.Settings.TraceFilePath;
                Clients[fsClient.InstanceId] = fsClient;
                fsClient.Connect();
            });

        }

        private FreeswitchClient GetClient(Guid callId)
        {
            var call = this.Calls.GetCall(callId);
            if (call == null) return null;
            if (!this.Clients.TryGetValue(call.SwitchId, out FreeswitchClient client)) return null;
            return client;
        }

        #endregion

        #region FsClient Events

        private void FsClient_OnConnectionStateChanged(FreeswitchClient client, FreeswitchClient.EventSocketState state)
        {
            switch (state)
            {
                case FreeswitchClient.EventSocketState.Connected:
                    OnConnectionStateChanged?.Invoke(client.InstanceId, SwitchConnectionState.Connected);
                    break;
                case FreeswitchClient.EventSocketState.Connecting:
                    OnConnectionStateChanged?.Invoke(client.InstanceId, SwitchConnectionState.Connecting);
                    break;
                case FreeswitchClient.EventSocketState.Disconnected:
                    OnConnectionStateChanged?.Invoke(client.InstanceId, SwitchConnectionState.Disconnected);
                    break;
            }
        }

        private void FsClient_OnCallStateChangedEvent(FreeswitchClient client, Protocol.CallStateEvent csEvent)
        {
            if (csEvent.Direction == FsCallDirection.Inbound && !this.Calls.HasCall(client.InstanceId, csEvent.CallIdGuid) && !this.Settings.IsContextValid(client.InstanceId, csEvent.Context)) return;
            var call = Calls.GetCall(Guid.Parse(csEvent.CallId));
            if (call != null)
            {
                // Sometimes after reinvite we dont need to read this status
                if (call.CallState == FsCallState.Active && csEvent.CallState == FsCallState.Ringing) return;
                if (call.CallState == FsCallState.Active && csEvent.CallState == FsCallState.Active) return;
            }
            // TODO: one call instead of 2
            call = Calls.AddOrUpdateCall(client.InstanceId, csEvent);
            OnCallStateChanged?.Invoke((SwitchCall)call.Clone());
        }

        /// <summary>
        /// Sometimes we just have CallId and no more info for some silly call statuses
        /// </summary>
        /// <param name="client"></param>
        /// <param name="callId"></param>
        /// <param name="newCallState"></param>
        /// <param name="cause"></param>
        private void FsClient_OnCallStateChanged2Event(FreeswitchClient client, Guid callId, FsCallState newCallState, FsEventCause? cause)
        {
            var call = Calls.GetCall (client.InstanceId, callId);
            if (call != null)
            {
                
                call.CallState = newCallState;
                if (newCallState == FsCallState.Disconnected && cause.HasValue) call.HangupCause = cause.Value;
                OnCallStateChanged?.Invoke((SwitchCall)call.Clone());
            }
        }


        private void FsClient_OnDtmfDetectedEvent(FreeswitchClient client, Protocol.DTMFEvent dtmfEvent)
        {
            var call = Calls.GetCall(client.InstanceId, dtmfEvent.CallIdGuid);
            if (call == null) return;
            dtmfEvent.Digit.ToList().ForEach(_ =>
            {
                DigitBuffers.AppendDigit(call, _);
                OnDtmfPressed?.Invoke(call, _, DigitBuffers.GetDigits(call));
            });
        }

        private void FsClient_OnPlayOperationCompletedEvent(FreeswitchClient client, Protocol.PlaybackCompletedEvent playbackCompleted)
        {
            var call = Calls.GetCall(client.InstanceId, playbackCompleted.CallIdGuid);
            if (call == null) return;
            OnPlayOperationCompleted?.Invoke(call, playbackCompleted.PlayResult);
        }


        private void FsClient_OnRecordOperationCompletedEvent(FreeswitchClient client, Protocol.RecordCompletedEvent recordCompleted)
        {
            var call = Calls.GetCall(client.InstanceId, recordCompleted.CallIdGuid);
            if (call == null) return;
            OnRecordOperationCompleted?.Invoke(call, recordCompleted.RecordResult);
        }


        #endregion

        #region Basic Call Methods

        public FsFunctionCallResult AnswerCall(Guid switchCallId)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.PickupCall(switchCallId).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        public FsFunctionCallResult Disconnect(Guid switchCallId, SipResponseCode sipResponseCode = SipResponseCode.OK)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.Hangup(switchCallId, sipResponseCode).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        public FsFunctionCallResult Hold(Guid switchCallId)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.Hold(switchCallId).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        public FsFunctionCallResult Retrieve(Guid switchCallId)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidSwitchId;
            return client.Retrieve(switchCallId).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        #endregion

        #region Dtmf

        public FsFunctionCallResult SendDtmf(Guid switchCallId, string dtmfString)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.SendDtmf (switchCallId, dtmfString).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        #endregion

        #region Call Bus Methods

        public FsFunctionCallResult Bridge(Guid firstLeg, Guid secondLeg)
        {
            var aLegCall = this.Calls.GetCall(firstLeg);
            if (aLegCall == null) return FsFunctionCallResult.InvalidCallId;
            var bLegCall = this.Calls.GetCall(secondLeg);
            if (bLegCall == null) return FsFunctionCallResult.InvalidCallId;
            if (bLegCall.SwitchId != aLegCall.SwitchId) return FsFunctionCallResult.InvalidSwitchId;
            FreeswitchClient client = null;
            if (!this.Clients.TryGetValue(aLegCall.SwitchId, out client)) return FsFunctionCallResult.InvalidSwitchId;
            return client.Bridge(firstLeg, secondLeg).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        public FsFunctionCallResult StopBridge(Guid firstLegCallId)
        {
            var aLegCall = this.Calls.GetCall(firstLegCallId);
            if (aLegCall == null) return FsFunctionCallResult.InvalidCallId;

            FreeswitchClient client = null;
            if (!this.Clients.TryGetValue(aLegCall.SwitchId, out client)) return FsFunctionCallResult.InvalidSwitchId;
            return client.StopBridge(firstLegCallId).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        #endregion

        #region Dialing

        public FsFunctionCallResult MakeCall(string dnis, string ani, int noAnswerTimeSeconds = 30, bool autoAnswer = false, Guid? callId = null, string switchId = "")
        {
            if (!callId.HasValue) callId = Guid.NewGuid();
            FreeswitchClient client = this.Clients.FirstOrDefault().Value;
            var newCall = new SwitchCall() { Ani = ani, Dnis = dnis, CallId = callId.Value, CallState = FsCallState.Dialing, Direction = FsCallDirection.Outbound, SwitchId = client.InstanceId, Context = "default" };
            this.Calls.AddCall(client.InstanceId, newCall);

            OnCallStateChanged?.Invoke(newCall);
            //if (!this.Clients.TryGetValue(switchCall.SwitchId, out client)) return FsFunctionCallResult.InvalidSwitchId;
            var callResult = client.MakeCall(callId.Value, dnis, ani, autoAnswer, noAnswerTimeSeconds);
            if (callResult.Result != Protocol.CommandReply.CommandResult.OK)
            {
                newCall.CallState = FsCallState.Disconnected;
                newCall.HangupCause = FsEventCause.NETWORK_OUT_OF_ORDER;
                OnCallStateChanged?.Invoke(newCall);
            }

            return FsFunctionCallResult.Ok;

        }

        #endregion

        #region Voice Playing

        public FsFunctionCallResult PlayFile(Guid switchCallId, string filePath, string terminationDigits = "", AudioDestination audioDest = AudioDestination.Both)
        {
            if (!System.IO.File.Exists(filePath)) return FsFunctionCallResult.FileNotExists;
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.PlayFile(switchCallId, filePath, terminationDigits, audioDest).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        public FsFunctionCallResult PlayTone(Guid switchCallId, FsTone tone, int repeat = 2, string terminationDigits = "", AudioDestination audioDest = AudioDestination.Both)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.PlayTone (switchCallId, tone, repeat, audioDest).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }


        public FsFunctionCallResult StopPlay(Guid switchCallId)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.StopPlay (switchCallId).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        #endregion

        #region Voice Recording

        public FsFunctionCallResult Record(Guid callId, string filePath, string terminationDigits = "", bool stereo = false)
        {
            var client = GetClient(callId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.Record (callId, filePath, stereo).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        public FsFunctionCallResult StopRecord(Guid switchCallId)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.StopRecord(switchCallId).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }


        #endregion

        #region Mute

        public FsFunctionCallResult Mute(Guid switchCallId)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.MuteCall(switchCallId,true).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        public FsFunctionCallResult Unmute(Guid switchCallId)
        {
            var client = GetClient(switchCallId);
            if (client == null) return FsFunctionCallResult.InvalidCallId;
            return client.MuteCall(switchCallId, false).Result == Protocol.CommandReply.CommandResult.OK ? FsFunctionCallResult.Ok : FsFunctionCallResult.Error;
        }

        #endregion

        #region Dtmf

        public FsFunctionCallResult ClearDigitBuffer(SwitchCall switchCall)
        {
            this.DigitBuffers.Remove(switchCall);
            return FsFunctionCallResult.Ok;
        }

        #endregion

    }


}
