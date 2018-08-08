using System;
using System.Collections.Generic;
using FsConnect.Extensions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace FsConnect.CallModel
{
    public class SwitchCallCollection 
    {

        private Dictionary<int, Dictionary<Guid, SwitchCall>> Calls = new Dictionary<int, Dictionary<Guid, SwitchCall>>();

        private Dictionary<Guid, int> CallIdSwitchId = new Dictionary<Guid, int>();

        private object _sync = new object();

        #region Methods

        public SwitchCall AddOrUpdateCall(int switchId, Protocol.CallStateEvent csEvent)
        {
            lock (_sync)
            {
                if (!Calls.ContainsKey(switchId)) Calls[switchId] = new Dictionary<Guid, SwitchCall>();
                if (!Calls[switchId].ContainsKey(Guid.Parse(csEvent.CallId))) Calls[switchId][csEvent.CallIdGuid] = new SwitchCall() { SwitchId = switchId };
                var callOb = Calls[switchId][csEvent.CallIdGuid];
                CallIdSwitchId[csEvent.CallIdGuid] = switchId;
                callOb.Update(csEvent);
                return callOb;
            }
        }

        public void AddCall(int switchId, SwitchCall call)
        {
            lock (_sync)
            {
                if (!Calls.ContainsKey(switchId)) Calls[switchId] = new Dictionary<Guid, SwitchCall>();
                Calls[switchId][call.CallId] = call;
            }
        }

        public bool HasCall(int switchId, Guid callId)
        {

            lock (_sync)
            {
                if (!Calls.ContainsKey(switchId)) return false;
                if (!Calls[switchId].ContainsKey(callId)) return false;
                return true;
            }
        }

        public SwitchCall GetCall(Guid callId)
        {
            lock (_sync)
            {
                if (CallIdSwitchId.TryGetValue(callId, out int switchId))
                {
                    if (!Calls.ContainsKey(switchId)) return null;
                    if (!Calls[switchId].ContainsKey(callId)) return null;
                    return Calls[switchId][callId];
                }

                return null;
            }
        }

        public SwitchCall GetCall(int switchId, Guid callId)
        {
            lock (_sync)
            {
                if (!Calls.ContainsKey(switchId)) return null;
                if (!Calls[switchId].ContainsKey(callId)) return null;
                return Calls[switchId][callId];
            }
        }

        #endregion

    }
}