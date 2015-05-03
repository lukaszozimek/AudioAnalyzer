using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioAnalyzer.Users
{
    [Serializable()]
   public abstract class User
    {
        public int ID { get; set; }
        public string IpAdress { get; set; }
    }
}
