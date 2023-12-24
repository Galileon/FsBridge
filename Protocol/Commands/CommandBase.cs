using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.Protocol.Commands
{
    internal abstract class CommandBase
    {
        public Guid UUID { get; private set; } = Guid.NewGuid();
        protected abstract string Encode();
        public string EncodeCommand()
        {
            return $"{Encode()}\nJob-UUID: {UUID}\n\n";
        }
    }
}
