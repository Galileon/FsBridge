﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FsBridge.FsClient.Protocol;
using FsBridge.FsClient.Protocol.Commands;
using FsBridge.FsClient.Protocol.Events;
using FsBridge.Helpers;
using NetCoreServer;

namespace FsBridge.FsClient
{

    public class EventSocketClient : TcpClient
    {
        static string DumpFilePath = "d:\\fsbridgedump.txt";

        public delegate void OnStateChangedDelegate(EventSocketClientState state, EventSocketClientState previousState);
        public delegate void OnEventDelegate(EventBase evnt);
        public event OnEventDelegate OnEvent;
        public event OnStateChangedDelegate OnStateChanged; 
        MessageParser _msgParser = new MessageParser();
        EventSocketClientState _state = EventSocketClientState.Closed;
        System.Timers.Timer _reconnectTimer;
        bool _disconnectRequired = false;
        public EventSocketClientState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                }
            }
        }
        public EventSocketClient(string address, int port) : base(address, port)
        {
            base.OptionKeepAlive = true;
            base.OptionNoDelay = true;
            base.OptionTcpKeepAliveRetryCount = 16;
#if DEBUG
            if (File.Exists (DumpFilePath)) File.Delete (DumpFilePath);
#endif
        }

        protected override void OnConnecting()
        {
            SetClientState(EventSocketClientState.Connecting);
            base.OnConnecting();
        }
        protected override void OnConnected()
        {
            // socket lib. bug -> we receivng connected after receive ..  so to avoid state missunderstanding 
            if (this.State == EventSocketClientState.Connecting) SetClientState(EventSocketClientState.Connected);
            base.OnConnected();
        }
        protected override void OnDisconnected()
        {
            SetClientState(EventSocketClientState.Closed);
            if (!_disconnectRequired && _reconnectTimer == null)
            {
                _reconnectTimer = new System.Timers.Timer()
                {
                    Enabled = true,
                    Interval = 2500,
                    AutoReset = false
                };
                _reconnectTimer.Elapsed += (s, r) => { _reconnectTimer.Close(); _reconnectTimer = null; ConnectAsync(); };
            }
            base.OnDisconnected();
        }
        protected override void OnDisconnecting()
        {
            SetClientState(EventSocketClientState.Closing);
            base.OnDisconnecting();
        }

        public void Close()
        {
            _disconnectRequired = true;
            this.Close();
        }
        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            _Trace("Receive",Encoding.UTF8.GetString(buffer, (int)offset, (int)size));
            _msgParser.Append(new ArraySegment<byte>(buffer, (int)offset, (int)size));
            while (_msgParser.ReadMessage(out var msg, out var msgType))
            {
                try
                {
                    switch (msgType)
                    {
                        case MessageType.AuthRequest:
                            SetClientState(EventSocketClientState.Authenticating);
                            SendCommand(new AuthenticateCommand() { Password = "ClueCon" });
                            break;
                        case MessageType.CommandReply:
                            if (this._state == EventSocketClientState.Authenticating)
                            {
                                var authResponse = MessageParser.GetCommandReply(msg);
                                if (authResponse.Result == CommandReplyResult.Ok)
                                {
                                    SetClientState(EventSocketClientState.Settings);
                                    SendCommand(new EventCommand() { });
                                }
                            }else
                            if (this._state == EventSocketClientState.Settings)
                            {
                                var authResponse = MessageParser.GetCommandReply(msg);
                                if (authResponse.Result == CommandReplyResult.Ok)
                                {
                                    SetClientState(EventSocketClientState.Receiving);
                                }else
                                {
                                    SetClientState(EventSocketClientState.SettingsFailed);
                                    // TODO: what?
                                }
                            }
                            break;
                        case MessageType.Event:
                            switch (Newtonsoft.Json.JsonConvert.DeserializeObject<EventBase>(msg).EventName)
                            {
                                case EventType.Heartbeat:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<HeartbeatEvent>(msg));
                                    break;
                                case EventType.Reschedule:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<RescheduleEvent>(msg));
                                    break;
                                case EventType.ChannelCreate:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelCreateEvent>(msg));
                                    break;
                                case EventType.ChannelCallState:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelCallStateEvent>(msg));
                                    break;
                                case EventType.ChannelState:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelStateEvent>(msg));
                                    break;
                                case EventType.ChannelDestroy:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelDestroyEvent>(msg));
                                    break;
                                case EventType.ChannelHangup:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelHangupEvent>(msg));
                                    break;
                                case EventType.ChannelHangupComplete:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelHangupCompleteEvent>(msg));
                                    break;
                                case EventType.PresenceIn:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<PresenceInEvent>(msg));
                                    break;
                                case EventType.Custom:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<CustomEvent>(msg));
                                    break;
                                case EventType.ChannelAnswer:
                                    OnEvent?.Invoke(Newtonsoft.Json.JsonConvert.DeserializeObject<ChannelAnswerEvent>(msg));
                                    break;
                                default:
                                    Console.WriteLine("Uknown event!");
                                    break;
                            }
                            break;
                    }
                }
                catch (Exception xce)
                {
                    Console.WriteLine("EX" + xce.Message);
                }
            }
            
            base.OnReceived(buffer, offset, size);
        }

        internal void SetClientState(EventSocketClientState state)
        {
            if (_state != state)
            {
                var prevState = _state;
                this._state = state;
                OnStateChanged?.Invoke(state, prevState);
            }
        }

        internal bool SendCommand(CommandBase command)
        {
            var cmd = command.EncodeCommand();
            _Trace("Snd",cmd);
            return base.SendAsync(Encoding.UTF8.GetBytes(cmd));
        }

        void _Trace (string operation, string msg)
        {
#if DEBUG
            File.AppendAllText(DumpFilePath,$"-------------------------------------------------------------------------------------------{Environment.NewLine}");
            File.AppendAllText(DumpFilePath,$"{operation}{Environment.NewLine}{msg}");
#endif
        }
    }
}