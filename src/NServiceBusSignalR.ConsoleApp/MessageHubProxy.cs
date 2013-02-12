using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;

namespace NServiceBusSignalR.ConsoleApp
{
    public class MessageHubProxy
    {
        readonly IHubProxy messageHub;

        public MessageHubProxy()
        {
            var hubConnection = new HubConnection("http://localhost:8001/");
            messageHub = hubConnection.CreateHubProxy("messageHub");
            hubConnection.StateChanged += change =>
                                          Console.WriteLine(change.OldState + " => " + change.NewState);

            messageHub.On<string>("Send", s => Console.WriteLine("Some client used send -> {0}", s));
            
            hubConnection.Start().Wait();
        }
        
        public void Send(string message)
        {
            messageHub.Invoke("send", message).ContinueWith(task =>
                {
                    using (var error = task.Exception.GetError())
                    {
                        Console.WriteLine(error);
                    }
                }, TaskContinuationOptions.OnlyOnFaulted);
        }
    }
}