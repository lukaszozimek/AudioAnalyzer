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
        public AudioAnalyzerConfiguration Config {  get; private set; }
        public NetworkConfiguration NetConfiguration {  get; private set; }
        public AudioConfiguration AudioConfig {  get; private set; }
        private static Configuration _config;
        public List<Meter> Meters { get { return AudioConfig.Meters; } }
        public Publisher.DistributorClient Proxy{ get{ return NetConfiguration.Client;} }
        public string ServiceAdress { get {return Config.ServiceAdress; } }
        public ushort ServicePort { get{ return Config.ServicePort;} }
        public MMDeviceCollection Devices { get { return AudioConfig.Devices.EnumerateAudioEndPoints(DataFlow.All, DeviceState.All); } }
        public Publisher.DistributorClient Client { get{return NetConfiguration.Client;}}
        public List<StationContainer> StationContainers { get { return AudioConfig.Content; } }
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
