using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AudioAnalyzer.Users;
using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Client.Configuration;
using AudioAnalyzer.Helpers;

namespace AudioAnalyzer.Client.ViewModel
{


    /// <summary>
    /// Filter data by SubscribingMonitorID
    /// </summary>
    public class LiveViewModel : INotifyPropertyChanged
    {
        static DistributorProxy.DistributorClient _client;
        static Subscriber sub;
        public delegate void CatcherDictionaryCallbackHandler(StationContainer Data);
        public static event CatcherDictionaryCallbackHandler CatcherCallBackEvent;
        
        InstanceContext _context;
        CatcherDictionaryCallbackHandler callbackHandler;
        ObservableCollection<Station> monitorCollection;
        AudioAnalyzerClientConfiguration _config;
        private ObservableCollection<Station> _bindableData;
        public ObservableCollection<Station> BindableData
        {
            get { return _bindableData; }
            set
            {
                _bindableData = value;
                OnPropertyChanged(new PropertyChangedEventArgs("BindableData"));
            }
        }
        public LiveViewModel()
        {

            GetConfiguration();
            CreateWCFClient();
            RegisterHandler();
           InitializeSubscribtion();
           
        }
       
        public void UpdateDataContext(StationContainer Data)
        {
            ObservableCollection<Station> tempCollection = new ObservableCollection<Station>(Data.MonitorContent);
            this.BindableData = tempCollection;  
        }

        public  static void StopSubscribig()
        {
          _client.LeaveSubscription(sub);
          _client.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private void GetConfiguration() 
        {
            _config = new AudioAnalyzerClientConfiguration();
            _config = AudioAnalyzerClientConfiguration.DeserializationConfiguration(_config);
        }

        private void CreateWCFClient()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            EndpointAddress endpoint = new EndpointAddress("net.tcp://" + _config.DistributorAdress[0].IpAdress.ToString() + ":" + _config.DistributorAdress[0].CommunicationPort.ToString() + "/Catch");
            _context = new InstanceContext(new ServiceCallback());
            _client = new DistributorProxy.DistributorClient(_context, binding, endpoint);
   
        }

        private void RegisterHandler()
        {
            callbackHandler = new CatcherDictionaryCallbackHandler(UpdateDataContext);
            CatcherCallBackEvent += callbackHandler;
        }
        
        private void InitializeSubscribtion()
        {
            sub = new Subscriber();
            sub.DictionaryID = _config.DistributorAdress[0].MonitorID;
            try
            {
                  _client.Subscribe(sub);
             }
            catch (Exception ex)
            {
                Logger.Instance.ServiceClientDebug(ex);
                throw;
            }
            monitorCollection = new ObservableCollection<Station>();
            this.BindableData = monitorCollection;
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, e);
            }

        }
        [CallbackBehaviorAttribute(UseSynchronizationContext = false)]
        public class ServiceCallback : DistributorProxy.IDistributorCallback
        {
            public void ContextChanged(StationContainer data)
            {
                LiveViewModel.CatcherCallBackEvent(data);
            }
        }

    }
}