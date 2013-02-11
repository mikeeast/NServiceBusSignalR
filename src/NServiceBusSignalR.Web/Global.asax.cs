using System;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;
using NServiceBus;
using NServiceBusSignalR.Web.Hubs;
using NServiceBusSignalR.Web.StructureMapIntegration;
using StructureMap;
using IDependencyResolver = Microsoft.AspNet.SignalR.IDependencyResolver;

namespace NServiceBusSignalR.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            InitialiseStructureMap();

            RouteTable.Routes.MapHubs();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var messageHub = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();
                    messageHub.Clients.All.setCurrentTime(DateTime.Now.ToString("HH:mm:ss"));
                    System.Threading.Thread.Sleep(1000);
                }
            })
            .ContinueWith(t => { throw new Exception("The task threw an exception", t.Exception); }, TaskContinuationOptions.OnlyOnFaulted);

            RegisterBus();
        }

        private void InitialiseStructureMap()
        {
            ObjectFactory.Initialize(i => 
                i.For<IDependencyResolver>().Singleton().Add<SignalRStructureMapResolver>()
            );
            
            GlobalHost.DependencyResolver = ObjectFactory.Container.GetInstance<Microsoft.AspNet.SignalR.IDependencyResolver>();
        }

        void RegisterBus()
        {
            Bus = NServiceBus.Configure.With()
                .StructureMapBuilder()
                .Log4Net()
                .XmlSerializer()
                .MsmqTransport()
                .UnicastBus()
                .CreateBus()
                .Start();
        }

        public static IBus Bus { get; set; }
    }
}