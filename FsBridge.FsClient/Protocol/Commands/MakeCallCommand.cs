using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Commands
{
    public class MakeCallCommand : CommandBase
    {
        protected override FsCommandType CommandType => FsCommandType.BgApi;
        public Guid CallId { get; set; } = Guid.NewGuid();
        public string CalledNumber { get; set; }
        public bool AutoAnswer { get; set; }
        public int TimeOutSeconds { get; set; } = 10;
        public bool Park { get; set; } = true;
        public Guid? BridgeWithCallId { get; set; }
        public IDictionary<string, string> CustomHeaderValues { get; set; }
        public string CallingNumber { get; set; }
        public string CallingName { get; set; }
        public string Context { get; set; }
        protected override string Encode()
        {
            string aaString = string.Empty;
            string destString = Park ? " &park()" : string.Empty;
            StringBuilder customHeaderValuesString = null;
            if (CustomHeaderValues != null)
            {
                customHeaderValuesString = new StringBuilder();
                foreach (var ki in CustomHeaderValues) customHeaderValuesString.AppendFormat(",{0}={1}", ki.Key, ki.Value?.Replace(' ', '_'));
            }
            // - not working for some reason sometimes if (autoAnswer) aaString = "sip_h_x-answer-after=0,sip_auto_answer=true,"; //if (autoAnswer) aaString = "sip_h_x-answer-after=0,sip_auto_answer=true,";
            if (AutoAnswer) aaString = "sip_h_x-answer-after=0,";
            if (BridgeWithCallId.HasValue) destString = $" &bridge('{BridgeWithCallId}')";
            //destNumber = client.GetOriginatePhoneNumber(destNumber);
            CallingNumber = string.IsNullOrEmpty(CallingNumber) ? "" : Uri.EscapeDataString(CallingNumber);
            var ori = string.Format($"originate [hangup_after_bridge=false,park_after_bridge=true,sip_h_x-calling-context={Context}sip_h_x-internal-callid={CallId}{customHeaderValuesString?.ToString()},leg_progress_timeout=8,{aaString}origination_uuid={CallId},origination_caller_id_number={CallingNumber},ignore_early_media=false,originate_retries=0,park_after_bridge=true,originate_timeout={TimeOutSeconds}]{CalledNumber} {destString} XML {Context} 99874 Marian");
            return ori;

        }
    }
}
