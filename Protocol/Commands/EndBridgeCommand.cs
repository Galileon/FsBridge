using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol.Commands
{
    internal class EndBridgeCommand
    {
        public static string Encode(Guid CallId)
        {
            //return $"bgapi uuid_displace {CallId} start {file} 0 [mux]";sched_broadcast
            return $"bgapi uuid_transfer {CallId} -both park inline";
        }
    }
}
