using Catel.IoC;
using Catel.MVVM;
using FsBridge.WpfClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.WpfClient.ViewModels
{
    public class CallListViewModel : ViewModelBase
    {

        FsClient.FreeswitchClient FsClient { get; set; }
        public Catel.Collections.FastObservableCollection<CallModel> Calls { get; set; } = new Catel.Collections.FastObservableCollection<CallModel>();
        public CallListViewModel(FsClient.FreeswitchClient fsClient) : base()
        {
            FsClient = fsClient;
            FsClient.OnStateChanged += FsClient_OnStateChanged;
            FsClient.OnChannelCallState += FsClient_OnChannelCallState;
            FsClient.Connect();
        }

        private void FsClient_OnChannelCallState(FsClient.FreeswitchClient client, FsClient.Protocol.Events.ChannelCallStateEvent callState)
        {
            Calls.Add(new CallModel() { CallId = callState.ChannelCallUUID, CallState = callState.ChannelCallState });
        }

        private void FsClient_OnStateChanged(FsClient.FreeswitchClient client, FsClient.Protocol.EventSocketClientState state, FsClient.Protocol.EventSocketClientState previousState)
        {

        }
    }
}
