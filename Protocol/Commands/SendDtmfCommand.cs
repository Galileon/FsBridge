using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
     internal class SendDtmfCommand
    {
        public static string Encode(Guid CallId,string dtmfString)
        {
            /*return $"sendmsg {CallId}\n" +
                    "call-command: execute\n" +
                    "execute-app-name: send_dtmf\n" +
                    $"execute-app-arg: {dtmfString}";*/

            return $"bgapi uuid_send_dtmf {CallId} {dtmfString}";
        }
    }
}
