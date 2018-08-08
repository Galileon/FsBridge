using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class SetCommand
    {
        public static string Encode(Guid CallId, string property, string value)
        {

            return $"sendmsg {CallId}\n" +
                    $"call-command: execute\n" +
                    $"execute-app-name: set\n" +
                    $"execute-app-arg: {property}={value}";
        }
    }
}
