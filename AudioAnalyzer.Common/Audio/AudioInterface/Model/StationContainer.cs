using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.ComponentModel;
namespace AudioAnalyzer.Common.Audio.AudioInterface.Model
{
    public class StationContainer
    {
        [DataMember]
        public int MonitorID { get; set; }
        private List<Station> _monitorContent;

        public List<Station> MonitorContent
        {
            get { return _monitorContent; }
            set {
               
                    _monitorContent = value;
                 
                
            }
        }


        
   }
}
