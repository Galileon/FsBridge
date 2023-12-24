using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.Protocol.Commands
{
    internal class EventCommand : CommandBase
    {
        protected override string Encode()
        {
            return "event json ALL";
        }

    }
}
