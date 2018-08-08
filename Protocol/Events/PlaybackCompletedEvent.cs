using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public class PlaybackCompletedEvent : EventBase
    {

        [JsonProperty("Playback-Status")]
        public FsPlaybackStatus PlayResult { get; set; }

        [JsonProperty("variable_playback_terminator_used")]
        public string TerminatorUsed { get; set; }

        public override string ToString()
        {
            return $"PlayCompleted Result: {PlayResult} Terminator: {TerminatorUsed}";
        }


    }
}
