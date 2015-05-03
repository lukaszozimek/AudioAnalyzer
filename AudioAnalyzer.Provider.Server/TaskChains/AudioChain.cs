using AudioAnalyzer.Common.Audio.AudioInterface.Model;
using NAudio.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioAnalyzer.Provider.Server.Tasks
{
    public class AudioTaskChain : TaskChain
    {


        public AudioTaskChain(Configuration.Configuration configuration)
            :base(configuration)
        {
        }

        public override void RequestHandler()
        {
            foreach (var meterSend in Config.GetMeters())
            {
                meterSend.LeftCHannelLevel = (int)(Decibels.LinearToDecibels(meterSend.Device.AudioMeterInformation.PeakValues[0]));
                meterSend.RightChannelLevel = (int)(Decibels.LinearToDecibels(meterSend.Device.AudioMeterInformation.PeakValues[1]));
                meterSend.Error = CheckMeter(meterSend, -10);
               
                Console.WriteLine(meterSend.LeftCHannelLevel + " " + meterSend.RightChannelLevel);
            }
            Successor.RequestHandler();
        
        }

        private bool CheckMeter(Meter meter, int criticalLevel)
        {
            if ((meter.LeftCHannelLevel <= criticalLevel))
            {
                return true;
            }
            else if ((meter.RightChannelLevel <= criticalLevel))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
