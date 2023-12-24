using FsBridge.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.Protocol.Events
{
    public class EventBase
    {
        [JsonProperty("Event-Name")]
        public EventType  EventName { get; set; }
    }
}
