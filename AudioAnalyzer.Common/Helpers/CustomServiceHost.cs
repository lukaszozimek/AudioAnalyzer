using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;

namespace AudioAnalyzer.Common.Helpers
{
    public static class CustomServiceHost
    {
        public static ServiceHost CreateCustomServiceHost(ServiceHost _serviceHost,Type serviceType,string serviceHttp,string serviceNetTcp, String baseAddress, Type contratc)
        {
            Uri[] baseAddreses = new Uri[2] { new Uri(serviceHttp), new Uri(serviceNetTcp) };
            _serviceHost = new ServiceHost(serviceType, baseAddreses);
            ServiceMetadataBehavior behavior = new ServiceMetadataBehavior();
            ServiceDebugBehavior debug = _serviceHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            if (debug == null)
            {
                _serviceHost.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });
            }
            else
            {
                if (!debug.IncludeExceptionDetailInFaults)
                {
                    debug.IncludeExceptionDetailInFaults = true;
                }
            }
            behavior.HttpGetEnabled = true;
            _serviceHost.Description.Behaviors.Add(behavior);
            Binding mexBinding = MetadataExchangeBindings.CreateMexHttpBinding();
            _serviceHost.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");
            NetTcpBinding binding1 = new NetTcpBinding(SecurityMode.None);
            _serviceHost.AddServiceEndpoint(contratc, binding1, baseAddress, baseAddreses[1]);
            return _serviceHost;
        
        }
    }
}
