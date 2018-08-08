using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class AnswerCommand
    {
        public static string Encode(string CallId)
        {
            return $"bgapi uuid_answer {CallId}";
        }
    }
}
