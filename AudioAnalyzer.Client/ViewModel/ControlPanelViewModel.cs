using AudioAnalyzer.Client.Controls;
using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using AudioAnalyzer.Common.Audio.Helpers;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


namespace AudioAnalyzer.Client.ViewModel
{
    public class ControlPanelViewModel : INotifyPropertyChanged, IDisposable
    {
        int captureSeconds;
        AudioGraph audioGraph;
        IWaveFormRenderer waveFormRenderer;
        SpectrumAnalyser analyzer;
        InstanceContext context;
        WaveOutEvent player;

        AnalyzerProxyDetails.DetalisClient client;
        int i = 0;
        public delegate void SpectrumCallbackFftHandler(FftEventArgs fttData);
        public static event SpectrumCallbackFftHandler CatcherCallBackFftEvent;
        public delegate void SpectrumCallbackMaxSampleHandler(MaxSampleEventArgs maxSampleData);
        public static event SpectrumCallbackMaxSampleHandler CatcherCallBackMaxSampleEvent;
        public delegate void Mp3RequestHandler(Mp3EncodedeEventArgs mp3);
        public static event Mp3RequestHandler CatcherCallBackMp3RequestEvent;
        public delegate void PcmRequestHandler(PCMBufferEventArgs pcm);
        public static event PcmRequestHandler CatcherCallBackPcmRequestEvent;
        [CallbackBehaviorAttribute(UseSynchronizationContext = false)]
        public class SpectrumCallback : AnalyzerProxyDetails.IDetalisCallback
        {
          
            public void SendFTTArgs(FftEventArgs fttData)
            {
                Task.Run(() =>
                {
                    ControlPanelViewModel.CatcherCallBackFftEvent(fttData);
                });
            }

            public void SendMaxSapleCalculaterd(MaxSampleEventArgs maxSampleData)
            {
                Task.Run(() =>
                {
                    ControlPanelViewModel.CatcherCallBackMaxSampleEvent(maxSampleData);
                });
            }
            



            public void SendCatchingData(List<Meter> result)
            {
                throw new NotImplementedException();
            }

            public void SendEncodedMP3(Mp3EncodedeEventArgs _mp3Data)
            {
                throw new NotImplementedException();
            }

            public void SendEncodedPCM(PCMBufferEventArgs _pcmData)
            {
                throw new NotImplementedException();
            }

            public void SendCatchingData(Meter[] result)
            {
                throw new NotImplementedException();
            }
        }
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
     
        public ControlPanelViewModel(IWaveFormRenderer waveFormRenderer, SpectrumAnalyser analyzer)
        {
            this.waveFormRenderer = waveFormRenderer;
            this.analyzer = analyzer;
            this.context = new InstanceContext(new SpectrumCallback());
            this.client = new AnalyzerProxyDetails.DetalisClient(context);
            SpectrumCallbackFftHandler callbackFftHandler = new SpectrumCallbackFftHandler(fftCalculated);
            SpectrumCallbackMaxSampleHandler callbackMaxSampleHandler = new SpectrumCallbackMaxSampleHandler(onMaxSampleCalculated);
            Mp3RequestHandler callbackMp3Handler = new Mp3RequestHandler(onMp3Sample);
            PcmRequestHandler callbackPcmHandler = new PcmRequestHandler(onPcmBuffer);
            CatcherCallBackFftEvent += callbackFftHandler;
            CatcherCallBackMaxSampleEvent +=callbackMaxSampleHandler;
            CatcherCallBackMp3RequestEvent += callbackMp3Handler;
            CatcherCallBackPcmRequestEvent += callbackPcmHandler;
       
            
            this.captureSeconds = 5;
     
            this.client.Open();
         
            CaptureCommand = new RelayCommand(
                        () => this.Capture(),
                        () => true);
            StopCommand = new RelayCommand(
                        () => this.Stop(),
                        () => true);

            PlayFileCommand = new RelayCommand(
                        () => this.Play(),
                        () => true);
        }
        void fftCalculated(FftEventArgs fttData)
        {
            analyzer.Update(fttData.Result);
        }
        void onMaxSampleCalculated(MaxSampleEventArgs maxSampleData)
        {
                waveFormRenderer.AddValue(maxSampleData.MaxSample, maxSampleData.MinSample);
 
        }
        void onMp3Sample(Mp3EncodedeEventArgs mp3EventArgs)
        {
          
        }
        void onPcmBuffer(PCMBufferEventArgs pcmEventArgs)
        {
           BufferedWaveProvider _proiveder = new BufferedWaveProvider(player.OutputWaveFormat);
          _proiveder.AddSamples(pcmEventArgs._pcmBuffor,0,pcmEventArgs._pcmBuffor.Count());
           player.Init(_proiveder);
            if (player.PlaybackState == PlaybackState.Stopped)
	        {
		        player.Play();
        	}
        }
        public ICommand CaptureCommand { get; private set; }
        public ICommand StopCommand { get; private set; }
        public ICommand PlayFileCommand { get; private set; }
        private void Capture()
        {
           
            this.client.GetDetails();
        }
        private bool HasCapturedAudio()
        {
            return audioGraph.HasCapturedAudio;
        }
        private void Stop()
        {
            audioGraph.Stop();
        }
        private void Play()
        {
            this.client.RequestMp3();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChangedEvent(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public void Dispose()
        {
            audioGraph.Dispose();
        }
    }
}