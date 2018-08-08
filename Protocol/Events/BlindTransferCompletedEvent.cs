using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol.Events
{
    public class BlindTransferCompletedEvent : EventBase
    {

        public FsBlindTrasnferStatus TransferResult { get; set; }

        public string Destination { get; set; }

        public override string ToString()
        {
            return $"BlindTransferCompletedEvent Result: {TransferResult} Destination: {Destination}";
        }

    }
}
