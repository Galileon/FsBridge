using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.Protocol.Events
{
    public class HeartbeatEvent : EventBase
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

        [JsonProperty("Event-Info")]
        public string EventInfo { get; set; }

        [JsonProperty("Up-Time")]
        public string UpTime { get; set; }

        [JsonProperty("FreeSWITCH-Version")]
        public string FreeSWITCHVersion { get; set; }

        [JsonProperty("Uptime-msec")]
        public string Uptimemsec { get; set; }

        [JsonProperty("Session-Count")]
        public string SessionCount { get; set; }

        [JsonProperty("Max-Sessions")]
        public string MaxSessions { get; set; }

        [JsonProperty("Session-Per-Sec")]
        public string SessionPerSec { get; set; }

        [JsonProperty("Session-Per-Sec-Last")]
        public string SessionPerSecLast { get; set; }

        [JsonProperty("Session-Per-Sec-Max")]
        public string SessionPerSecMax { get; set; }

        [JsonProperty("Session-Per-Sec-FiveMin")]
        public string SessionPerSecFiveMin { get; set; }

        [JsonProperty("Session-Since-Startup")]
        public string SessionSinceStartup { get; set; }

        [JsonProperty("Session-Peak-Max")]
        public string SessionPeakMax { get; set; }

        [JsonProperty("Session-Peak-FiveMin")]
        public string SessionPeakFiveMin { get; set; }

        [JsonProperty("Idle-CPU")]
        public string IdleCPU { get; set; }
    }

}
