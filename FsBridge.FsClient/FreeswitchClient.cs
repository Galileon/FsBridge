using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient
{
    public class FreeswitchClient
    {
        public FreeswitchConfiguration Configuration { get; private set; }
        internal ILogger _log;
        public FreeswitchClient(FreeswitchConfiguration config,ILogger logger)
        {
            Configuration = config;
            _log = logger;
        }

    }

}
