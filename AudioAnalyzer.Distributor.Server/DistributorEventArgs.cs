using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Users;



namespace AudioAnalyzer.Distributor.Server
{
   public class DistributorEventArgs : EventArgs
   {
        public StationContainer data;
        public Subscriber subcriber;
   }
}
