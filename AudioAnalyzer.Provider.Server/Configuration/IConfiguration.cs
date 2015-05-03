using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioAnalyzer.Provider.Server.Configuration
{
   public interface IConfiguration
    {
       void PrepareConfiguration();
       void Clean();

    }
}
