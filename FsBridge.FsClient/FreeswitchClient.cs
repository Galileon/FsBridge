using FsBridge.FsClient.Helpers;
using FsBridge.FsClient.Protocol;
using FsBridge.FsClient.Protocol.Commands;
using FsBridge.FsClient.Protocol.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient
{
    public class FreeswitchClient
    {
        public FreeswitchConfiguration Configuration { get; private set; }
        internal ILogger _log;
        private EventSocketClient _eClient;
        private MultiActionInvoker<object> _invoker = new MultiActionInvoker<object>();
        public event OnChannelCallStateEventDelegate OnChannelCallState;
        public event OnStateChangedDelegate OnStateChanged;
        public Protocol.EventSocketClientState State { get => _eClient?.State ?? Protocol.EventSocketClientState.Closed; }
        public FreeswitchClient(FreeswitchConfiguration config, ILogger logger)
        {
            Configuration = config;
            _log = logger;
            _eClient = new EventSocketClient(config.Address, config.Port, config.Password);
            _eClient.OnEvent += _eClient_OnEvent;
            _eClient.OnStateChanged += _eClient_OnStateChanged;
            _eClient.OnCommandReply += _eClient_OnCommandReply;
        }
        private void _eClient_OnCommandReply(EventSocketClient client, Action<CommandReply> action, CommandReply parameter) => _invoker.Invoke(Guid.Empty, action, parameter);
        private void _eClient_OnStateChanged(EventSocketClient client, Protocol.EventSocketClientState state, Protocol.EventSocketClientState previousState) => _invoker.Invoke(null, () => OnStateChanged?.Invoke(this, state, previousState));
        private void _eClient_OnEvent(EventSocketClient client, Protocol.Events.EventBase evnt)
        {
            switch (evnt)
            {
                case ChannelCallStateEvent ccse:
                    if (OnChannelCallState != null && (Configuration?.Context == "" || Configuration.Context.ToLower().Contains(ccse.CallerContext.ToLower()))) _invoker.Invoke(ccse.ChannelCallUUID, () => OnChannelCallState(this, ccse));
                    break;
            }
        }
        public void Connect()
        {
            if (!_eClient.ConnectAsync()) throw new Exception("Cannot connect.");
        }
        public void Close()
        {
            _invoker.Stop();
            _eClient?.Close();
        }
        public bool HangupCall(Guid callId, FsEventCause cause = FsEventCause.NORMAL_CLEARING, Action<CommandReply>? onReply = null)
        {
            return _eClient.SendCommand(new HangupCommand(callId, cause), callId, onReply);
        }
        public bool AnswerCall(Guid callId, Action<CommandReply>? onReply = null)
        {
            return _eClient.SendCommand(new AnswerCommand(callId), callId, onReply);
        }
        public bool MakeCall(Guid callId, string destintionNumber, Action<CommandReply>? onReply = null)
        {
            var cmd = new MakeCallCommand()
            {
                CallId = callId,
                CalledNumber = GetOriginatePhoneNumber(destintionNumber),
                Context = "mediaproxy"//Configuration.Context
            };

            return _eClient.SendCommand(cmd, callId, onReply);
        }
        private string GetOriginatePhoneNumber(string calledNumber)
        {
            if (Configuration.IgnorePhoneNumberFormatting) return calledNumber;
            var userCalling = !calledNumber.Contains("@") && !calledNumber.Contains("%");
            var num = $"sofia/{Configuration.CallingProfile}/{calledNumber}";
            if (userCalling) num = $"user/{calledNumber}";
            return num;
        }
    }
}
