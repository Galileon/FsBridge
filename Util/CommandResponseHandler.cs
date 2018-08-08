using FsConnect.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsConnect.Util
{
    /// <summary>
    /// Keeps a responses for commands related to callid
    /// </summary>
    internal class CommandResponseHandler
    {

        Dictionary<Guid, CommandReply> _queue = new Dictionary<Guid, CommandReply>();
        object _sync = new object();
        AutoResetEvent _hasEvents = new AutoResetEvent(false);
        ManualResetEvent _stopping = new ManualResetEvent(false);
        const int WaitTimeOut = 1000;

        internal void AppendResponse(CommandReply reply)
        {
            if (reply.RequestId == Guid.Empty) return;
            lock (_sync) this._queue[reply.RequestId] = reply;
            _hasEvents.Set();
        }

        internal CommandReply WaitForReply(Guid commandId)
        {
            var started = DateTime.Now.TimeOfDay.TotalSeconds;

            do
            {
                var reply = FetchReply(commandId);
                if (reply != null) return reply;
            } while (!_stopping.WaitOne(10) && DateTime.Now.TimeOfDay.TotalSeconds < started + 5);

            return null;
        }

        private CommandReply FetchReply(Guid requestId)
        {
            lock (_sync)
            {
                if (!_queue.ContainsKey(requestId)) return null;
                if (_queue[requestId] == null) return null;
                var ret = _queue[requestId];
                if (ret != null) _queue.Remove(requestId);
                return ret;
            }
        }

        internal void Clear()
        {
        }
    }

}
