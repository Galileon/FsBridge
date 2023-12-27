using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Events
{
    internal class ChannelCallStateEvent : EventBase
    {
        [JsonProperty("Core-UUID")]
        public string CoreUUID { get; set; }

        [JsonProperty("FreeSWITCH-Hostname")]
        public string FreeSWITCHHostname { get; set; }

        [JsonProperty("FreeSWITCH-Switchname")]
        public string FreeSWITCHSwitchname { get; set; }

        [JsonProperty("FreeSWITCH-IPv4")]
        public string FreeSWITCHIPv4 { get; set; }

        [JsonProperty("FreeSWITCH-IPv6")]
        public string FreeSWITCHIPv6 { get; set; }

        [JsonProperty("Event-Date-Local")]
        public string EventDateLocal { get; set; }

        [JsonProperty("Event-Date-GMT")]
        public string EventDateGMT { get; set; }

        [JsonProperty("Event-Date-Timestamp")]
        public string EventDateTimestamp { get; set; }

        [JsonProperty("Event-Calling-File")]
        public string EventCallingFile { get; set; }

        [JsonProperty("Event-Calling-Function")]
        public string EventCallingFunction { get; set; }

        [JsonProperty("Event-Calling-Line-Number")]
        public string EventCallingLineNumber { get; set; }

        [JsonProperty("Event-Sequence")]
        public string EventSequence { get; set; }

        [JsonProperty("Original-Channel-Call-State")]
        public string OriginalChannelCallState { get; set; }

        [JsonProperty("Channel-Call-State-Number")]
        public string ChannelCallStateNumber { get; set; }

        [JsonProperty("Channel-State")]
        public string ChannelState { get; set; }

        [JsonProperty("Channel-Call-State")]
        public string ChannelCallState { get; set; }

        [JsonProperty("Channel-State-Number")]
        public string ChannelStateNumber { get; set; }

        [JsonProperty("Channel-Name")]
        public string ChannelName { get; set; }

        [JsonProperty("Unique-ID")]
        public string UniqueID { get; set; }

        [JsonProperty("Call-Direction")]
        public FsCallDirection CallDirection { get; set; }

        [JsonProperty("Presence-Call-Direction")]
        public string PresenceCallDirection { get; set; }

        [JsonProperty("Channel-HIT-Dialplan")]
        public string ChannelHITDialplan { get; set; }

        [JsonProperty("Channel-Presence-ID")]
        public string ChannelPresenceID { get; set; }

        [JsonProperty("Channel-Call-UUID")]
        public string ChannelCallUUID { get; set; }

        [JsonProperty("Answer-State")]
        public string AnswerState { get; set; }

        [JsonProperty("Hangup-Cause")]
        public string HangupCause { get; set; }

        [JsonProperty("Caller-Direction")]
        public string CallerDirection { get; set; }

        [JsonProperty("Caller-Logical-Direction")]
        public string CallerLogicalDirection { get; set; }

        [JsonProperty("Caller-Username")]
        public string CallerUsername { get; set; }

        [JsonProperty("Caller-Dialplan")]
        public string CallerDialplan { get; set; }

        [JsonProperty("Caller-Caller-ID-Name")]
        public string CallerCallerIDName { get; set; }

        [JsonProperty("Caller-Caller-ID-Number")]
        public string CallerCallerIDNumber { get; set; }

        [JsonProperty("Caller-Orig-Caller-ID-Name")]
        public string CallerOrigCallerIDName { get; set; }

        [JsonProperty("Caller-Orig-Caller-ID-Number")]
        public string CallerOrigCallerIDNumber { get; set; }

        [JsonProperty("Caller-Network-Addr")]
        public string CallerNetworkAddr { get; set; }

        [JsonProperty("Caller-ANI")]
        public string CallerANI { get; set; }

        [JsonProperty("Caller-Destination-Number")]
        public string CallerDestinationNumber { get; set; }

        [JsonProperty("Caller-Unique-ID")]
        public string CallerUniqueID { get; set; }

        [JsonProperty("Caller-Source")]
        public string CallerSource { get; set; }

        [JsonProperty("Caller-Context")]
        public string CallerContext { get; set; }

        [JsonProperty("Caller-Channel-Name")]
        public string CallerChannelName { get; set; }

        [JsonProperty("Caller-Profile-Index")]
        public string CallerProfileIndex { get; set; }

        [JsonProperty("Caller-Profile-Created-Time")]
        public string CallerProfileCreatedTime { get; set; }

        [JsonProperty("Caller-Channel-Created-Time")]
        public string CallerChannelCreatedTime { get; set; }

        [JsonProperty("Caller-Channel-Answered-Time")]
        public string CallerChannelAnsweredTime { get; set; }

        [JsonProperty("Caller-Channel-Progress-Time")]
        public string CallerChannelProgressTime { get; set; }

        [JsonProperty("Caller-Channel-Progress-Media-Time")]
        public string CallerChannelProgressMediaTime { get; set; }

        [JsonProperty("Caller-Channel-Hangup-Time")]
        public string CallerChannelHangupTime { get; set; }

        [JsonProperty("Caller-Channel-Transfer-Time")]
        public string CallerChannelTransferTime { get; set; }

        [JsonProperty("Caller-Channel-Resurrect-Time")]
        public string CallerChannelResurrectTime { get; set; }

        [JsonProperty("Caller-Channel-Bridged-Time")]
        public string CallerChannelBridgedTime { get; set; }

        [JsonProperty("Caller-Channel-Last-Hold")]
        public string CallerChannelLastHold { get; set; }

        [JsonProperty("Caller-Channel-Hold-Accum")]
        public string CallerChannelHoldAccum { get; set; }

        [JsonProperty("Caller-Screen-Bit")]
        public string CallerScreenBit { get; set; }

        [JsonProperty("Caller-Privacy-Hide-Name")]
        public string CallerPrivacyHideName { get; set; }

        [JsonProperty("Caller-Privacy-Hide-Number")]
        public string CallerPrivacyHideNumber { get; set; }
    }

}
