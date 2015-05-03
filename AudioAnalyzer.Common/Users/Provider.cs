using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AudioAnalyzer.Users
{
    [Serializable()]
  public class Provider : User
    {
        public List<StationContainer> ProviderContent { get; set; }
        public bool IsAlive { get; set; }

        public Provider(List<StationContainer> content) 
        {
            ProviderContent = content;
        
        
        }
    }
}
