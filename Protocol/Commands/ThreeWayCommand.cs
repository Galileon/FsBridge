using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class ThreeWayCommand
    {
        public static string Encode(string CallId, string destCallId)
        {
            return $"sendmsg {CallId}\n" +
                    "call-command: execute\n" +
                    "execute-app-name: three_way\n" + // three_way
                     $"execute-app-arg: {destCallId}";
        }

    }
}
