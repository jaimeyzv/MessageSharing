using ChatRoom.Injector;
using ChatRoom.MessageBroker.Processor.Interfaces;

namespace ChatRoom.MessageBroker.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new BootStrapper();
            container.RegisterComponents();
            IProcessor processor = new Processor.Processor();
            processor.ProcessMessage();
        }
    }
}