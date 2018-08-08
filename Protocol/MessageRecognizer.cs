using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{

    public enum MessageType
    {
        AuthRequest,
        CommandReply,
        Event
    }

    public enum EventType
    {
        [JsonProperty("CHANNEL_CALLSTATE")]
        [EnumMember(Value = "CHANNEL_CALLSTATE")]
        ChannelCallState,
        [JsonProperty("DTMF")]
        [EnumMember(Value = "DTMF")]
        DtmfEvent,
        [JsonProperty("CHANNEL_EXECUTE")]
        [EnumMember(Value = "CHANNEL_EXECUTE")]
        ChannelExecute,
        [JsonProperty("CHANNEL_EXECUTE_COMPLETE")]
        [EnumMember(Value = "CHANNEL_EXECUTE_COMPLETE")]
        ChannelExecuteCompleteEvent,
        /// <summary>
        /// Used for command reply 
        /// </summary>
        [JsonProperty("SOCKET_DATA")]
        [EnumMember(Value = "SOCKET_DATA")]
        SocketData,
        [JsonProperty("PLAYBACK_STOP")]
        [EnumMember(Value = "PLAYBACK_STOP")]
        PlaybackStopEvent,
        [EnumMember(Value = "RECORD_STOP")]
        RecordStopEvent,
        [EnumMember(Value = "API")]
        ApiEvent,
        [EnumMember(Value = "BACKGROUND_JOB")]
        BackgroundEvent,

    }


    public static class MessageRecognizer
    {
        public static FreeswitchMessageBase GetMessageType(string header, out bool dynamicSize, out int contentLenght)
        {
            dynamicSize = false;
            contentLenght = 0;

            var messageParts = header.Split('\n');
            if (messageParts.Length == 0) return null;

            dynamicSize = false;

            if (messageParts.Length > 1 && messageParts[0].Contains("Content-Length")) 
            {
                dynamicSize = true;
                contentLenght = Int32.Parse(messageParts[0].Split(':')[1].Trim());
            }

            if (messageParts[0] == "Content-Type: auth/request") return new Protocol.AuthRequest().Parse(messageParts);
            if (messageParts[0] == "Content-Type: command/reply") return new Protocol.CommandReply().Parse(messageParts);

            return new Protocol.ApiEvent();
        }
    }
}
