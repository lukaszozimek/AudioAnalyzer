using System;
using AudioAnalyzer.Provider.Server.Configuration;
using System.Collections.Generic;
using System.ServiceModel;
using System.Collections.ObjectModel;
using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Common.Audio.Helpers;
using AudioAnalyzer.Common.Callbacks;
namespace AudioAnalyzer.Provider.Server.Contract
{

    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IProviderCallback))]
    public interface IDataProvider
    {
        [OperationContract(IsOneWay = true)]
        void StartApplication();

        [OperationContract(IsOneWay = true)]
        void ChangeConfigurationOfAudioCatcher(AudioAnalyzerConfiguration configFile);

        [OperationContract(IsOneWay = true)]
        void StopCatching();

        [OperationContract]
        List<Station> GetTrivialData();

        [OperationContract]
        List<string> GetHostAudioInterfaces();
 
    }
 
}
