using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Commands
{
    public class HangupCommand : CommandBase
    {
        public Guid CallId { get; set; }
        public FsEventCause? Cause { get; set; } 
        protected override FsCommandType CommandType => FsCommandType.BgApi;
        public HangupCommand(Guid callId) => (CallId) = (callId);
        public HangupCommand(Guid callId, FsEventCause cause) => (CallId, Cause) = (callId, cause);
        //protected override string Encode() =>  $"uuid_answer {CallId}";
        protected override string Encode() =>
           Cause.HasValue ? $"uuid_kill {CallId} {(int)Cause}" :  $"uuid_kill {CallId}";
    }
}
