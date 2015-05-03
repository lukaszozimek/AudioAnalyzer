using System;
using System.Runtime.Serialization;

namespace AudioAnalyzer.Common.Audio.AudioInterface
{[DataContract]
   public class StereoAudioInterface : AudioInterface
   {
        private int _rightChannelLevel;
        private int _leftChannelLevel;
        private int _iD;
        private string _name;
        private bool _error;
        private bool _isActive;

        public override int ID
        {
            get
            {
                return _iD;
            }
            set
            {
                _iD = value;
                NotifyPropertyChanged("ID");
            }
        }
        [DataMember]
        public override string Name
        {
            get
            {
                return _name;
            
            }
            set
            {
                _name = value;

                NotifyPropertyChanged("Name");
            }
        }
        [DataMember]   
        public int RightChannelLevel
       {
           get
           {
               return _rightChannelLevel;
           }
           set
           {
               _rightChannelLevel = value;
               NotifyPropertyChanged("RightChannelLevel");
           }
       }
        [DataMember]   
        public int LeftCHannelLevel
       {
           get
           {
               return _leftChannelLevel;
           }
           set
           {
               _leftChannelLevel = value;
               NotifyPropertyChanged("LeftCHannelLevel");
           }
       }
        public override bool Error
        {
            get
            {
            
                return _error;
            }
            set
            {
            
                _error = value;
                NotifyPropertyChanged("Error");
            }
        }
        public override bool IsActive
       {
           get
           {
               return _isActive;
           }
           set
           {

               _error = value;
               NotifyPropertyChanged("IsActive");
           }
       }
   }
}
