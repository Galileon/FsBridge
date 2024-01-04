using FsBridge.FsClient;
using FsBridge.FsClient.Helpers;
using FsBridge.FsClient.Protocol;
using FsBridge.FsClient.Protocol.Commands;
using FsBridge.FsClient.Protocol.Events;
using System.Runtime.CompilerServices;

namespace FsBridge.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fs = new FreeswitchClient(new FreeswitchConfiguration(), null);
            fs.OnChannelCallState += Fs_OnChannelCallState;
            fs.OnStateChanged += Fs_OnStateChanged1;
            fs.Connect();
            Console.ReadKey();
        }

        private static void Fs_OnStateChanged1(FreeswitchClient client, EventSocketClientState state, EventSocketClientState previousState)
        {
            Console.WriteLine($"Fs_OnStateChanged {state} Prev: {previousState}");
        }

        private static void Fs_OnChannelCallState(FreeswitchClient client, ChannelCallStateEvent callState)
        {
            Console.WriteLine($"{callState} {callState.CallDirection} {callState.CallerANI} {callState.CallerDestinationNumber} {callState.ChannelCallState} {callState.ChannelState}");
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
                    //client.SendCommand(new AnswerCommand(Guid.NewGuid  ()));
                }

                if (cCse.ChannelCallState == FsCallState.Active)
                {
                    client.SendCommand(new PlaybackCommand(cCse.ChannelCallUUID, "d:\\1.wav"));
                }
            }
        }
    }
}