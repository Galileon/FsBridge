using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.Protocol.Commands
{
    public class ResponseBase
    {
        public Guid? UUID { get; private set; }
    }
}
