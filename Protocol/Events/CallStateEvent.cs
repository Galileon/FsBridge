using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public class CallStateEvent : EventBase
    {

        public string Ani
        {
            get
            {
                return string.IsNullOrEmpty(CallerId) ? CallerAni : CallerId;
            }
        }



        [JsonProperty("Channel-Call-State")]
        public FsCallState CallState { get; set; }

        [JsonProperty("Call-Direction")]
        public FsCallDirection Direction { get; set; }

        [JsonProperty("Caller-ANI")]
        public string CallerAni { get; set; }

        [JsonProperty("Caller-Caller-ID-Number")]
        public string CallerId { get; set; }

        [JsonProperty("Caller-Destination-Number")]
        public string Dnis { get; set; }

        [JsonProperty("Caller-Context")]
        public string Context { get; set; }

        [JsonProperty("Hangup-Cause")]
        public FsEventCause? HangupCause { get; set; }
        
        public override string ToString()
        {
            return $"CallStateEvent CallId: {CallId} CallState: {CallState} Ani: {Ani} Dnis: {Dnis} Direction: {Direction} Context: {Context} HangupCause: {HangupCause}";
        }
    }
}
