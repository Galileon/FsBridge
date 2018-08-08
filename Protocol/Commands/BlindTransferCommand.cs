using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol.Commands
{
    internal class BlindTransferCommand
    {

        public static string Encode(FreeswitchClient client, Guid CallId, string num)
        {
            num = client.GetOriginatePhoneNumber(num);
            var transferContext = string.IsNullOrEmpty(client.TransferContext) ? "internal" : client.TransferContext;
            /*
            return $"sendmsg {CallId}\n" +
                $"call-command: execute\n" +
                $"execute-app-name: transfer\n" +
                $"execute-app-arg: {num} XML {transferContext}";*/

            return $"bgapi uuid_transfer {CallId} {num} XML {transferContext}";

        }

    }
}
