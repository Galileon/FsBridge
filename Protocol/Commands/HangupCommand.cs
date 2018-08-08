using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class HangupCommand
    {
        public static string Encode(Guid CallId, SipResponseCode hangupCause)
        {
            /*return $"sendmsg {CallId}\n" +
                   "call-command: execute\n" +
                   "execute-app-name: kill\n" +
                    $"execute-app-arg: {(int)hangupCause}";*/

            return $"bgapi uuid_kill {CallId} {(int)Freeswitch.SipResponseCodeToFsCause (hangupCause)}";
        }
    }
}
