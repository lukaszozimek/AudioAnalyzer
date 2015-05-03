using AudioAnalyzer.Common.Callbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AudioAnalyzer.Provider.Server.Contract
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IProviderCallback))]
    public interface IDetalis
    {
        [OperationContract(IsOneWay = true)]
        void GetDetails();

        [OperationContract(IsOneWay = true)]
        void Capture();

        [OperationContract]
        void RequestMp3();

        [OperationContract]
        void RequestPCM();
    }
}
