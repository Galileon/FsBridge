using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public abstract class FreeswitchMessageBase
    {
        public abstract MessageType MsgType { get; }

        public object Parse(string[] linesOfText) { return null; }

        //[JsonProperty("Channel-Call-UUID")] TAK DZIAŁA
        //public string CallId { get; set; }

        string _callId;

        //[JsonProperty("variable_call_uuid")]
        [JsonProperty("Unique-ID")]
        //[JsonProperty("Channel-Call-UUID")]
        public string CallId
        {
            get
            {
                return _callId;
            }
            set
            {
                if (this._callId != value)
                {
                    this._callId = value;
                    this.CallIdGuid = Guid.Parse(value);
                }
            }
        }

        public Guid CallIdGuid { get; private set; }




    }
}
