using System;
using System.Collections.Generic;
using NAudio.CoreAudioApi;

using System.Runtime.Serialization;
using AudioAnalyzer.Common.Audio.AudioInterface;

namespace AudioAnalyzer.Common.Audio.AudioInterface.Model
{
    [DataContract]
    public sealed class Meter : StereoAudioInterface
    {
        private bool _isActive;
        
        private int _meterID;
        private string _friendlyName;
        private string _hostIPAdress;
   
        [DataMember]
        public string Source { get; set; }
        [DataMember]
        public int MeterID
        {
            get { return _meterID; }
            set
            {
                _meterID = value;
                NotifyPropertyChanged("MeterID");
            }
        }
        [DataMember]
        public string HostIPAdress
        {
            get { return _hostIPAdress; }
            set { _hostIPAdress = value;
            NotifyPropertyChanged("MeterID");
            }
        }
        
        [DataMember]
        public override bool IsActive
        {
            get { return _isActive; }
            set
            {
                _isActive = value;
                NotifyPropertyChanged("IsActive");
            }
        }
        public string FriendlyName
        {
            get { return _friendlyName; }
            set { _friendlyName = value; }
        }

        public MMDevice Device { get; set; }
    }
}
