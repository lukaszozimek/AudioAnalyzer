using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AudioAnalyzer.Provider.Server.Tasks
{
   public abstract class TaskChain
    {
       public Configuration.Configuration Config { get; protected set; }
       private TaskChain successor;

       protected TaskChain Successor
       {
           get { return successor; }
           set { successor = value; }
       }
      
       protected TaskChain(Configuration.Configuration config)
       {
           this.Config = config;
       
       }
       
       public void SetSuccesor(TaskChain successor)
       {
           this.Successor = successor;
       }
       
       public abstract void RequestHandler();
    }
}
