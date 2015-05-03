using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Helpers;
using AudioAnalyzer.Provider.Server.Configuration;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioAnalyzer.Provider.Server.Configuration
{
    public class AudioConfiguration : IConfiguration ,IDisposable
    {
        public MMDeviceEnumerator Devices { get; set; }
        public List<StationContainer> Content { get; set; }
        public List<Station> Stations { get;  set; }
        public List<Meter> Meters { get;  set; }
        public AudioAnalyzerConfiguration Config { get; set; }
        public AudioConfiguration(AudioAnalyzerConfiguration config)
        {
            Devices = new MMDeviceEnumerator();
            Content = new List<StationContainer>();
            Stations = new List<Station>();
            Meters = new List<Meter>();
            this.Config = config;
        }
        public void PrepareConfiguration()
        {
            CreateCollectionOfDevice();
        }
        private void CreateCollectionOfDevice()
        {
            foreach (var monitor in this.Config.MonitorID)
            {
                StationContainer monitorDataSend = new StationContainer();
                foreach (var station in monitor.Station)
                {
                    GetStation(station);
                }
                monitorDataSend.MonitorID = monitor.MonitorID;
                monitorDataSend.MonitorContent = Stations;
                Content.Add(monitorDataSend);
            }
            
        }
        private Meter AddDevice(Meter meter)
        {
            foreach (MMDevice device in Devices.EnumerateAudioEndPoints(DataFlow.All, DeviceState.All))
            {
                if (device.FriendlyName == meter.FriendlyName)
                {
                    meter.Device = device;
                }
            }
            return meter;
        }
     
        private void GetStation(AudioAnalyzerConfigurationMonitorIDStation station)
        {
            Station stationSend = new Station();
            stationSend.StationID = station.StationID;
            stationSend.StationName = station.StationName;
            stationSend.ListOfMeters = new List<Meter>();
            GetMeters(station, stationSend, station.MeterID.Count());
            Stations.Add(stationSend);
        }
        private void GetMeters(AudioAnalyzerConfigurationMonitorIDStation station, Station stationSend, int numberOfMeters)
        {
            for (int i = 0; i < station.MeterID.Count(); i++)
            {
                Meter catcherModelSend = new Meter();
                catcherModelSend.MeterID = station.MeterID[i].MeterID;
                catcherModelSend.FriendlyName = station.MeterID[i].FriendlyName;
                catcherModelSend.IsActive = station.MeterID[i].IsActive;
                catcherModelSend = AddDevice(catcherModelSend);
                catcherModelSend.HostIPAdress = NetworkHelpers.ResolveIpAdress();
                stationSend.ListOfMeters.Add(catcherModelSend);
                if (catcherModelSend.IsActive == true)
                {
                    Meters.Add(catcherModelSend);

                }
            }

        }

        public void Dispose()
        {

            this.Devices = null;
            this.Config = null;
            this.Stations = null;
            this.Meters = null;
        }
    }
}
