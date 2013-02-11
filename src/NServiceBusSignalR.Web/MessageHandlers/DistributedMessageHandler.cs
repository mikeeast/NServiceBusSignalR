using Microsoft.AspNet.SignalR;
using NServiceBus;
using NServiceBusSignalR.Messages;
using NServiceBusSignalR.Web.Hubs;

namespace NServiceBusSignalR.Web.MessageHandlers
{
    public class DistributedMessageHandler : 
        IHandleMessages<DistributedMessage>,
        IHandleMessages<LoopbackMessage>

    {
        public void Handle(DistributedMessage message)
        {
            NotifyClients(message.Content);
        }

        public void Handle(LoopbackMessage message)
        {
            NotifyClients(message.Content);
        }

        static void NotifyClients(string content)
        {
            var messageHub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
            messageHub.Clients.All.notifyDistributed(content);
        }
    }
}