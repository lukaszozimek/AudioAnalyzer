using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Common.Audio.Helpers;
using AudioAnalyzer.Common.Callbacks;
using AudioAnalyzer.Helpers;
using AudioAnalyzer.Provider.Server.Configuration;
using AudioAnalyzer.Provider.Server.Contract;
using AudioAnalyzer.Provider.Server.Tasks;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace AudioAnalyzer.Provider.Server
{
  [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
    public sealed partial class DataProvider : IDataProvider
    {
#region Variables
        private CancellationToken _cancelationMenager;
        private CancellationTokenSource _tokenMenager;
        public TaskChain audioCatch; 
        public TaskChain networkCatch; 
        Publisher.DistributorClient _client = null;
        private NetworkHelpers _hostChecker = null;
        private bool _isSeviceAlive;
#endregion    

        private AudioAnalyzerConfiguration DeserializeConfiguration()
        {
            var configurationOfAudioCatcher = new AudioAnalyzerConfiguration();
            configurationOfAudioCatcher = AudioAnalyzerConfiguration.DeserializationConfiguration(configurationOfAudioCatcher);
            return configurationOfAudioCatcher;
        }

        List<Station> IDataProvider.GetTrivialData()
        {
            throw new NotImplementedException();
        }

        public void ChangeConfigurationOfAudioCatcher(AudioAnalyzerConfiguration configFile)
        {
            AudioAnalyzerConfiguration.SerializeConfiguration(configFile);
      
        }

        public List<string> GetHostAudioInterfaces()
        {
            List<string> list = new List<string>();
            foreach (MMDevice device in Configuration.Configuration.Instance().Devices)
            {
                list.Add(device.FriendlyName);
            }
            return list;
        }
        
        public void StartApplication()
        {
            Configuration.Configuration.Instance();
            CreateHandlers();
            Configuration.Configuration.Instance().SetProvider();
            TryConnect();
            this.Menager();
        }

        private void CreateHandlers()
        {
            networkCatch = new NetworkTaskChain(Configuration.Configuration.Instance());
            audioCatch = new AudioTaskChain(Configuration.Configuration.Instance());
            audioCatch.SetSuccesor(networkCatch);
        }

        private void TryConnect()
        {
            try
            {
               var ProxyObject = Configuration.Configuration.Instance().Proxy;
                ProxyObject.Open();
                Configuration.Configuration.Instance().SetSystemRole();
                
            }
            catch (Exception)
            {
                CheckRemoteHost();
            }

        }

        private void CheckRemoteHost()
        {
            this._hostChecker = new NetworkHelpers(Configuration.Configuration.Instance().ServiceAdress, Configuration.Configuration.Instance().ServicePort);
            _isSeviceAlive = this._hostChecker.CheckHostAlive();
            if (_isSeviceAlive)
            {
                this._hostChecker = null;
                this.StartApplication();
            }
            else
            {
                int i = 0;
                do
                {
                    _isSeviceAlive = this._hostChecker.CheckHostAlive();
                    Console.WriteLine(i += 1);
                }
                while (_isSeviceAlive == false);
            }
            Console.WriteLine("Reconnected");
            this._hostChecker = null;
            Configuration.Configuration.Instance().Dispose() ;
            this.StartApplication();
        }

        private void Menager()
        {
            _tokenMenager = new System.Threading.CancellationTokenSource();
            _cancelationMenager = new System.Threading.CancellationToken();
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        audioCatch.RequestHandler();

                    }
                    catch (Exception)
                    {
                        CheckRemoteHost();                        
                    }
                    
                }
            }, _cancelationMenager);
        }

        void IDataProvider.StopCatching()
        {

        }
        
        public List<StationContainer> GetTrivialData()
        {
            return Configuration.Configuration.Instance().StationContainers;
        }

        public class ServiceCallback : Publisher.IDistributorCallback
        {
            public void ContextChanged(Common.Audio.AudioInterface.Model.StationContainer data)
            {
                throw new NotImplementedException();
            }
        }

    }
}