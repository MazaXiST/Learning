using System;
using System.Threading;
using System.Threading.Tasks;
using Rebus.Activation;
using Rebus.Config;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Messages
{
    public interface IReceive
    {
        void WaitToReceive();
    }

    public class Receiver : IReceive, IHostedService
    {
        private readonly ILogger _logger;
        BuiltinHandlerActivator adapter = new BuiltinHandlerActivator();
        private readonly IHostApplicationLifetime _appLifetime;

        public Receiver(
                        IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
        }

        public void WaitToReceive()
        {
                adapter.Handle<Message>(async method =>
                {
                    Console.WriteLine("Processing message {0}", method.message);
                });

                Configure.With(adapter)
                    .Logging(l => l.ColoredConsole(Rebus.Logging.LogLevel.Warn))
                    .Transport(t => t.UseRabbitMq("amqp://user:12345@10.15.0.15", "consumer"))
                    .Options(o => o.SetMaxParallelism(5))
                    .Start();

                adapter.Bus.Subscribe<Message>().Wait();

                Console.WriteLine("Consumer listening!");
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStarted.Register(OnStarted);
            WaitToReceive();
            _appLifetime.ApplicationStopping.Register(OnStopping);
            _appLifetime.ApplicationStopped.Register(OnStopped);

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            Console.WriteLine("OnStarted has been called.");
        }

        private void OnStopping()
        {
            Console.WriteLine("OnStopping has been called.");
        }

        private void OnStopped()
        {
            Console.WriteLine("OnStopped has been called.");
        }

    }
}
