using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect
{
    /// <summary>
    /// Settings for switch socket connection
    /// </summary>
    public class SwitchConnectionSettings
    {

        public string SocketAddress { get; set; }

        public int SocketPort { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        /// <summary>
        /// Unique switch connection identifier
        /// </summary>
        public int SwitchId { get; set; }

        /// <summary>
        /// List of monitored incoming context spearated by ' ' or ',' or ';'
        /// </summary>
        public string IncomingContexts { get; set; } = "mediastack";


    }
}
