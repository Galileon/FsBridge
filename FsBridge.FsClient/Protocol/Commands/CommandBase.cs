using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Commands
{
    public abstract class CommandBase
    {
        public Guid UUID { get; private set; } = Guid.NewGuid();
        protected abstract string Encode();
        protected abstract FsCommandType CommandType { get; }
        public string EncodeCommand()
        {
            switch (CommandType)
            {
                case FsCommandType.SendMsg:
                    return $"sendmsg {Encode()}\nEvent-UUID: {UUID}\n\n";
                case FsCommandType.Api:
                    return $"api {Encode()}\nEvent-UUID: {UUID}\n\n";
                case FsCommandType.BgApi:
                    return $"bgapi {Encode()}\nJob-UUID: {UUID}\n\n";
                default:
                    return $"{Encode()}\n\n";
            }
        }
    }
}
