using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsBridge.FsClient.Protocol.Commands
{
    public class PlaybackCommand : CommandBase
    {
        protected override FsCommandType CommandType => FsCommandType.SendMsg;
        public Guid CallId { get; private set; }
        public string FilePath { get; private set; }
        public AudioStreamDestination Destination { get; private set; }

        public PlaybackCommand(Guid callId,string file, AudioStreamDestination destination = AudioStreamDestination.Both)
        {
            (CallId, FilePath, Destination) = (callId, file, destination);
        }

        protected override string Encode()
        {
            return $"{CallId}\n" +
                    $"call-command: execute\n" +
                    $"execute-app-name: playback\n" +
                    $"execute-app-arg: {FilePath}";
        }
    }
}
