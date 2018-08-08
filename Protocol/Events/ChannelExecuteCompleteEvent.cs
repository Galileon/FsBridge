using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public class ChannelExecuteCompleteEvent : EventBase
    {
        public override MessageType MsgType => MessageType.Event;

        public string Application { get; set; }

        [JsonProperty("Application-Response")]
        public string Response { get; set; }

        [JsonProperty("Application-UUID")]
        public string RequestId { get; set; }

        #region Play Section

        [JsonProperty("variable_playback_terminator_used")]
        public string PlaybackTerminator { get; set; }

        #endregion

        public override string ToString()
        {
            return $"ExecuteCompleted: Application: {Application} Response: {Response} RequestId: {RequestId}";
        }


    }
}
