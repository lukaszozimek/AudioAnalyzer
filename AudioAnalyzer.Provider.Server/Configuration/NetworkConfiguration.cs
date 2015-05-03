using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Common.Callbacks;
using AudioAnalyzer.Helpers;
using AudioAnalyzer.Provider.Server.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AudioAnalyzer.Provider.Server.Configuration
{
   public class NetworkConfiguration :IConfiguration, IDisposable
    {
        public AudioAnalyzerConfiguration Config { get; set; }

        public Publisher.DistributorClient Client { get; private set; }

        public Users.Provider Provider { get; private set; }
        
        public NetworkConfiguration(AudioAnalyzerConfiguration config)
        {
            this.Config = config;
        }

        public void PrepareConfiguration()
        {
            var bindingAdressContract = CreateWFC();
            Client = new Publisher.DistributorClient(bindingAdressContract.Item3, bindingAdressContract.Item1, bindingAdressContract.Item2);
           
            
        }

        public void CreateProvider(List<StationContainer> content)
        {
            Provider = new Users.Provider(content);
            Provider.IpAdress = NetworkHelpers.ResolveIpAdress();
        }

        public void Dispose()
        {
            Config = null;
            if (Client.State != CommunicationState.Faulted)
            {
                Client.Close();

            }
            Client = null;
            Provider = null;
        }

        private Tuple<NetTcpBinding, EndpointAddress, InstanceContext> CreateWFC()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpoint = new EndpointAddress("net.tcp://" + this.Config.ServiceAdress + ":" + this.Config.ServicePort.ToString() + "/Catch");
            InstanceContext _context = new InstanceContext(new DataProvider.ServiceCallback());
            return new Tuple<NetTcpBinding, EndpointAddress, InstanceContext>(binding, endpoint, _context);
        }
        
    }
}
