using System;
using NServiceBus;

namespace NServiceBusSignalR.Messages
{
    public class DistributedMessage : IMessage
    {
        public string Content { get; set; }
        public bool ReportToHub { get; set; }
    }

    public class LoopbackMessage : IMessage
    {
        public string Content { get; set; }
    }
}
