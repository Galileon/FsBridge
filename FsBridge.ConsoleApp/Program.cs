using FsBridge.FsClient;
using FsBridge.FsClient.Protocol;
using FsBridge.FsClient.Protocol.Commands;
using FsBridge.FsClient.Protocol.Events;

namespace FsBridge.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fs = new FsBridge.FsClient.EventSocketClient("127.0.0.1", 8021, "ClueCon");
            fs.OnEvent += Fs_OnEvent;
            fs.OnStateChanged += Fs_OnStateChanged;
            fs.ConnectAsync();
            Console.ReadKey();
        }
        private static void Fs_OnStateChanged(EventSocketClient client, EventSocketClientState state, EventSocketClientState previousState)
        {
            Console.WriteLine($"StateChanged: {state}");
        }
        private static void Fs_OnEvent(EventSocketClient client, EventBase evnt)
        {
            if (evnt is ChannelStateEvent cSe)
            {
                //    Console.WriteLine($"{cSe.CallDirection} {cSe.CallerANI} {cSe.CallerDestinationNumber} {cSe.ChannelCallUUID} {cSe.ChannelCallState} {cSe.ChannelState}");
            }

            if (evnt is ChannelExecuteEvent cEe)
            {
                Console.WriteLine($"{cEe.Application} {cEe.ApplicationData}");
            }

            if (evnt is ChannelCallStateEvent cCse)
            {
                Console.WriteLine($"{cCse.CallDirection} {cCse.CallerANI} {cCse.CallerDestinationNumber} {cCse.ChannelCallUUID} {cCse.ChannelCallState} {cCse.ChannelState} {cCse.ChannelCallState} {cCse.HangupCause}");
                if (cCse.ChannelCallState == FsCallState.Ringing && cCse.CallDirection == FsCallDirection.Inbound && cCse.CallerDestinationNumber == "77777")
                {
                    client.SendCommand(new AnswerCommand(cCse.ChannelCallUUID));
                }

                if (cCse.ChannelCallState == FsCallState.Active)
                {
                    client.SendCommand(new PlaybackCommand (cCse.ChannelCallUUID, "d:\\1.wav"));
                }

            }
        }
    }
}
