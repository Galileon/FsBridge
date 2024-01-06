using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Commands
{
    public enum CommandReplyResult
    {
        Ok,
        Failed
    }

    public class CommandReply : ResponseBase
    {
        public string Text { get; set; }
        public CommandReplyResult Result { get; set; }
        [JsonProperty("Job-UUID")]
        public Guid? JobUUID { get; set; }
        /// <summary>
        /// Added to pass parameter from EventSocket to multiinvoker
        /// </summary>
        public Guid? CallId { get; set; }

    }

    public class CommandReply<T> : CommandReply
    {
        public T? Response { get; set; }
    }
}
