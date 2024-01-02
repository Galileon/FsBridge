using FsBridge.FsClient.Protocol.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Helpers
{
    internal class RequestResponsePool
    {
        System.Collections.Concurrent.ConcurrentDictionary<Guid, _RequestResponseEntry> _queries;
        public RequestResponsePool()
        {
            _queries = new System.Collections.Concurrent.ConcurrentDictionary<Guid, _RequestResponseEntry>();
        }
        internal void AppendRequest(Guid requestId, Type command, Action<CommandReply> callBack = null)
        {
            if (!_queries.TryAdd (requestId, new _RequestResponseEntry () {  CallBack = callBack, RequestedOn = DateTime.Now })) throw new Exception("Cannot Add request to dictionary!");
        }
        internal bool RemoveRequest(Guid uUID, out _RequestResponseEntry entry) => _queries.Remove(uUID, out entry);
    }

    internal class _RequestResponseEntry 
    {
        internal Action<CommandReply> CallBack { get; set; }
        internal DateTime RequestedOn { get; set; }
        internal Guid? CallId { get; set; } 

    }

}
