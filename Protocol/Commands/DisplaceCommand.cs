using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol.Commands
{
    internal class DisplaceCommand
    {
        public static string Encode(Guid CallId, string file)
        {
            //return $"bgapi uuid_displace {CallId} start {file} 0 [mux]";sched_broadcast
            return $"api sched_broadcast +0 {CallId} {file} both";
        }
    }
}
