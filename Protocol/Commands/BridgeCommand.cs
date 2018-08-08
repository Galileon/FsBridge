using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class BridgeCommand
    {
        public static string Encode(Guid CallId, Guid destCallId)
        {
            /*
            return $"sendmsg {CallId}\n" +
                    "call-command: execute\n" +
                    "execute-app-name: three_way\n" + // three_way
                     $"execute-app-arg: {destCallId}";
             */

            

            return $"bgapi uuid_bridge {CallId} {destCallId}";


        }

        public static string Encode(Guid CallId, Guid destCallId, Guid thirdCallId)
        {
            /*
            return $"sendmsg {CallId}\n" +
                    "call-command: execute\n" +
                    "execute-app-name: three_way\n" + // three_way
                     $"execute-app-arg: {destCallId}";
             */
            return $"bgapi uuid_bridge {CallId} {destCallId} {thirdCallId}";



        }
    }
}
