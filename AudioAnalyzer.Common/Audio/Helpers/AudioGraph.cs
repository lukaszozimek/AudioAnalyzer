
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AudioAnalyzer.Common.Audio;

namespace AudioAnalyzer.Common.Audio.Helpers
{
  public  class AudioGraph : IDisposable
    {
        public AudioCapture Capture { get; set; }

        private SampleAggregator aggregator;
        private Mp3Encoder _encoder;

        public event EventHandler<Mp3EncodedeEventArgs> OnMp3Encode
        {
            add { _encoder.EncodedMP3 += value; }
            remove { _encoder.EncodedMP3 -= value; }
        }

        public event EventHandler<PCMBufferEventArgs> OnPCMBuffer {

            add { _encoder.CrossPCM += value; }
            remove { _encoder.CrossPCM -= value; }
        }

      public event EventHandler CaptureComplete
        {
            add { Capture.CaptureComplete += value; }
            remove { Capture.CaptureComplete -= value; }
        }
        public event EventHandler<MaxSampleEventArgs> MaximumCalculated
        {
            add { aggregator.MaximumCalculated += value; }
            remove { aggregator.MaximumCalculated -= value; }
        }
        public event EventHandler<FftEventArgs> FftCalculated
        {
            add { aggregator.FftCalculated += value; }
            remove { aggregator.FftCalculated -= value; }
        }
    
        public AudioGraph()
        {
            _encoder = new Mp3Encoder();
            Capture = new AudioCapture();

            Capture.OnSample += OnSample;
         
            Capture.OnPcmBuffer += Capture_OnPcmBuffer;
            aggregator = new SampleAggregator();
            aggregator.NotificationCount = 100;
            aggregator.PerformFFT = true;
        }

        void Capture_OnPcmBuffer(object sender, PCMBufferEventArgs e)
        {
            if (this.Capture.IsPcmStreamRequested)
            {
                _encoder.SendPCM(e._pcmBuffor);
            }
            else
            {

                
        
            }
        }
        void OnSample(object sender, SampleEventArgs e)
        {
            aggregator.Add(e.Left);
            aggregator.Add(e.Right);
        }
        public int NotificationsPerSecond
        {
            get { return aggregator.NotificationCount; }
            set { aggregator.NotificationCount = value; }
        }
        public double RecordVolume
        {
            get { return Capture.RecordVolume; }
            set { Capture.RecordVolume = value; }
        }
        public bool HasCapturedAudio { get; private set; }
        private void CancelCurrentOperation()
        {
            Capture.Stop();
        }
        public void Stop()
        {
            CancelCurrentOperation();
        }
        public void StartCapture(int captureSeconds)
        {
            aggregator.NotificationCount = 200;
            Capture.CaptureSeconds = captureSeconds;
            Capture.Capture(new WaveFormat(8000, 1));
        }
        public void Dispose()
        {
            if (Capture != null)
            {
                Capture.Dispose();
                Capture = null;
            }
       
        }
        public void Mp3Requested()
        {
            this.Capture.IsPcmStreamRequested = true;
        }
        public void PCMRequest()
        {
            this.Capture.IsPcmStreamRequested = true;
        }

    }
}
