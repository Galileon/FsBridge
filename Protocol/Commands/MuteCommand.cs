using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol.Commands
{
   internal class MuteCommand
    {
        public static string Encode(Guid CallId, bool mute)
        {
            return mute ? $"bgapi uuid_audio {CallId} start read mute -4" : $"bgapi uuid_audio {CallId} start read mute 0";
        }
    }
}
