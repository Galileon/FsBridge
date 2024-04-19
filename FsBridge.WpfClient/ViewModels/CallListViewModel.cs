using Catel;
using Catel.Collections;
using Catel.Data;
using Catel.IoC;
using Catel.Messaging;
using Catel.MVVM;
using FsBridge.FsClient.Protocol.Events;
using FsBridge.WpfClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace FsBridge.WpfClient.ViewModels
{
    public class CallListViewModel : ViewModelBase
    {
        FsClient.FreeswitchClient FsClient { get; set; }
        public Catel.Collections.FastObservableCollection<CallModel> Calls { get; set; } = new Catel.Collections.FastObservableCollection<CallModel>();
        public CallModel SelectedCall
        {
            get => GetValue<CallModel>(SelectedCallProperty);
            set => SetValue(SelectedCallProperty, value);
        }
        public static readonly IPropertyData SelectedCallProperty = RegisterProperty(nameof(SelectedCall), default(CallModel), (m, s) => { });
        public EventBase SelectedEvent
        {
            get => GetValue<EventBase>(SelectedEventProperty);
            set => SetValue(SelectedEventProperty, value);
        }
        public static readonly IPropertyData SelectedEventProperty = RegisterProperty(nameof(SelectedEvent), default(EventBase));
        public bool AutoRemoveDisconnectedCalls { get; set; } = true;
        public MakeCallParams MakeCallProperties
        {
            get { return GetValue<MakeCallParams>(MakeCallPropertiesProperty); }
            set { SetValue(MakeCallPropertiesProperty, value); }
        }
        public static readonly IPropertyData MakeCallPropertiesProperty = RegisterProperty(nameof(MakeCallProperties), () => new MakeCallParams() { Destination = "10001@10.10.10.200:61490" });
        public CallListViewModel(FsClient.FreeswitchClient fsClient) : base()
        {
            FsClient = fsClient;
            FsClient.OnStateChanged += FsClient_OnStateChanged;
            FsClient.OnChannelCallState += FsClient_OnChannelCallState;
            FsClient.Connect();
            new DispatcherTimer(TimeSpan.FromSeconds (2),  DispatcherPriority.DataBind, (s,r) => { if (this.AutoRemoveDisconnectedCalls) this.Calls.RemoveItems(this.Calls.Where(c => c.CallState == FsBridge.FsClient.Protocol.FsCallState.Hangup && DateTime.UtcNow - c.StateChangedOn > TimeSpan.FromSeconds(5)).ToArray()); }, Dispatcher.CurrentDispatcher).Start();

            HangupCallCommand = new Command(() =>
            {
                FsClient.HangupCall(SelectedCall.CallId, FsBridge.FsClient.Protocol.FsEventCause.NORMAL_CLEARING,
                (response) =>
                {
                    ServiceLocator.Default.ResolveType<IMessageMediator>()?.SendMessage<DebugMessage>(new DebugMessage() { Message = response.Text });
                });
            }, () => SelectedCall != null && SelectedCall?.CallState != FsBridge.FsClient.Protocol.FsCallState.Hangup && SelectedCall?.CallState != FsBridge.FsClient.Protocol.FsCallState.Disconnected);

            AnswerCallCommand = new Command(() =>
            {
                FsClient.AnswerCall(SelectedCall.CallId,
                (response) =>
                {
                    ServiceLocator.Default.ResolveType<IMessageMediator>()?.SendMessage<DebugMessage>(new DebugMessage() { Message = response.Text });
                });
            }, () => SelectedCall?.CallState == FsBridge.FsClient.Protocol.FsCallState.Ringing);
            MakeCallCommand = new Command(() =>
            {
                //
                //FsClient.MakeCall(Guid.NewGuid(), "10001@10.10.10.200:5070",
                FsClient.MakeCall(Guid.NewGuid(), "10001@10.10.10.200:61490",
                (response) =>
                {
                    ServiceLocator.Default.ResolveType<IMessageMediator>()?.SendMessage<DebugMessage>(new DebugMessage() {  Message = $"{response.Text} {response.Result}" });
                });

            }, () => fsClient.State == FsBridge.FsClient.Protocol.EventSocketClientState.Receiving);

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
                cm = new CallModel() { Context = callState.CallerContext, CallId = callState.ChannelCallUUID, CallState = callState.ChannelCallState, Ani = callState.CallerANI, Dnis = callState.CallerDestinationNumber };
                Calls.Add(cm);
            }

            cm.Events.Add(callState);
            Console.WriteLine(callState.ToString());
        }
        private void FsClient_OnStateChanged(FsClient.FreeswitchClient client, FsClient.Protocol.EventSocketClientState state, FsClient.Protocol.EventSocketClientState previousState)
        {
            var s = state;
            base.ViewModelCommandManager.InvalidateCommands();
        }
        public Command HangupCallCommand { get; private set; }
        public Command AnswerCallCommand { get; private set; }
        public Command MakeCallCommand { get; private set; }

    }
}
