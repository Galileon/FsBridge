using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Commands
{
    internal class AuthenticateCommand : CommandBase
    {
        public string Password { get; set; }
        protected override string Encode() => $"auth {Password}";
        protected override FsCommandType CommandType => FsCommandType.Raw;
    }
}
