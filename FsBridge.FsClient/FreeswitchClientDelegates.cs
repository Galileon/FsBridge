using FsBridge.FsClient.Protocol.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient
{
    public delegate void OnChannelCallStateEventDelegate(FreeswitchClient client, ChannelCallStateEvent callState);
    public delegate void OnStateChangedDelegate(FreeswitchClient client, Protocol.EventSocketClientState state, Protocol.EventSocketClientState previousState);
}
