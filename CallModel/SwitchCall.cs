using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.CallModel
{
    public class SwitchCall : ICloneable
    {

        public int SwitchId { get; set; }

        public Guid CallId { get; set; }

        public string Ani { get; set; }

        public string Dnis { get; set; }

        public FsConnect.FsCallDirection Direction { get; set; }

        public FsConnect.FsCallState CallState { get; set; }

        public FsEventCause? HangupCause { get; set; }

        public string Context { get; set; }

        public object Clone()
        {
            return new SwitchCall()
            {
                SwitchId = this.SwitchId,
                Ani = this.Ani,
                Dnis = this.Dnis,
                CallId = this.CallId,
                CallState = this.CallState,
                Context = this.Context,
                Direction = this.Direction,
                HangupCause = this.HangupCause
            };
        }


        public void Update(SwitchCall newCallState)
        {
            Ani = newCallState.Ani;
            Dnis = newCallState.Dnis;
            CallId = newCallState.CallId;
            CallState = newCallState.CallState;
            Context = newCallState.Context;
            Direction = newCallState.Direction;
            HangupCause = newCallState.HangupCause;
        }

        public override string ToString()
        {
            return $"Id: {this.CallId} State: {this.CallState} Ani: {this.Ani} Dnis: {this.Dnis} Direction: {this.Direction} {HangupCause} Context: {Context}";
        }
    }
}
