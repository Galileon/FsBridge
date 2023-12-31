﻿using Catel.Data;
using Catel.IoC;
using Catel.Messaging;
using Catel.MVVM;
using FsBridge.WpfClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.WpfClient.ViewModels
{
    public class StatusBarControlViewModel : ViewModelBase
    {
        FsClient.FreeswitchClient Client { get; set; }
        public FsClient.Protocol.EventSocketClientState State
        {
            get { return GetValue<FsClient.Protocol.EventSocketClientState>(StateProperty); }
            set { SetValue(StateProperty, value); }
        }
        /// <summary>
        /// Register the Name property so it is known in the class.
        /// </summary>
        public static readonly IPropertyData StateProperty = RegisterProperty("State", () => FsClient.Protocol.EventSocketClientState.Closed);
        public string LastCommandResult
        {
            get { return GetValue<string>(LastCommandResultProperty); }
            set { SetValue(LastCommandResultProperty, value); }
        }
        public static readonly IPropertyData LastCommandResultProperty = RegisterProperty(nameof(LastCommandResult), () => "");
        public StatusBarControlViewModel(FsClient.FreeswitchClient client)
        {
            //AlwaysInvokeNotifyChanged = true;
            ServiceLocator.Default.ResolveType<IMessageMediator>()?.Register<DebugMessage>(this, (m) => LastCommandResult = m.Message);
            Client = client;
            Client.OnStateChanged += FsClient_OnStateChanged;
            State = client.State;
        }
        private void FsClient_OnStateChanged(FsClient.FreeswitchClient client, FsClient.Protocol.EventSocketClientState state, FsClient.Protocol.EventSocketClientState previousState)
        {
            State = state;
        }
    }
}
