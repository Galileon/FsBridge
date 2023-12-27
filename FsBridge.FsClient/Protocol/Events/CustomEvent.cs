﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Events
{
    public class CustomEvent : EventBase
    { 
        [JsonProperty("Event-Subclass")]
        public string EventSubclass { get; set; }

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
        public string Gateway { get; set; }
        public string State { get; set; }

        [JsonProperty("Ping-Status")]
        public string PingStatus { get; set; }
    }


}
