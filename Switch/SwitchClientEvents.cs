using FsConnect.CallModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect
{
    public static class SwitchClientEvents
    {
        public delegate void OnConnectionStateChangedDelegate(int switchId, SwitchConnectionState connectionState);

        public delegate void OnCallStateChangedDelegate(SwitchCall call);

        public delegate void OnDtmfPressedDelegate(SwitchCall call, char pressedDigit, string digitBuffer);

        public delegate void OnPlayOperationCompletedDelegate (SwitchCall call, FsPlaybackStatus result);

        public delegate void OnRecordOperationCompletedDelegate(SwitchCall call, FsRecordStatus result);


    }
}
