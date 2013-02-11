using System;
using System.Threading;
using NServiceBus;
using NServiceBusSignalR.Messages;

namespace NServiceBusSignalR.ConsoleApp
{
    public class DistributedMessageHandler : IHandleMessages<DistributedMessage>
    {
        public void Handle(DistributedMessage message)
        {
            Console.WriteLine("Web: {0}", message.Content);
        }
    }
}