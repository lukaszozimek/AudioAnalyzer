using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AudioAnalyzer.Common.Callbacks
{
  public interface IDistributorCallback
    {
            [OperationContract(IsOneWay = true)]
            void ContextChanged(StationContainer data);


    }
}
