using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public class AuthRequest : FreeswitchMessageBase
    {
        public override MessageType MsgType => MessageType.AuthRequest;

        public new AuthRequest Parse(string[] linesOfText)
        {
            return this;
        }
    }
}
