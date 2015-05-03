using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
namespace AudioAnalyzer.Common.Audio.AudioInterface
{
   [DataContract]
  public abstract class AudioInterface : INotifyPropertyChanged
    {
        public abstract int ID { get; set; }
        public abstract string Name { get; set; }
        [DataMember]
        public abstract bool Error { get; set; }
		[DataMember] 
		public abstract bool IsActive { get; set; }   
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}

