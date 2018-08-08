using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class PlaybackCommand
    {

        public static string Encode(Guid CallId, string file, AudioDestination destination = AudioDestination.Both)
        {
            return $"sendmsg {CallId}\n" +
                    $"call-command: execute\n" +
                    $"execute-app-name: playback\n" +
                    $"execute-app-arg: {file}";
        }
    }
}
