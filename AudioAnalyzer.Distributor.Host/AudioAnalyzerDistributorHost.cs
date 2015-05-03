using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using AudioAnalyzer.Common.Helpers;
namespace AudioAnalyzer.Distributor.Host
{
   public class AudioAnalyzerDistributorHost : ServiceBase
    {
        const string CONSOLE = "-console";
        public const string NAME = "AudioAnalyzerDistributor";
        const string INSTALL = "-install";
        
       ServiceHost _serviceHost = null;
       private AudioAnalyzer.Distributor.Server.Distributor _service = null;
     
       public AudioAnalyzerDistributorHost()
         {
            ServiceName = NAME; 
         }


       public static void Main(string[] args)
       {

#if DEBUG
           new AudioAnalyzerDistributorHost().ConsoleRun();

#else
           if (args.Length == 1 && args[0].Equals(CONSOLE))
           {
               new AudioAnalyzerDistributorHost().ConsoleRun();
           }
           else if (args.Length == 1 && args[0].Equals(INSTALL))
           {
             
               ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });
           
           
           }
           else
           {
               ServiceBase.Run(new AudioAnalyzerDistributorHost());
           }
#endif
       }
       private void ConsoleRun()
        {
            Console.WriteLine(string.Format("{0}::starting...", GetType().FullName));

            OnStart(null);

            Console.WriteLine(string.Format("{0}::ready (ENTER to exit)", GetType().FullName));
            Console.ReadLine();

            OnStop();

            Console.WriteLine(string.Format("{0}::stopped", GetType().FullName));
        }

        protected override void OnStart(string[] args)
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
            }
            _serviceHost = CustomServiceHost.CreateCustomServiceHost(_serviceHost, typeof(Server.Distributor), "http://localhost:30200/", "net.tcp://localhost:30201/", "Catch", typeof(Server.IDistributor));

            _serviceHost.Open();
        }
        protected override void OnStop()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
            }
        }

    }

    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        const string DESCRIPTION = "Distributor for Audio monitoring system";
        const string DISPLAY_NAME = "AudioAnalyzerDistributor";

        private ServiceProcessInstaller _process;

        private ServiceInstaller _service;

        public ProjectInstaller()
        {
            _process = new ServiceProcessInstaller();
            _process.Account = ServiceAccount.LocalSystem;
            _service = new ServiceInstaller();
            _service.ServiceName = AudioAnalyzerDistributorHost.NAME;
            _service.Description = DESCRIPTION;
            _service.DisplayName = DISPLAY_NAME;
            _service.StartType = ServiceStartMode.Automatic;
            Installers.Add(_process);
            Installers.Add(_service);
            Installer installer = new Installer();
         
        }
        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }
    }
}
