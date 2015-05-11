using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace AudioAnalyzer.Provider.Server.Tasks
{
  public class NetworkTaskChain : TaskChain
    {


      public NetworkTaskChain(Configuration.Configuration config)
          :base (config)
      {
        

      }

        public override void RequestHandler()
        {
            try
            {
                if (this.Config.StationContainers != null)
                {
                    this.Config.PublishContext();
                    Thread.Sleep(100);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Trying reconnect");
                throw;
                
            }
        }
    }
}
