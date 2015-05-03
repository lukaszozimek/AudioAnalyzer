
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Common.Callbacks;
using AudioAnalyzer.Users;
namespace AudioAnalyzer.Distributor.Server
{

    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IDistributorCallback))]
    public interface IDistributor
    {
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        bool Subscribe(Subscriber subscriber);
        [OperationContract(IsOneWay = false, IsInitiating = true)]
        void LeaveSubscription(Subscriber subscriber);
        [OperationContract(IsOneWay = false)]
        void PublishContextChange(List<StationContainer> data);
        [OperationContract]
        void ConnectPublisher(Provider dataProvider);
    }
    


}

