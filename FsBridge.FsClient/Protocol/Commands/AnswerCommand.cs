using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Commands
{
    public class AnswerCommand : CommandBase
    {
        public Guid CallId { get; set; }
        protected override FsCommandType CommandType => FsCommandType.BgApi;
        public AnswerCommand(Guid callId) => (CallId) = (callId);
        //protected override string Encode() =>  $"uuid_answer {CallId}";
        protected override string Encode() =>
           $"uuid_answer {CallId}";
    }
}
