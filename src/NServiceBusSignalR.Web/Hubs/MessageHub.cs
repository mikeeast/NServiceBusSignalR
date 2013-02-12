using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using NServiceBus;
using NServiceBusSignalR.Messages;

namespace NServiceBusSignalR.Web.Hubs
{
    [HubName("messageHub")]
    public class MessageHub : Hub
    {
        readonly IBus bus;

        public MessageHub(IBus bus)
        {
            this.bus = bus;
        }
        
        public void Send(string message)
        {
            Clients.AllExcept(Context.ConnectionId).notify(message);
        }

        public void SendLoopback(string message)
        {
            bus.Send(new LoopbackMessage { Content = message });
        }

        public void SendDistributed(string message)
        {
            bus.Send(new DistributedMessage { Content = message });
        }
    }
}