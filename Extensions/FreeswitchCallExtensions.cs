using FsConnect.CallModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Extensions
{
    public static class FreeswitchCallExtensions
    {

        public static void Update(this SwitchCall call, Protocol.CallStateEvent callEvent)
        {
            call.Ani = callEvent.Ani;
            call.Dnis = callEvent.Dnis;
            call.CallId = callEvent.CallIdGuid;
            call.CallState = callEvent.CallState;
            call.Context = callEvent.Context;
            call.Direction = callEvent.Direction;
            call.HangupCause = callEvent.HangupCause;
        }
    }
}
