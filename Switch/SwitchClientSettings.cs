using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect
{
    public class SwitchClientSettings
    {

        #region Trace Log

        public bool TraceEnabled { get; set; }

        public string TraceFilePath { get; set; }

        #endregion

        #region Http File Server
        
        /// <summary>
        /// Root for audio prompts to be served via built in http server 
        /// </summary>
        public string AudioPromptsRoot { get; set; }

        /// <summary>
        /// Root for audio prompts to be served via built in http server 
        /// </summary>
        public string AudioRecordingsRoot { get; set; }
                
        /// <summary>
        /// Url for audio http server
        /// </summary>
        public string AudioHttpServerUrl { get; set; }

        #endregion

        List<SwitchConnectionSettings> _connections { get; set; }

        public List<SwitchConnectionSettings> Connections
        {
            get
            {
                return _connections;
            }
            set
            {
                if (_connections != value)
                {
                    _connections = value;
                    UpdateContext();
                }
            }
        }

        private Dictionary<int, List<string>> Context { get; set; }

        private object _contextSync = new object();

        public SwitchClientSettings()
        {
            this.Connections = new List<SwitchConnectionSettings>();
        }

        public SwitchClientSettings(SwitchConnectionSettings settings)
        {
            this.Connections = new List<SwitchConnectionSettings>() { settings };
            UpdateContext();
        }

        private void UpdateContext()
        {
            lock (_contextSync)
            {
                this.Context = new Dictionary<int, List<string>>();
                this.Connections.ForEach(c =>
                {
                    foreach (var pn in c.IncomingContexts.Split(' ', ',', ';'))
                    {
                        if (!Context.ContainsKey(c.SwitchId)) Context[c.SwitchId] = new List<string>();
                        Context[c.SwitchId].Add(pn.Trim());
                    }
                });
            }
        }

        public bool IsContextValid(int switchId, string context)
        {
            lock (_contextSync)
            {
                if (Context.ContainsKey(switchId) && this.Context[switchId].Contains(context)) return true;
                return false;
            }
        }
    }
}
