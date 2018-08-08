using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    internal class MakeCallCommand
    {

        public static string Encode(FreeswitchClient client, Guid callId, string destNumber, string ani, string dnis, bool autoAnswer, int timeOutSeconds = 10, Guid? bridgeWithCallId = null)
        {
            string aaString = string.Empty;
            string destString = " &park()";
            if (autoAnswer) aaString = "sip_h_x-answer-after=0,sip_auto_answer=true,";
            if (bridgeWithCallId.HasValue) destString = $" &bridge('{bridgeWithCallId}')";
            destNumber = client.GetOriginatePhoneNumber(destNumber);
            //return string.Format($"bgapi originate [hangup_after_bridge=false,park_after_bridge=true,sip_h_x-internal-callid={callId},leg_progress_timeout=8,{aaString}origination_uuid={callId},origination_caller_id_number={ani},ignore_early_media=true,originate_retries=0,park_after_bridge=true,originate_timeout={timeOutSeconds}]{destNumber} &park()");
            return string.Format($"bgapi originate [hangup_after_bridge=false,park_after_bridge=true,sip_h_x-internal-callid={callId},leg_progress_timeout=8,{aaString}origination_uuid={callId},origination_caller_id_number={ani},ignore_early_media=false,originate_retries=0,park_after_bridge=true,originate_timeout={timeOutSeconds}]{destNumber} {destString}");

        }
    }
}
