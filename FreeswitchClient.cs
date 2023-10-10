﻿using AxCommon.Log;
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
            this.InstanceId = instanceId;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                           