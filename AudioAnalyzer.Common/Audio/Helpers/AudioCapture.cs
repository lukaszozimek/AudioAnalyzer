
using NAudio.Utils;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AudioAnalyzer.Common.Audio;

namespace AudioAnalyzer.Common.Audio.Helpers
{
  public  class AudioCapture : IDisposable
    {
        private WaveInEvent captureDevice;
        private MemoryStream recordedStream;
        public bool IsEncodingRequested { get; set; }
        public bool IsPcmStreamRequested { get; set; }
        private int maxCaptureBytes;
        public AudioCapture()
        {
            CaptureSeconds = 30;
            
        }
      

        public bool IsCapturing { get; set; }
        public int CaptureSeconds { get; set; }
        public WaveFormat WaveFormat { get; set; }
       public event EventHandler<SampleEventArgs> OnSample;
        public event EventHandler CaptureComplete;
        public event EventHandler<PCMBufferEventArgs> OnPcmBuffer;

        public void Capture(WaveFormat captureFormat)
        {
            if (IsCapturing)
            {
                throw new InvalidOperationException("Already Recording");
            }

            CreateCaptureStream(captureFormat);
            StartCapture(captureFormat);
        }
         
      private void StartCapture(WaveFormat captureFormat)
        {
            this.WaveFormat = captureFormat;
       
                EnsureDeviceIsCreated();
                captureDevice.WaveFormat = captureFormat;
                captureDevice.StartRecording();
            
                IsCapturing = true;
            
        }

        private void CreateCaptureStream(WaveFormat captureFormat)
        {
            int maxSeconds = CaptureSeconds == 0 ? 30 : CaptureSeconds;
            int captureBytes = maxSeconds * captureFormat.AverageBytesPerSecond;
            this.maxCaptureBytes = CaptureSeconds == 0 ? 0 : captureBytes;
            recordedStream = new MemoryStream(captureBytes + 50);
         
        }

        private void EnsureDeviceIsCreated()
        {
            if (captureDevice == null)
            {
                

                 captureDevice = new WaveInEvent();
                 captureDevice.DeviceNumber = 0;
                captureDevice.RecordingStopped +=  OnRecordingStopped;
                captureDevice.DataAvailable +=  OnDataAvailable;
            }
              
        }

        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            if (!IsCapturing)
            {
                return;
            }
            // first save the audio
            byte[] buffer = e.Buffer;
        
            // now report each sample if necessary
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) | buffer[index + 0]);
                float sample32 = sample / 32768f;
                if (OnSample != null)
                {
                    OnSample(this, new SampleEventArgs(sample32, 0));
                }
                // stop the recording if necessary
                if (maxCaptureBytes != 0 && recordedStream.Length >= maxCaptureBytes)
                {
                    Stop();
                }
                if (IsEncodingRequested)
                {
                    
                        OnPcmBuffer(this, new PCMBufferEventArgs(buffer));
               }
                if (IsPcmStreamRequested)
                {
                    OnPcmBuffer(this, new PCMBufferEventArgs(buffer));
                }
            }
            
        }
        

        public void CloseRecording()
        {
            if (captureDevice != null)
            {
                captureDevice.StopRecording();
            }

            
                recordedStream.Position = 0;
                RaiseCaptureStopped();
            }
        

        void OnRecordingStopped(object sender, EventArgs e)
        {
            IsCapturing = false;
            CloseRecording();
            captureDevice.Dispose();
            captureDevice = null;
        }

        public void Stop()
        {
            if (captureDevice != null)
            {
                captureDevice.StopRecording();
            }
        }

        private void RaiseCaptureStopped()
        {
            if (CaptureComplete != null)
            {
                CaptureComplete(this, EventArgs.Empty);
            }
        }

        public double RecordVolume { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            if (captureDevice != null)
            {
                captureDevice.Dispose();
                captureDevice = null;
            }
        }

        #endregion
    }
}
