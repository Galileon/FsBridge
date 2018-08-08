using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace FsConnect.Protocol
{
    public class EventBase : FreeswitchMessageBase
    {
        public override MessageType MsgType => MessageType.Event;

        ////[JsonConverter(typeof(StringEnumConverter))]
        [JsonProperty("Event-Name")]
        public EventType EventType { get; set; }

    }
}
