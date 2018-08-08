using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public class RecordCompletedEvent : EventBase
    {

        [JsonProperty("Record-Completion-Cause")]
        public FsRecordStatus RecordResult { get; set; }

        [JsonProperty("Record-File-Path")]
        public string RecordFilePath { get; set; }
        
        public override string ToString()
        {
            return $"RecordCompleted: Result: {RecordResult}";
        }


    }
}
