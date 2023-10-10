using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect
{
    public class FreeswitchHttpServerSettings
    {

        public string Url { get; set; }

        /// <summary>
        /// Base path for reading files
        /// </summary>
        public string ReadBasePath { get; set; }
    }
}
