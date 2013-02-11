using NServiceBus;

namespace NServiceBusSignalR.ConsoleApp
{
    public class MessageOrderHandlingSpecifier : ISpecifyMessageHandlerOrdering
    {
        public void SpecifyOrder(Order order)
        {
            order.SpecifyFirst<NotifyReceivedMessage>();
        }
    }
}