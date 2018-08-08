using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public class BackgroundJobEvent : EventBase
    {
        [JsonProperty("Job-UUID")]
        public Guid RequestId { get; set; }

        [JsonProperty("origination_uuid")]
        public Guid? OriginationUID { get; set; }

        [JsonProperty("Job-Command")]
        public string Command { get; set; }

        [JsonProperty("Job-Command-Arg")]
        public string CommandArgs { get; set; }

        [JsonProperty("_body")]
        public string Response { get; set; }

        public FsEventCause? Cause
        {
            get
            {
                if (string.IsNullOrEmpty(Response)) return null;
                if (Response.Contains("SUBSCRIBER_ABSENT")) return FsEventCause.SUBSCRIBER_ABSENT;
                if (Response.Contains("USER_NOT_REGISTERED")) return FsEventCause.USER_NOT_REGISTERED;
                return null;
            }
        }


        public bool Error
        {
            get
            {
                if (Response == null) Response = string.Empty;
                return Response.Contains("-ERR");
            }
        }

        public override string ToString()
        {
            return $"BakgroundJobResult: Command: {Command} Error: {Error} Result: {Response}";
        }
    }
}
