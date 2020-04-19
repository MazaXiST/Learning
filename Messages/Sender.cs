using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Bus;
using Rebus.Config;

namespace Messages
{
    public interface ISend
    {
        void Send(string message);
    }

    public class Sender : ISend
    {
        private BuiltinHandlerActivator adapter = new BuiltinHandlerActivator();

        public Sender()
        {
            Configure.With(adapter)
                .Logging(l => l.ColoredConsole(Rebus.Logging.LogLevel.Warn))
                .Transport(t => t.UseRabbitMqAsOneWayClient("amqp://user:12345@10.15.0.15"))
                .Start();
        }

        public void Send(string message)
        {
                    Publish(message, adapter.Bus);
        }

        void Publish(string mes, IBus bus)
        {
            Console.WriteLine("Publishing {0} messages", mes);

            bus.Publish(new Message { message = mes });
        }
    }
}
