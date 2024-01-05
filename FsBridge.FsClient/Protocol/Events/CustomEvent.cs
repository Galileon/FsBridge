using Newtonsoft.Json;
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
        
        public string Gateway { get; set; }
        public string State { get; set; }

        [JsonProperty("Ping-Status")]
        public string PingStatus { get; set; }
    }


}
