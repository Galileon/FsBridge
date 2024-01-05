using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Events
{
    public class ChannelExecuteEvent : EventBase
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

        [JsonProperty("variable_direction")]
        public string VariableDirection { get; set; }

        [JsonProperty("variable_uuid")]
        public string VariableUuid { get; set; }

        [JsonProperty("variable_session_id")]
        public string VariableSessionId { get; set; }

        [JsonProperty("variable_sip_from_user")]
        public string VariableSipFromUser { get; set; }

        [JsonProperty("variable_sip_from_port")]
        public string VariableSipFromPort { get; set; }

        [JsonProperty("variable_sip_from_uri")]
        public string VariableSipFromUri { get; set; }

        [JsonProperty("variable_sip_from_host")]
        public string VariableSipFromHost { get; set; }

        [JsonProperty("variable_video_media_flow")]
        public string VariableVideoMediaFlow { get; set; }

        [JsonProperty("variable_audio_media_flow")]
        public string VariableAudioMediaFlow { get; set; }

        [JsonProperty("variable_text_media_flow")]
        public string VariableTextMediaFlow { get; set; }

        [JsonProperty("variable_channel_name")]
        public string VariableChannelName { get; set; }

        [JsonProperty("variable_sip_call_id")]
        public string VariableSipCallId { get; set; }

        [JsonProperty("variable_sip_local_network_addr")]
        public string VariableSipLocalNetworkAddr { get; set; }

        [JsonProperty("variable_sip_network_ip")]
        public string VariableSipNetworkIp { get; set; }

        [JsonProperty("variable_sip_network_port")]
        public string VariableSipNetworkPort { get; set; }

        [JsonProperty("variable_sip_invite_stamp")]
        public string VariableSipInviteStamp { get; set; }

        [JsonProperty("variable_sip_received_ip")]
        public string VariableSipReceivedIp { get; set; }

        [JsonProperty("variable_sip_received_port")]
        public string VariableSipReceivedPort { get; set; }

        [JsonProperty("variable_sip_via_protocol")]
        public string VariableSipViaProtocol { get; set; }

        [JsonProperty("variable_sip_from_user_stripped")]
        public string VariableSipFromUserStripped { get; set; }

        [JsonProperty("variable_sip_from_tag")]
        public string VariableSipFromTag { get; set; }

        [JsonProperty("variable_sofia_profile_name")]
        public string VariableSofiaProfileName { get; set; }

        [JsonProperty("variable_sofia_profile_url")]
        public string VariableSofiaProfileUrl { get; set; }

        [JsonProperty("variable_recovery_profile_name")]
        public string VariableRecoveryProfileName { get; set; }

        [JsonProperty("variable_sip_P-Preferred-Identity")]
        public string VariableSipPPreferredIdentity { get; set; }

        [JsonProperty("variable_sip_cid_type")]
        public string VariableSipCidType { get; set; }

        [JsonProperty("variable_sip_full_via")]
        public string VariableSipFullVia { get; set; }

        [JsonProperty("variable_sip_from_display")]
        public string VariableSipFromDisplay { get; set; }

        [JsonProperty("variable_sip_full_from")]
        public string VariableSipFullFrom { get; set; }

        [JsonProperty("variable_sip_full_to")]
        public string VariableSipFullTo { get; set; }

        [JsonProperty("variable_sip_allow")]
        public string VariableSipAllow { get; set; }

        [JsonProperty("variable_sip_req_user")]
        public string VariableSipReqUser { get; set; }

        [JsonProperty("variable_sip_req_port")]
        public string VariableSipReqPort { get; set; }

        [JsonProperty("variable_sip_req_uri")]
        public string VariableSipReqUri { get; set; }

        [JsonProperty("variable_sip_req_host")]
        public string VariableSipReqHost { get; set; }

        [JsonProperty("variable_sip_to_user")]
        public string VariableSipToUser { get; set; }

        [JsonProperty("variable_sip_to_port")]
        public string VariableSipToPort { get; set; }

        [JsonProperty("variable_sip_to_uri")]
        public string VariableSipToUri { get; set; }

        [JsonProperty("variable_sip_to_host")]
        public string VariableSipToHost { get; set; }

        [JsonProperty("variable_sip_contact_params")]
        public string VariableSipContactParams { get; set; }

        [JsonProperty("variable_sip_contact_user")]
        public string VariableSipContactUser { get; set; }

        [JsonProperty("variable_sip_contact_port")]
        public string VariableSipContactPort { get; set; }

        [JsonProperty("variable_sip_contact_uri")]
        public string VariableSipContactUri { get; set; }

        [JsonProperty("variable_sip_contact_host")]
        public string VariableSipContactHost { get; set; }

        [JsonProperty("variable_rtp_use_codec_string")]
        public string VariableRtpUseCodecString { get; set; }

        [JsonProperty("variable_sip_user_agent")]
        public string VariableSipUserAgent { get; set; }

        [JsonProperty("variable_sip_via_host")]
        public string VariableSipViaHost { get; set; }

        [JsonProperty("variable_sip_via_port")]
        public string VariableSipViaPort { get; set; }

        [JsonProperty("variable_sip_via_rport")]
        public string VariableSipViaRport { get; set; }

        [JsonProperty("variable_max_forwards")]
        public string VariableMaxForwards { get; set; }

        [JsonProperty("variable_presence_id")]
        public string VariablePresenceId { get; set; }

        [JsonProperty("variable_switch_r_sdp")]
        public string VariableSwitchRSdp { get; set; }

        [JsonProperty("variable_ep_codec_string")]
        public string VariableEpCodecString { get; set; }

        [JsonProperty("variable_endpoint_disposition")]
        public string VariableEndpointDisposition { get; set; }

        [JsonProperty("variable_call_uuid")]
        public string VariableCallUuid { get; set; }

        [JsonProperty("variable_current_application")]
        public string VariableCurrentApplication { get; set; }

        [JsonProperty("variable_current_application_data")]
        public string VariableCurrentApplicationData { get; set; }

        [JsonProperty("Application")]
        public string Application { get; set; }

        [JsonProperty("Application-Data")]
        public string ApplicationData { get; set; }

        [JsonProperty("Application-UUID")]
        public string ApplicationUUID { get; set; }
    }
}
