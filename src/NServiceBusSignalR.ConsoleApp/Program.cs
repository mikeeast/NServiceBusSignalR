using System;
using NServiceBus;
using NServiceBusSignalR.Messages;

namespace NServiceBusSignalR.ConsoleApp
{
    class Program
    {
        protected static IBus Bus { get; set; }

        static void Main(string[] args)
        {
            Console.WindowWidth = 160;
            Console.ForegroundColor = ConsoleColor.Green;

            RegisterBus();

            System.Threading.Thread.Sleep(2000);
            
            while(true)
            {
                Console.WriteLine("What would you like to send?");
                var message = Console.ReadLine();
                Bus.Send(new DistributedMessage { Content = message });
            }
        }

        static void RegisterBus()
        {
            NServiceBus.SetLoggingLibrary.Log4Net(log4net.Config.XmlConfigurator.Configure);
            Bus = NServiceBus.Configure.With()
                .DefaultBuilder()
                .XmlSerializer()
                .DisableTimeoutManager()
                .MsmqTransport()
                    .IsTransactional(false)
                    .PurgeOnStartup(false)
                .UnicastBus()
                    .ImpersonateSender(false)
                    .LoadMessageHandlers()
                .CreateBus()
                .Start();
        }
    }
}
