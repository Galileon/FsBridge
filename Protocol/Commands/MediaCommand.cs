using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol.Commands
{
    internal class MediaCommand
    {

        public static string Encode(Guid CallId, bool enable)
        {
            var offString = enable ? "" : "[off]";
            return $"bgapi uuid_break {CallId} [all]";
        }

        //uuid_media [off]
    }
}
