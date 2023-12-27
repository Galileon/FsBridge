using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Events
{
    public class ChannelHangupCompleteEvent : EventBase
    {
        [JsonProperty("Core-UUID")]
        public string CoreUUID { get; set; }

        [JsonProperty("FreeSWITCH-Hostname")]
        public string FreeSWITCHHostname { get; set; }

        [JsonProperty("FreeSWITCH-Switchname")]
        public string FreeSWITCHSwitchname { get; set; }

        [JsonProperty("FreeSWITCH-IPv4")]
        public string FreeSWITCHIPv4 { get; set; }

        [JsonProperty("FreeSWITCH-IPv6")]
        public string FreeSWITCHIPv6 { get; set; }

        [JsonProperty("Event-Date-Local")]
        public string EventDateLocal { get; set; }

        [JsonProperty("Event-Date-GMT")]
        public string EventDateGMT { get; set; }

        [JsonProperty("Event-Date-Timestamp")]
        public string EventDateTimestamp { get; set; }

        [JsonProperty("Event-Calling-File")]
        public string EventCallingFile { get; set; }

        [JsonProperty("Event-Calling-Function")]
        public string EventCallingFunction { get; set; }

        [JsonProperty("Event-Calling-Line-Number")]
        public string EventCallingLineNumber { get; set; }

        [JsonProperty("Event-Sequence")]
        public string EventSequence { get; set; }

        [JsonProperty("Hangup-Cause")]
        public string HangupCause { get; set; }

        [JsonProperty("Channel-State")]
        public string ChannelState { get; set; }

        [JsonProperty("Channel-Call-State")]
        public FsCallState ChannelCallState { get; set; }

        [JsonProperty("Channel-State-Number")]
        public string ChannelStateNumber { get; set; }

        [JsonProperty("Channel-Name")]
        public string ChannelName { get; set; }

        [JsonProperty("Unique-ID")]
        public string UniqueID { get; set; }

        [JsonProperty("Call-Direction")]
        public FsCallDirection CallDirection { get; set; }

        [JsonProperty("Presence-Call-Direction")]
        public string PresenceCallDirection { get; set; }

        [JsonProperty("Channel-HIT-Dialplan")]
        public string ChannelHITDialplan { get; set; }

        [JsonProperty("Channel-Presence-ID")]
        public string ChannelPresenceID { get; set; }

        [JsonProperty("Channel-Call-UUID")]
        public string ChannelCallUUID { get; set; }

        [JsonProperty("Answer-State")]
        public string AnswerState { get; set; }

        [JsonProperty("Caller-Direction")]
        public string CallerDirection { get; set; }

        [JsonProperty("Caller-Logical-Direction")]
        public string CallerLogicalDirection { get; set; }

        [JsonProperty("Caller-Username")]
        public string CallerUsername { get; set; }

        [JsonProperty("Caller-Dialplan")]
        public string CallerDialplan { get; set; }

        [JsonProperty("Caller-Caller-ID-Name")]
        public string CallerCallerIDName { get; set; }

        [JsonProperty("Caller-Caller-ID-Number")]
        public string CallerCallerIDNumber { get; set; }

        [JsonProperty("Caller-Orig-Caller-ID-Name")]
        public string CallerOrigCallerIDName { get; set; }

        [JsonProperty("Caller-Orig-Caller-ID-Number")]
        public string CallerOrigCallerIDNumber { get; set; }

        [JsonProperty("Caller-Network-Addr")]
        public string CallerNetworkAddr { get; set; }

        [JsonProperty("Caller-ANI")]
        public string CallerANI { get; set; }

        [JsonProperty("Caller-Destination-Number")]
        public string CallerDestinationNumber { get; set; }

        [JsonProperty("Caller-Unique-ID")]
        public string CallerUniqueID { get; set; }

        [JsonProperty("Caller-Source")]
        public string CallerSource { get; set; }

        [JsonProperty("Caller-Context")]
        public string CallerContext { get; set; }

        [JsonProperty("Caller-Channel-Name")]
        public string CallerChannelName { get; set; }

        [JsonProperty("Caller-Profile-Index")]
        public string CallerProfileIndex { get; set; }

        [JsonProperty("Caller-Profile-Created-Time")]
        public string CallerProfileCreatedTime { get; set; }

        [JsonProperty("Caller-Channel-Created-Time")]
        public string CallerChannelCreatedTime { get; set; }

        [JsonProperty("Caller-Channel-Answered-Time")]
        public string CallerChannelAnsweredTime { get; set; }

        [JsonProperty("Caller-Channel-Progress-Time")]
        public string CallerChannelProgressTime { get; set; }

        [JsonProperty("Caller-Channel-Progress-Media-Time")]
        public string CallerChannelProgressMediaTime { get; set; }

        [JsonProperty("Caller-Channel-Hangup-Time")]
        public string CallerChannelHangupTime { get; set; }

        [JsonProperty("Caller-Channel-Transfer-Time")]
        public string CallerChannelTransferTime { get; set; }

        [JsonProperty("Caller-Channel-Resurrect-Time")]
        public string CallerChannelResurrectTime { get; set; }

        [JsonProperty("Caller-Channel-Bridged-Time")]
        public string CallerChannelBridgedTime { get; set; }

        [JsonProperty("Caller-Channel-Last-Hold")]
        public string CallerChannelLastHold { get; set; }

        [JsonProperty("Caller-Channel-Hold-Accum")]
        public string CallerChannelHoldAccum { get; set; }

        [JsonProperty("Caller-Screen-Bit")]
        public string CallerScreenBit { get; set; }

        [JsonProperty("Caller-Privacy-Hide-Name")]
        public string CallerPrivacyHideName { get; set; }

        [JsonProperty("Caller-Privacy-Hide-Number")]
        public string CallerPrivacyHideNumber { get; set; }
        public string variable_direction { get; set; }
        public string variable_uuid { get; set; }
        public string variable_session_id { get; set; }
        public string variable_sip_from_user { get; set; }
        public string variable_sip_from_port { get; set; }
        public string variable_sip_from_uri { get; set; }
        public string variable_sip_from_host { get; set; }
        public string variable_video_media_flow { get; set; }
        public string variable_audio_media_flow { get; set; }
        public string variable_text_media_flow { get; set; }
        public string variable_channel_name { get; set; }
        public string variable_sip_call_id { get; set; }
        public string variable_sip_local_network_addr { get; set; }
        public string variable_sip_network_ip { get; set; }
        public string variable_sip_network_port { get; set; }
        public string variable_sip_invite_stamp { get; set; }
        public string variable_sip_received_ip { get; set; }
        public string variable_sip_received_port { get; set; }
        public string variable_sip_via_protocol { get; set; }
        public string variable_sip_from_user_stripped { get; set; }
        public string variable_sip_from_tag { get; set; }
        public string variable_sofia_profile_name { get; set; }
        public string variable_sofia_profile_url { get; set; }
        public string variable_recovery_profile_name { get; set; }

        [JsonProperty("variable_sip_P-Preferred-Identity")]
        public string variable_sip_PPreferredIdentity { get; set; }
        public string variable_sip_cid_type { get; set; }
        public string variable_sip_full_via { get; set; }
        public string variable_sip_from_display { get; set; }
        public string variable_sip_full_from { get; set; }
        public string variable_sip_full_to { get; set; }
        public string variable_sip_allow { get; set; }
        public string variable_sip_req_user { get; set; }
        public string variable_sip_req_port { get; set; }
        public string variable_sip_req_uri { get; set; }
        public string variable_sip_req_host { get; set; }
        public string variable_sip_to_user { get; set; }
        public string variable_sip_to_port { get; set; }
        public string variable_sip_to_uri { get; set; }
        public string variable_sip_to_host { get; set; }
        public string variable_sip_contact_params { get; set; }
        public string variable_sip_contact_user { get; set; }
        public string variable_sip_contact_port { get; set; }
        public string variable_sip_contact_uri { get; set; }
        public string variable_sip_contact_host { get; set; }
        public string variable_rtp_use_codec_string { get; set; }
        public string variable_sip_user_agent { get; set; }
        public string variable_sip_via_host { get; set; }
        public string variable_sip_via_port { get; set; }
        public string variable_sip_via_rport { get; set; }
        public string variable_max_forwards { get; set; }
        public string variable_presence_id { get; set; }
        public string variable_switch_r_sdp { get; set; }
        public string variable_ep_codec_string { get; set; }
        public string variable_endpoint_disposition { get; set; }
        public string variable_call_uuid { get; set; }
        public string variable_hangup_cause { get; set; }
        public string variable_hangup_cause_q850 { get; set; }
        public string variable_digits_dialed { get; set; }
        public string variable_start_stamp { get; set; }
        public string variable_profile_start_stamp { get; set; }
        public string variable_end_stamp { get; set; }
        public string variable_start_epoch { get; set; }
        public string variable_start_uepoch { get; set; }
        public string variable_profile_start_epoch { get; set; }
        public string variable_profile_start_uepoch { get; set; }
        public string variable_answer_epoch { get; set; }
        public string variable_answer_uepoch { get; set; }
        public string variable_bridge_epoch { get; set; }
        public string variable_bridge_uepoch { get; set; }
        public string variable_last_hold_epoch { get; set; }
        public string variable_last_hold_uepoch { get; set; }
        public string variable_hold_accum_seconds { get; set; }
        public string variable_hold_accum_usec { get; set; }
        public string variable_hold_accum_ms { get; set; }
        public string variable_resurrect_epoch { get; set; }
        public string variable_resurrect_uepoch { get; set; }
        public string variable_progress_epoch { get; set; }
        public string variable_progress_uepoch { get; set; }
        public string variable_progress_media_epoch { get; set; }
        public string variable_progress_media_uepoch { get; set; }
        public string variable_end_epoch { get; set; }
        public string variable_end_uepoch { get; set; }
        public string variable_caller_id { get; set; }
        public string variable_duration { get; set; }
        public string variable_billsec { get; set; }
        public string variable_progresssec { get; set; }
        public string variable_answersec { get; set; }
        public string variable_waitsec { get; set; }
        public string variable_progress_mediasec { get; set; }
        public string variable_flow_billsec { get; set; }
        public string variable_mduration { get; set; }
        public string variable_billmsec { get; set; }
        public string variable_progressmsec { get; set; }
        public string variable_answermsec { get; set; }
        public string variable_waitmsec { get; set; }
        public string variable_progress_mediamsec { get; set; }
        public string variable_flow_billmsec { get; set; }
        public string variable_uduration { get; set; }
        public string variable_billusec { get; set; }
        public string variable_progressusec { get; set; }
        public string variable_answerusec { get; set; }
        public string variable_waitusec { get; set; }
        public string variable_progress_mediausec { get; set; }
        public string variable_flow_billusec { get; set; }
        public string variable_sip_hangup_disposition { get; set; }
    }
}

