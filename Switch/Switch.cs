using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect
{

    public enum SwitchConnectionState
    {
        Disconnected,
        Connecting,
        Authenticating,
        Connected
    }

    /// <summary>
    /// Related to AxCallState
    /// </summary>
    public enum SwitchCallState
    {
        /// <summary>
        /// Created or pending before ringing
        /// </summary>
        Initiating = 0,

        /// <summary>
        /// Ring received
        /// </summary>
        Alerting = 1,

        /// <summary>
        /// Call is in state connecting ( negotiating media, gatering additional information )
        /// </summary>
        Connecting = 2,

        /// <summary>
        /// Connected / Pickuped
        /// </summary>
        Connected = 3,

        /// <summary>
        /// Pending disconnection
        /// </summary>
        Disconnecting = 4,

        /// <summary>
        /// Call is on hold
        /// </summary>
        Holded = 6,

        /// <summary>
        /// Call is parked
        /// </summary>
        Parked = 7,

        /// <summary>
        /// Call is waiting in Queue
        /// </summary>
        Queued = 8,

        /// <summary>
        /// After waiting for network response
        /// </summary>
        NetworkReached = 9,

        /// <summary>
        /// Call was unheld and going to be active 
        /// </summary>
        Unheld = 10,

        Unknown = ushort.MaxValue - 1,

        /// <summary>
        /// Disconnected
        /// </summary>
        Disconnected = ushort.MaxValue
    }
}
