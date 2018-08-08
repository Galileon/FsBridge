using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class StopPlaybackCommand
    {
        public static string Encode(Guid CallId)
        {
            /*
            return $"sendmsg {CallId}\n" +
                    $"call-command: execute\n" +
                    $"execute-app-name: fileman\n" +
                    $"execute-app-arg: stop";
            */
            return $"bgapi uuid_break {CallId} all";
            //return $"bgapi uuid_fileman {CallId} stop";
        }
    }
}
