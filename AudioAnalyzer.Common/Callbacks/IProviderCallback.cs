using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Common.Audio.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AudioAnalyzer.Common.Callbacks
{
  public  interface IProviderCallback
    {
        [OperationContract(IsOneWay = true)]
        void SendCatchingData(ObservableCollection<Meter> result);
        [OperationContract(IsOneWay = true)]
        void SendFTTArgs(FftEventArgs fttData);
        [OperationContract(IsOneWay = true)]
        void SendMaxSapleCalculaterd(MaxSampleEventArgs maxSampleData);
        [OperationContract(IsOneWay = true)]
        void SendEncodedMP3(Mp3EncodedeEventArgs _mp3Data);
        [OperationContract(IsOneWay = true)]
        void SendEncodedPCM(PCMBufferEventArgs _pcmData);
    }
}
