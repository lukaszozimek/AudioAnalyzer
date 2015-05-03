using AudioAnalyzer.Common.Audio.Helpers;
using AudioAnalyzer.Common.Callbacks;
using AudioAnalyzer.Provider.Server.Contract;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace AudioAnalyzer.Provider.Server
{
   [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, IncludeExceptionDetailInFaults = true)]
   public class AudioDetalis :IDetalis
    {

        int captureSeconds;
        AudioGraph audioGraph;
        Configuration.Configuration appConfiguration;
       
        public int CaptureSeconds
        {
            get
            {
                return captureSeconds;
            }
            set
            {
                if (captureSeconds != value)
                {
                    captureSeconds = value;
                    RaisePropertyChangedEvent("CaptureSeconds");
                }
            }
        }
        public double RecordVolume
        {
            get
            {
                return audioGraph.RecordVolume;
            }
            set
            {
                if (audioGraph.RecordVolume != value)
                {
                    audioGraph.RecordVolume = value;
                    RaisePropertyChangedEvent("RecordVolume");
                }
            }
        }
        public int NotificationsPerSecond
        {
            get
            {
                return audioGraph.NotificationsPerSecond;
            }
            set
            {
                if (NotificationsPerSecond != value)
                {
                    audioGraph.NotificationsPerSecond = value;
                    RaisePropertyChangedEvent("NotificationsPerSecond");
                }
            }
        }
        public IProviderCallback callback;
       
       public AudioDetalis()
        {
            this.audioGraph = new AudioGraph();
           
        }

        public void Capture()
        {

            audioGraph.StartCapture(100);

        }

        public void RequestMp3()
        {
            audioGraph.Mp3Requested();
            audioGraph.OnMp3Encode += audioGraph_OnMp3Encode;
        }

        public void RequestPCM()
        {
            audioGraph.PCMRequest();
            audioGraph.OnPCMBuffer += audioGraph_OnPCMBuffer;

        }

        void audioGraph_OnPCMBuffer(object sender, PCMBufferEventArgs e)
        {
            callback.SendEncodedPCM(e);
        }

        void audioGraph_OnMp3Encode(object sender, Mp3EncodedeEventArgs e)
        {
            callback.SendEncodedMP3(e);
        }

        void audioGraph_FftCalculated(object sender, FftEventArgs e)
        {
            callback.SendFTTArgs(e);
        }

        void audioGraph_MaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            callback.SendMaxSapleCalculaterd(e);
        }

        void audioGraph_CaptureComplete(object sender, EventArgs e)
        {
            ///Implenet Catching CompletedLog
        }
 
        public void GetDetails()
        {
            audioGraph = new AudioGraph();
            audioGraph.CaptureComplete += new EventHandler(audioGraph_CaptureComplete);
            audioGraph.MaximumCalculated += new EventHandler<MaxSampleEventArgs>(audioGraph_MaximumCalculated);
            audioGraph.FftCalculated += new EventHandler<FftEventArgs>(audioGraph_FftCalculated);
            callback = OperationContext.Current.GetCallbackChannel<IProviderCallback>();
            this.Capture();
        }
       
        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
