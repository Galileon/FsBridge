using FsBridge.Protocol.Commands;
using NetCoreServer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;

namespace FsBridge.Helpers
{
    public enum MessageType
    {
        AuthRequest,
        CommandReply,
        Event
    }

    public enum EventType
    {
        [EnumMember(Value = "HEARTBEAT")]
        Heartbeat,
        [EnumMember(Value = "RE_SCHEDULE")]
        Reschedule,
        [EnumMember(Value = "CHANNEL_CALLSTATE")]
        ChannelCallState,
        [EnumMember(Value = "CHANNEL_STATE")]
        ChannelState,
        [EnumMember(Value = "DTMF")]
        DtmfEvent,
        [EnumMember(Value = "CHANNEL_CREATE")]
        ChannelCreate,
        [EnumMember(Value = "CHANNEL_HANGUP")]
        ChannelHangup,
        [EnumMember(Value = "CHANNEL_DESTROY")]
        ChannelDestroy,
        [EnumMember(Value = "CHANNEL_EXECUTE")]
        ChannelExecute,
        [EnumMember(Value = "CHANNEL_EXECUTE_COMPLETE")]
        ChannelExecuteCompleteEvent,
        [EnumMember(Value = "CHANNEL_HANGUP_COMPLETE")]
        ChannelHangupComplete,
        /// <summary>
        /// Used for command reply 
        /// </summary>
        [EnumMember(Value = "SOCKET_DATA")]
        SocketData,
        [EnumMember(Value = "PLAYBACK_STOP")]
        PlaybackStopEvent,
        [EnumMember(Value = "RECORD_STOP")]
        RecordStopEvent,
        [EnumMember(Value = "API")]
        ApiEvent,
        [EnumMember(Value = "BACKGROUND_JOB")]
        BackgroundEvent,
        [EnumMember(Value = "PRESENCE_IN")]
        PresenceIn,
    }

    
    public class MessageParser
    {
        NetCoreServer.Buffer _receiveBuffer = new NetCoreServer.Buffer();
        ArraySegment<byte> _lastSegment;
        int? _expectedSegmentSize;
        #region Static Methods
        public static MessageType? GetMessageType(string header, out int? contentLenght)
        {
            contentLenght = null;
            var contentTypeIndex = 0;

            var messageParts = header.Split('\n');
            if (messageParts.Length == 0) return null;

            if (messageParts.Length > 1 && messageParts[0].StartsWith("Content-Length"))
            {
                contentLenght = Int32.Parse(MessageParser.GetStringParameter (messageParts[0]).Trim());
                contentTypeIndex = 1; // For sized content it will be there 
            }
            if (messageParts[contentTypeIndex] == "Content-Type: auth/request") return MessageType.AuthRequest;
            if (messageParts[contentTypeIndex] == "Content-Type: command/reply") return MessageType.CommandReply;
            return MessageType.Event;
        }
        public static CommandReply GetCommandReply (string content)
        {
            var lines = content.Split("\n");
            if (lines.Count() < 2) throw new Exception("Expected at least 2 lines at command reply.");
            if (lines[1].Contains ("+OK")) return new CommandReply () {  Result = CommandReplyResult.Ok, Text = GetStringParameter(lines[1]) };
            return new CommandReply() { Result = CommandReplyResult.Failed, Text = GetStringParameter (lines[1]) };
        }
        public static string GetStringParameter (string content)
        {
            var idx = content.IndexOf(':');
            if (idx == -1) return content;
            return content.Substring(idx + 1, content.Length - idx - 1);

        }
        #endregion

        public void Append (ArraySegment<byte> buffer)
        {
            _lastSegment = buffer;
            _receiveBuffer.Append (_lastSegment);
        }
        internal bool ReadMessage(out string msg, out MessageType? msgType)
        {
            msgType = MessageType.Event;
            msg = string.Empty;

            if (_receiveBuffer.Size == 0) return false; 
            if (!BufferHelper.HasCompletedSegment(_lastSegment)) return false;
            if (!BufferHelper.FetchMessageSegment(_receiveBuffer, _expectedSegmentSize, out msg)) return false;
            if (_expectedSegmentSize.HasValue) // we have had declared segment and we fetched succsfully
            { 
                _expectedSegmentSize = null;
                return true;
            }
            msgType = GetMessageType(msg, out _expectedSegmentSize);
            if (_expectedSegmentSize.HasValue && BufferHelper.FetchMessageSegment(_receiveBuffer, _expectedSegmentSize, out msg))
            {
                _expectedSegmentSize = null;
                return true;
            }
            if (_expectedSegmentSize != null) return false; // We do not return header
            return true;
        }
    }
}
