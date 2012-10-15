using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;
using NServiceBusSignalR.Web.Hubs;
using SignalR;

namespace NServiceBusSignalR.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteTable.Routes.MapHubs();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var messageHub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                    messageHub.Clients.setCurrentTime(DateTime.Now.ToString("HH:mm:ss"));
                    System.Threading.Thread.Sleep(1000);
                }
            })
            .ContinueWith(t => { throw new Exception("The task threw an exception", t.Exception); }, TaskContinuationOptions.OnlyOnFaulted);

            RegisterBus();
        }

        void RegisterBus()
        {
            Bus = NServiceBus.Configure.With()
                .Log4Net()
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

        public static IBus Bus { get; set; }
    }
}