﻿using FsConnect.CallModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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

        public SwitchCallCollection Calls = new SwitchCallCollection();

        private SwitchCallDigitBuffers DigitBuffers { get; set; }

        private FreeswitchHttpFileServer HttpAudioFileServer { get; set; }

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
            if (!string.IsNullOrEmpty (this.Settings.AudioHttpServerUrl)) SetupAudioHttpServer();
        }

        private void SetupAudioHttpServer()
        {
            HttpAudioFileServer = new FreeswitchHttpFileServer();
            HttpAudioFileServer.Start(new FreeswitchHttpServerSettings()
            {
                ReadBasePath = this.Settings.AudioPromptsRoot,
                Url = this.Settings.AudioHttpServerUrl
            });
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
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               