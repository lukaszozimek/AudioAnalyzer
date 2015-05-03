using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Common.Callbacks;
using AudioAnalyzer.Helpers;
using AudioAnalyzer.Provider.Server.Configuration;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioAnalyzer.Provider.Server.Configuration
{
   public class Configuration  :IDisposable
    {
        private AudioAnalyzerConfiguration Config {  get;  set; }
        private NetworkConfiguration NetConfiguration {  get;  set; }
        private AudioConfiguration AudioConfig {  get;  set; }
        private static Configuration _config;

        public static Configuration Instance()
       {
       
            if (_config==null)
	        {
                _config = new Configuration();
                return _config;
	        }
            else
            {
                return _config;
            }
       
       }

        public List<StationContainer> GetContainer()
        {
            return AudioConfig.Content;
        }

        public Publisher.DistributorClient GetClient()
        {
            return NetConfiguration.Client;
        }

        public MMDeviceCollection GetDevices()
        {
            return AudioConfig.Devices.EnumerateAudioEndPoints(DataFlow.All, DeviceState.All);
        }

        public ushort GetServicePort()
        {
            return Config.ServicePort;
        }
       
        public string GetServiceAdress()
        {
            return Config.ServiceAdress;
        }

        public Publisher.DistributorClient GetProxyObject()
        {
            return NetConfiguration.Client;
         
        }

        public void SetSystemRole()
        {
            NetConfiguration.Client.ConnectPublisher(NetConfiguration.Provider);
        }

        public void SetProvider()
        {
            NetConfiguration.CreateProvider(AudioConfig.Content);
        }

        public void PublishContext()
       {
           NetConfiguration.Client.PublishContextChange(AudioConfig.Content);
       }

        public List<Meter> GetMeters()
       {
           return AudioConfig.Meters;
       }

        public void Dispose()
        {

            NetConfiguration.Dispose();
            AudioConfig.Dispose();
            Config = null;
            NetConfiguration = null;
            AudioConfig = null;
            _config = null;
        }

        private Configuration()
        {
            Config = DeserializeConfiguration();
            NetConfiguration = new NetworkConfiguration(this.Config);
            AudioConfig = new AudioConfiguration(this.Config);
            NetConfiguration.PrepareConfiguration();
            AudioConfig.PrepareConfiguration();
        }

        private AudioAnalyzerConfiguration DeserializeConfiguration()
        {
            var configurationOfAudioCatcher = new AudioAnalyzerConfiguration();
            configurationOfAudioCatcher = AudioAnalyzerConfiguration.DeserializationConfiguration(configurationOfAudioCatcher);
            return configurationOfAudioCatcher;
        }

   }
}
