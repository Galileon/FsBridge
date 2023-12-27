using FsBridge.FsClient;
using FsBridge.FsClient.Protocol;
using FsBridge.FsClient.Protocol.Events;

namespace FsBridge.ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var fs = new FsBridge.FsClient.EventSocketClient("127.0.0.1", 8021);
            fs.OnEvent += Fs_OnEvent;
            fs.OnStateChanged += Fs_OnStateChanged;
            fs.ConnectAsync();
            Console.ReadKey();
        }

        private static void Fs_OnStateChanged(EventSocketClientState state, EventSocketClientState previousState)
        {
            Console.WriteLine($"StateChanged: {state}");
        }

        private static void Fs_OnEvent(EventBase evnt)
        {
            Console.WriteLine (evnt);
        }
    }
}
