using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{

    public class ApiEvent : EventBase
    {
        public override MessageType MsgType => MessageType.Event;

        public override string ToString()
        {
            return $"ApiEvent: CallId: {this.CallId}";
        }

    }
}
