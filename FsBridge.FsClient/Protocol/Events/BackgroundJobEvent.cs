using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Events
{
    internal class BackgroundJobEvent : EventBase
    { 
        [JsonProperty("Job-UUID")]
        public Guid JobUUID { get; set; }

        [JsonProperty("Job-Command")]
        public string JobCommand { get; set; }

        [JsonProperty("Job-Command-Arg")]
        public string JobCommandArg { get; set; }

        [JsonProperty("Content-Length")]
        public string ContentLength { get; set; }

        [JsonProperty("_body")]
        public string Body { get; set; }
    }

}
