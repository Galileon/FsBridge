using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol.Commands
{
    internal class StopDisplaceCommand
    {
        public static string Encode(Guid CallId)
        {
            return $"bgapi uuid_displace {CallId} stop";
        }
    }
}
