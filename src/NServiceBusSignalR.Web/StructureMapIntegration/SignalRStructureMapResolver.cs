using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using StructureMap;

namespace NServiceBusSignalR.Web.StructureMapIntegration
{
    public class SignalRStructureMapResolver : DefaultDependencyResolver
    {
        readonly IContainer container;

        public SignalRStructureMapResolver(IContainer container)
        {
            this.container = container;
        }

        public override object GetService(Type serviceType)
        {
            object service = null;
            if (!serviceType.IsAbstract && !serviceType.IsInterface && serviceType.IsClass)
            {
                // Concrete type resolution
                service = container.GetInstance(serviceType);
            }
            else
            {
                // Other type resolution with base fallback
                service = container.TryGetInstance(serviceType) ?? base.GetService(serviceType);
            }
            return service;
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            var objects = container.GetAllInstances(serviceType).Cast<object>();
            return objects.Concat(base.GetServices(serviceType));
        }
    }
}