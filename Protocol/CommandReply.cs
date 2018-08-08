using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsConnect.Protocol
{
    public class CommandReply : FreeswitchMessageBase
    {
        public override MessageType MsgType => MessageType.CommandReply;

        public CommandResult Result { get; internal set; }

        /// <summary>
        /// Identifier of event
        /// </summary>
        public Guid RequestId { get; set; }

        public FsEventCause? Cause { get; set; }

        public new CommandReply Parse(string[] linesOfText)
        {
            Result = CommandResult.OK;
                        
            if (linesOfText.Length > 2 && linesOfText[2].Contains("Request-Id"))
            {
                var indexOfRequestId = linesOfText[2].IndexOf("Request-Id");
                if (linesOfText[2].Length >= indexOfRequestId + 10 + 36)
                {
                    RequestId = Guid.Parse(linesOfText[2].Substring(indexOfRequestId + 12, 36).Trim());
                }
            }

            if (linesOfText.Length > 2 && linesOfText[2].Contains("Job-UUID"))
            {
                var indexOfRequestId = linesOfText[2].IndexOf("Job-UUID");
                if (linesOfText[2].Length >= indexOfRequestId + 8 + 36)
                {
                    RequestId = Guid.Parse(linesOfText[2].Substring(indexOfRequestId + 10, 36).Trim());
                }
            }

            if (linesOfText.Length > 1)
            {
                if (linesOfText[1].Contains("-ERR")) Result = CommandResult.Failed;
                if (linesOfText[1].Contains("invalid session id")) { Result = CommandResult.InvalidCallId; return this; }
                if (Result == CommandResult.Failed)
                {
                    // Try to get cause!
                    Enum.GetNames(typeof(FsEventCause)).ToList().ForEach(_ =>
                    {
                        if (!string.IsNullOrWhiteSpace(_) && linesOfText[1].Contains(_)) { this.Cause = (FsEventCause)Enum.Parse(typeof(FsEventCause), _); };
                    });
                }
            }

            return this;
        }

        public enum CommandResult
        {
            OK,
            Failed,
            InvalidCallId
        }

    }
}

