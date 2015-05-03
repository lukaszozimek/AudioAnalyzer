using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Helpers;
using AudioAnalyzer.Common.Callbacks;
using AudioAnalyzer.Users;

namespace AudioAnalyzer.Distributor.Server
{
    /// <summary>
    /// Procesing Danych przysyłanych przez Catchery

    /// TODO: Subskrypcja innych typów danych
    /// TODO: Określenie kto rząda i jakich danych
    /// TODO: Processing Słowników bardziej uniewersalny
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class Distributor : IDistributor
    {
        private delegate void MonitorContextChangeEventHandler(object sender, DistributorEventArgs e);
        private static event MonitorContextChangeEventHandler MoniotorChangeContextEvent;
        private readonly object sync = new object();
        private Dictionary<int, StationContainer> _catchersDictionary = new Dictionary<int, StationContainer>();
        private MonitorContextChangeEventHandler ChangeContext = null;
        private Dictionary<Subscriber, MonitorContextChangeEventHandler> _subscribers = new Dictionary<Subscriber, MonitorContextChangeEventHandler>();
        private List<Provider> _providerList;
        private Timer _providerTimer;

        public  Distributor()
        {
            _catchersDictionary.Add(1, new StationContainer());
            _catchersDictionary.Add(2, new StationContainer());
            _catchersDictionary.Add(3, new StationContainer());
            _providerList = new List<Provider>();
            _providerTimer = new Timer(10000);
            _providerTimer.Elapsed += _providerTimer_Elapsed;
            _providerTimer.Enabled = true;
            _providerTimer.AutoReset = true;
            _providerTimer.Start();
            Console.WriteLine("InvokeConstructor");
        }
        
        public void Unsubscribe()
        {
            MoniotorChangeContextEvent -= ChangeContext;
        }

        public void PublishContextChange(List<StationContainer> Data)
        {
            lock (sync)
            {
                foreach (var singleMonitor in Data)
                {
                  
                        if (_catchersDictionary[singleMonitor.MonitorID].MonitorContent == null)
                        {
                            _catchersDictionary[singleMonitor.MonitorID] = singleMonitor;
                        }
                        else
                        {
                            SetContent(singleMonitor);
                        }
                        if (singleMonitor != null)
                        {
                            Task.Run(() =>
                            {
                                InvokeSendEvent(GetSubscribers(singleMonitor), singleMonitor);
                            });
                        }
                    
                    //        if (_catchersDictionary[singleMonitor.MonitorID].MonitorContent == null)
                    //        {
                    //            _catchersDictionary[singleMonitor.MonitorID] = singleMonitor;
                    //        }
                    //        else
                    //        {
                    //            for (int i = 0; i < _catchersDictionary[singleMonitor.MonitorID].MonitorContent.Count; i++)
                    //            {
                    //                for (int z = 0; z < singleMonitor.MonitorContent.Count; z++)
                    //                {
                    //                    var stationExist = _catchersDictionary[singleMonitor.MonitorID].MonitorContent.Exists(station => station.StationID.Equals(singleMonitor.MonitorContent[z].StationID));
                    //                    if (stationExist)
                    //                    {
                    //                        if (_catchersDictionary[singleMonitor.MonitorID].MonitorContent[i].StationID == singleMonitor.MonitorContent[z].StationID)
                    //                        {
                    //                            for (int i1 = 0; i1 < _catchersDictionary[singleMonitor.MonitorID].MonitorContent[i].ListOfMeters.Count; i1++)
                    //                            {
                    //                                for (int z1 = 0; z1 < singleMonitor.MonitorContent[z].ListOfMeters.Count; z1++)
                    //                                {
                    //                                    var meterExist = _catchersDictionary[singleMonitor.MonitorID].MonitorContent[i].ListOfMeters.Exists(station => station.MeterID.Equals(singleMonitor.MonitorContent[z].ListOfMeters[z1].MeterID));
                    //                                    if (meterExist)
                    //                                    {
                    //                                        int dicValIndex = _catchersDictionary[singleMonitor.MonitorID].MonitorContent[i].ListOfMeters.FindIndex(station => station.MeterID.Equals(singleMonitor.MonitorContent[z].ListOfMeters[z1].MeterID));

                    //                                        _catchersDictionary[singleMonitor.MonitorID].MonitorContent[i].ListOfMeters[dicValIndex] = singleMonitor.MonitorContent[z].ListOfMeters[z1];
                    //                                        Console.WriteLine(singleMonitor.MonitorContent[z].ListOfMeters[z1].LeftCHannelLevel);
                    //                                        Console.WriteLine(singleMonitor.MonitorContent[z].ListOfMeters[z1].RightChannelLevel);

                    //                                    }
                    //                                    else
                    //                                    {
                    //                                        AddMeters(singleMonitor.MonitorID, i, singleMonitor, z, z1);
                    //                                        Console.WriteLine(singleMonitor.MonitorContent[z].ListOfMeters[z1].LeftCHannelLevel);
                    //                                        Console.WriteLine(singleMonitor.MonitorContent[z].ListOfMeters[z1].RightChannelLevel);
                    //                                    }
                    //                                }
                    //                            }
                    //                        }
                    //                    }
                    //                    else
                    //                    {
                    //                        SetContent(singleMonitor.MonitorID, singleMonitor, z);
                    //                    }
                    //                }
                    //            }
                    //            if (singleMonitor != null)
                    //            {
                    //                Task.Run(() =>
                    //                {
                    //                    InvokeSendEvent(GetSubscribers(singleMonitor), singleMonitor);
                    //                });
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
        }
        
        public void LeaveSubscription(Subscriber sub)
        {
            var _currentSubscriber = from _sub in _subscribers
                                     where _sub.Key.IpAdress == sub.IpAdress
                                     select _sub.Key;
            _subscribers.Remove(_currentSubscriber.FirstOrDefault());
            Console.WriteLine("Subscriber is Gone");


        }
        
        public void ConnectPublisher(Provider _dataProvider)
        {
            _dataProvider.IsAlive = true;
            _providerList.Add(_dataProvider);

        }
        
        bool IDistributor.Subscribe(Subscriber sub)
        {
            try
            {
                sub.ServiceCallback = OperationContext.Current.GetCallbackChannel<IDistributorCallback>();
                sub.Monitor = _catchersDictionary[sub.DictionaryID];
                MoniotorChangeContextEvent = new MonitorContextChangeEventHandler(PublishContextChangeHandler);
                MoniotorChangeContextEvent += ChangeContext;
                this._subscribers.Add(sub, MoniotorChangeContextEvent);
                Console.WriteLine("Connected");
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        
        private void PublishContextChangeHandler(object sender, DistributorEventArgs se)
        {
            try
            {
                lock (sync)
                {
                    Console.WriteLine("TryPublish");
                    if (se.subcriber.ServiceCallback != null)
                    {
                        se.subcriber.ServiceCallback.ContextChanged(se.data);
                    }
                }
            }
            catch (Exception ex)
            {

                Logger.Instance.ServiceDistributorDebug(ex);

            }
        }
        
        private void _providerTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_providerList.Count == 0)
            {
                Console.WriteLine("No Providers here");
            }
            else
            {

                foreach (var provider in _providerList)
                {
                    provider.IsAlive = NetworkHelpers.CheckHostAlive(provider.IpAdress, 8024);
                    if (!provider.IsAlive)
                    {
                        RemoveProvider(provider, _providerList);
                        lock (sync)
                        {
                            foreach (var singleMonitor in provider.ProviderContent)
                            {
                                if (_catchersDictionary[singleMonitor.MonitorID].MonitorContent == null)
                                {
                                    _catchersDictionary[singleMonitor.MonitorID] = singleMonitor;
                                }
                                else
                                {
                                    SetContent(singleMonitor);
                                }
                                if (singleMonitor != null)
                                {
                                    Task.Run(() =>
                                    {
                                        InvokeSendEvent(GetSubscribers(singleMonitor), singleMonitor);
                                    });
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private void SetContent(StationContainer singleMonitor)
        {

            for (int i = 0; i < GetNumberOfStations(singleMonitor.MonitorID); i++)
            {
                for (int z = 0; z < singleMonitor.MonitorContent.Count; z++)
                {
                    if (IsStationExist(singleMonitor.MonitorID, singleMonitor, z))
                    {
                        IterateMetersInFirsCollection(singleMonitor, i, z);
                    }
                    else
                    {
                        SetContent(singleMonitor.MonitorID, singleMonitor, z);
                    }
                }
            }
        }
        
        private void IterateMetersInFirsCollection(StationContainer singleMonitor,int firstIndex, int secondIndex)
        {
            if (CompareStations(singleMonitor.MonitorID, firstIndex, singleMonitor, secondIndex))
            {
                for (int i1 = 0; i1 < GetNumberOfMeters(singleMonitor.MonitorID, firstIndex); i1++)
                {
                    IterateMeters(singleMonitor, firstIndex, secondIndex);
                }
            }
            
        }
        
        private void IterateMeters(StationContainer singleMonitor, int firstIndex, int secondIndex)
        {
            for (int z1 = 0; z1 < GetNumberOfMeters(singleMonitor, secondIndex); z1++)
            {
                SetStation(singleMonitor, firstIndex, secondIndex, z1);
            }
        }
        
        private void SetStation(StationContainer singleMonitor, int firstIndex, int secondIndex, int thirdIndex)
        {
            if (IsMeterExist(singleMonitor.MonitorID, firstIndex, singleMonitor, secondIndex, thirdIndex))
            {

                SetMeterError(singleMonitor.MonitorID, firstIndex, GetIndexOfDictionary(singleMonitor.MonitorID, firstIndex, singleMonitor, secondIndex, thirdIndex));

            }
            else
            {
                AddMeters(singleMonitor.MonitorID, firstIndex, singleMonitor, secondIndex, thirdIndex);

            }
        }

        
        private void RemoveProvider(Provider provider ,List<Provider> providerList) 
        {
            var _provider = from providers in _providerList
                            where providers.IpAdress == provider.IpAdress
                            select provider;
            _providerList.Remove(_provider.FirstOrDefault());
            ///MiernikiErrror
            Console.WriteLine("ProviderIsDead");

            Logger.Instance.WriteSimpleMessag(DateTime.Now.ToString() + " " + "Remote Host is Dead" + provider.IpAdress);

        }
        
        private bool IsStationExist(  int MonitorID,StationContainer singleMonitor , int contentIndex ) 
        {
            return _catchersDictionary[MonitorID].MonitorContent.Exists(station => station.StationID.Equals(singleMonitor.MonitorContent[contentIndex].StationID));
        }
        
        private bool IsMeterExist(int monitorIndex, int contetnIndex, StationContainer singleMonitor, int secondMonitorIndex, int listMeterIndex)
        {
            return _catchersDictionary[monitorIndex].MonitorContent[contetnIndex].ListOfMeters.Exists(station => station.MeterID.Equals(singleMonitor.MonitorContent[secondMonitorIndex].ListOfMeters[listMeterIndex].MeterID));
        }
        
        private bool CompareStations(int monitorID, int monitorContentIndex, StationContainer singleMonitor , int stationIndex )
        {
            return _catchersDictionary[monitorID].MonitorContent[monitorContentIndex].StationID == singleMonitor.MonitorContent[stationIndex].StationID;
        }
        
        private int  GetNumberOfMeters(int monitorIndex, int stationIndex)
        {
            return _catchersDictionary[monitorIndex].MonitorContent[stationIndex].ListOfMeters.Count;
        }
      
        private int  GetNumberOfMeters(StationContainer singleMonitor,int monitorIndex)
        {
            return singleMonitor.MonitorContent[monitorIndex].ListOfMeters.Count;
        }
        
        private int  GetNumberOfStations(int monitorIndex)
        {
            return _catchersDictionary[monitorIndex].MonitorContent.Count;
        }
        
        private int  GetIndexOfDictionary (int monitorID, int contentIndex, StationContainer monitor, int monitorIndex, int indexer)
        {
            return _catchersDictionary[monitorID].MonitorContent[contentIndex].ListOfMeters.FindIndex(station => station.MeterID.Equals(monitor.MonitorContent[monitorIndex].ListOfMeters[indexer].MeterID));
        }
        
        private void SetMeterError(int MonitorID, int stationIndex, int meterIndex)
        {
            _catchersDictionary[MonitorID].MonitorContent[stationIndex].ListOfMeters[meterIndex].Error = true;
            _catchersDictionary[MonitorID].MonitorContent[stationIndex].ListOfMeters[meterIndex].LeftCHannelLevel = -60;
            _catchersDictionary[MonitorID].MonitorContent[stationIndex].ListOfMeters[meterIndex].RightChannelLevel = -60;
        }
        
        private void AddMeters(int monitorID, int monitorIndex, StationContainer container, int containerIndex, int metersIndex)
        {
            _catchersDictionary[monitorID].MonitorContent[monitorIndex].ListOfMeters.Add(container.MonitorContent[containerIndex].ListOfMeters[metersIndex]);
        }
        
        private void SetContent(int monitorID, StationContainer container, int stationIndex) 
        {
            _catchersDictionary[monitorID].MonitorContent.Add(container.MonitorContent[stationIndex]);
        }
        
        private IEnumerable<Subscriber> GetSubscribers(StationContainer singleMonitor)
        {
            var tempSub = from subscribers in this._subscribers.Keys
                          where subscribers.DictionaryID == _catchersDictionary[singleMonitor.MonitorID].MonitorID
                          select subscribers;
            return tempSub;                
        }
        
        private void InvokeSendEvent(IEnumerable<Subscriber> tempSub, StationContainer singleMonitor)
        {
            foreach (var item in tempSub)
            {
                DistributorEventArgs se = new DistributorEventArgs();
                se.subcriber = item;
                if (se.subcriber != null)
                {
                    foreach (var sub in _subscribers)
                    {
                        se.data = _catchersDictionary[sub.Key.DictionaryID];
                        if (singleMonitor.MonitorID == sub.Key.DictionaryID)
                        {
                            sub.Value.Invoke(this, se);
                        }
                    }
                }
            }
        }
       
      }
}