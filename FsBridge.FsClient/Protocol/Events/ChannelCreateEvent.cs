using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Events
{
    public class ChannelCreateEvent : EventBase
    {
        [JsonProperty("Channel-State")]
        public FsChannelState ChannelState { get; set; }

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
        public Guid ChannelCallUUID { get; set; }

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
        public string variable_call_uuid { get; set; }
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
}
}
