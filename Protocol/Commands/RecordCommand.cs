using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class RecordCommand
    {

        public static string Encode(Guid CallId, string file, bool stereo = false)
        {

            return $"sendmsg {CallId}\n" +
                    $"call-command: execute\n" +
                    $"execute-app-name: record_session\n" +
                    $"execute-app-arg: {file}";

        }
    }
}
