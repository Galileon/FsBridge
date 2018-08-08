using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public class DTMFEvent : EventBase
    {
        public override MessageType MsgType => MessageType.Event;
             
        [JsonProperty("DTMF-Digit")]
        public string Digit { get; set; }

        public override string ToString()
        {
            return $"DtmfEvent CallId: {this.CallId} Digit: {Digit}";
        }

    }
}
