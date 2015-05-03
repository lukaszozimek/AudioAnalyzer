
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AudioAnalyzer.Common.Audio.Helpers
{
    public class Mp3Encoder
    {
        public event EventHandler<Mp3EncodedeEventArgs> EncodedMP3;
        public event EventHandler<PCMBufferEventArgs> CrossPCM;

        
        public void SendPCM( byte[] pcmBuffer)
        {

            CrossPCM(this, new PCMBufferEventArgs(pcmBuffer));

        }
    }

[Serializable()]
    public class Mp3EncodedeEventArgs : EventArgs
    {
        public byte[] _encodedMP3;
        public Mp3EncodedeEventArgs(byte[] _encodedMp3)
        {
            _encodedMP3 = _encodedMp3;
        }

    }
   [Serializable()]
    public class PCMBufferEventArgs : EventArgs
    {

        public byte[] _pcmBuffor;
        public PCMBufferEventArgs(byte[] _pcmbuffor)
        {
            _pcmBuffor = _pcmbuffor;
        }
    }
}