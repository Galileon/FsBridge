using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class StopRecordCommand
    {
        public static string Encode(Guid CallId)
        {

            return $"sendmsg {CallId}\n" +
                    $"call-command: execute\n" +
                    $"execute-app-name: stop_record_session\n" +
                    $"execute-app-arg: all";
        }
    }
}
