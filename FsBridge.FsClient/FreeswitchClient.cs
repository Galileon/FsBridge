using FsBridge.FsClient.Helpers;
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
        private MultiActionInvoker _invoker = new MultiActionInvoker();
        public event OnChannelCallStateEventDelegate OnChannelCallState;
        public event OnStateChangedDelegate OnStateChanged;
        public FreeswitchClient(FreeswitchConfiguration config, ILogger logger)
        {
            Configuration = config;
            _log = logger;
            _eClient = new EventSocketClient(config.Address, config.Port, config.Password);
            _eClient.OnEvent += _eClient_OnEvent;
            _eClient.OnStateChanged += _eClient_OnStateChanged;
        }
        private void _eClient_OnStateChanged(EventSocketClient client, Protocol.EventSocketClientState state, Protocol.EventSocketClientState previousState)
        {
            _invoker.Invoke(null, () => OnStateChanged?.Invoke(this, state, previousState));
        }
        private void _eClient_OnEvent(EventSocketClient client, Protocol.Events.EventBase evnt)
        {
            switch (evnt)
            {
                case ChannelCallStateEvent ccse:
                    if (OnChannelCallState != null) _invoker.Invoke(ccse.ChannelCallUUID, () => OnChannelCallState (this, ccse));
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
    }
}
