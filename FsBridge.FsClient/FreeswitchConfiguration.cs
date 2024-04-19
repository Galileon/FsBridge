using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient
{
    public class FreeswitchConfiguration
    {
        public string Password { get; set; } = "ClueCon";
        public string Address { get; set; } = "10.10.10.200";
        public int Port { get; set; } = 8021;
        /// <summary>
        /// Profile context - leave empty if we want to see everything
        /// </summary>
        public string Context { get; set; }
        /// <summary>
        /// For debug/diagnose purposes we can dump all packets to file
        /// </summary>
        public string ConnectionDumpFilePath { get; set; }
        /// <summary>
        /// Lib will not use originate/bridge destination phone formatting
        /// </summary>
        public bool IgnorePhoneNumberFormatting { get; set; }
        /// <summary>
        /// Context used to makecalls from
        /// </summary>
        public string CallingProfile { get; set; } = "mediaproxy";
    }
}
