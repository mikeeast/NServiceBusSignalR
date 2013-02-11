using NServiceBus;

namespace NServiceBusSignalR.ConsoleApp
{
    public class NotifyReceivedMessage : IHandleMessages<IMessage>
    {
        readonly MessageHubProxy messageHubProxy;

        public NotifyReceivedMessage(MessageHubProxy messageHubProxy)
        {
            this.messageHubProxy = messageHubProxy;
        }

        public void Handle(IMessage message)
        {
            messageHubProxy.Send("Hello, I'm hijacking the hub from here. And I intend to do so for each received message.");
        }
    }
}