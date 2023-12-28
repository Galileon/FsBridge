using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient
{
    public class FreeswitchConfiguration
    {
        public string Password { get; set; }
        public string Address { get; set; }
        public int Port { get; set; }
        /// <summary>
        /// Profile context - leave empty if we want to see everything
        /// </summary>
        public string Context { get; set; }
        /// <summary>
        /// For debug/diagnose purposes we can dump all packets to file
        /// </summary>
        public string ConnectionDumpFilePath { get; set; }
    }
}
