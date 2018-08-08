﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class RetrieveCommand
    {
        public static string Encode(Guid CallId)
        {
            return $"bgapi uuid_hold off {CallId}";
        }
    }
}
