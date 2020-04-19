using System;
using Messages;
using Rebus.Bus;

using Microsoft.Extensions.DependencyInjection;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<ISend, Sender>()
                .BuildServiceProvider();

            var sender = serviceProvider.GetService<ISend>();

            var keepRunning = true;

            while (keepRunning)
            {
                Console.Write("Enter the message:");
                string mes = Console.ReadLine();
                sender.Send(mes);

                System.Threading.Thread.Sleep(500);

                Console.Clear();
            }
        }

        static void Publish(string mes, IBus bus)
        {
            Console.WriteLine("Publishing {0} messages", mes);

            bus.Publish(new Message { message = mes });
        }
    }
}
