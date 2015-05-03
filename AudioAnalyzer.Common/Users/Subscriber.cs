using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Common.Callbacks;

namespace AudioAnalyzer.Users
{
    
    [Serializable()]
   public class Subscriber: User
    {
        public int DictionaryID { get; set; }
        public int SingleInstanceID { get; set; }
        public StationContainer Monitor { get; set; }
        public IDistributorCallback ServiceCallback { get; set; } 
    }
}
