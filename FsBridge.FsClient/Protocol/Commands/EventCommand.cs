using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Commands
{
    internal class EventCommand : CommandBase
    {
        protected override string Encode() => "event json ALL";
        protected override FsCommandType CommandType => FsCommandType.Raw;
    }
}
