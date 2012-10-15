using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NServiceBusSignalR.Messages;
using SignalR.Hubs;

namespace NServiceBusSignalR.Web.Hubs
{
    [HubName("messageHub")]
    public class MessageHub : Hub
    {
        public void Send(string message)
        {
            Clients.notify(message);
        }

        public void SendLoopback(string message)
        {
            MvcApplication.Bus.Send(new LoopbackMessage { Content = message });
        }

        public void SendDistributed(string message)
        {
            MvcApplication.Bus.Send(new DistributedMessage { Content = message });
        }
    }
}