
namespace FsBridge
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new EventSocketClient("127.0.0.1", 8021);
            client.OnEvent += Client_OnEvent;
            Console.ReadKey();

        }

        private static void Client_OnEvent(Protocol.Events.EventBase evnt)
        {
            Console.WriteLine(evnt);
        }
    }
}
