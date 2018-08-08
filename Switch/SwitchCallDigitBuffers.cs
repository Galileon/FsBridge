using FsConnect.CallModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect
{
    internal class SwitchCallDigitBuffers
    {
        Dictionary<SwitchCall, List<char>> digitBuffers = new Dictionary<SwitchCall, List<char>>();

        private object _sync = new object();

        public SwitchCallDigitBuffers(SwitchClient client)
        {
            client.OnCallStateChanged += (call) =>
            {
                if (call.CallState == FsCallState.Disconnected) Remove(call);
            };
        }

        internal void AppendDigit(SwitchCall call, char digit)
        {
            if (digit == ' ') return;

            lock (_sync)
            {
                if (!digitBuffers.ContainsKey(call)) this.digitBuffers[call] = new List<char>();
                digitBuffers[call].Add(digit);
            }
        }

        internal void Remove(SwitchCall call)
        {
            lock (_sync)
            {
                if (this.digitBuffers.ContainsKey(call)) this.digitBuffers.Remove(call);
            }
        }

        internal string GetDigits(SwitchCall call)
        {
            lock (_sync)
            {
                if (!digitBuffers.ContainsKey(call)) return string.Empty;
                return new string(digitBuffers[call].ToArray());
            }

        }

    }
}
