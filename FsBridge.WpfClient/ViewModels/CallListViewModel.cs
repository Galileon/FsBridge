using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.MVVM;
using FsBridge.FsClient.Protocol.Events;
using FsBridge.WpfClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace FsBridge.WpfClient.ViewModels
{
    public class CallListViewModel : ViewModelBase
    {

        FsClient.FreeswitchClient FsClient { get; set; }
        public Catel.Collections.FastObservableCollection<CallModel> Calls { get; set; } = new Catel.Collections.FastObservableCollection<CallModel>();
        public CallModel SelectedCall
        {
            get { return GetValue<CallModel>(SelectedCallProperty); }
            set { SetValue(SelectedCallProperty, value); }
        }
        public static readonly IPropertyData SelectedCallProperty = RegisterProperty(nameof(SelectedCall),default(CallModel), (m,s) =>
        {
        });
        public EventBase SelectedEvent
        {
            get { return GetValue<EventBase>(SelectedEventProperty); }
            set { SetValue(SelectedEventProperty, value); }
        }
        public static readonly IPropertyData SelectedEventProperty = RegisterProperty(nameof(SelectedEvent), default(EventBase));


        public CallListViewModel(FsClient.FreeswitchClient fsClient) : base()
        {
            FsClient = fsClient;
            FsClient.OnStateChanged += FsClient_OnStateChanged;
            FsClient.OnChannelCallState += FsClient_OnChannelCallState;
            FsClient.Connect();
            HangupCallCommand = new Command(() => { FsClient.HangupCall(SelectedCall.CallId); }, () => SelectedCall?.CallState == FsBridge.FsClient.Protocol.FsCallState.Active || SelectedCall?.CallState == FsBridge.FsClient.Protocol.FsCallState.Held);

        }

        private void FsClient_OnChannelCallState(FsClient.FreeswitchClient client, FsClient.Protocol.Events.ChannelCallStateEvent callState)
        {
            var cm = Calls.FirstOrDefault(c => c.CallId == callState.ChannelCallUUID);
            if (cm != null)
            {
                cm.CallState = callState.ChannelCallState;
            }
            else
            {
                cm = new CallModel() { CallId = callState.ChannelCallUUID, CallState = callState.ChannelCallState, Ani = callState.CallerANI, Dnis = callState.CallerDestinationNumber };
                Calls.Add(cm);
            }

            cm.Events.Add(callState);
        }

        private void FsClient_OnStateChanged(FsClient.FreeswitchClient client, FsClient.Protocol.EventSocketClientState state, FsClient.Protocol.EventSocketClientState previousState)
        {

        }


        public Command HangupCallCommand { get; private set; }

    }
}
