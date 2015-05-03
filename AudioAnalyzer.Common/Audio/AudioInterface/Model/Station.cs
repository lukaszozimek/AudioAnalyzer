using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AudioAnalyzer.Common.Audio.AudioInterface.Model
{
   public class Station
   {
       private int _stationID;
       [DataMember]
       public int StationID
       {
           get { return _stationID; }
           set { _stationID = value; }
       }
       [DataMember]
       public string StationName { get; set; }
       [DataMember]
       public List<Meter> ListOfMeters { get; set; }
    }
}
