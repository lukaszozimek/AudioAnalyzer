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
using System.Reflection;
using AudioAnalyzer.Common.Helpers;

namespace AudioAnalyzer.Provider.Host
{
    public class AudioAnalyzerProviderHost : ServiceBase
    {

        const string CONSOLE = "-console";
        public const string NAME = "AudioAnalyzerProviderHost";
        const string INSTALL = "-install";
        ServiceHost _serviceHost = null;
        ServiceHost _secondServiceHost = null;
        private AudioAnalyzer.Provider.Server.DataProvider _service = null;
        private AudioAnalyzer.Provider.Server.AudioDetalis _service1 = null;

        public AudioAnalyzerProviderHost()
        {
            ServiceName = NAME;
        }
        public static void Main(string[] args)
        {
#if DEBUG
            new AudioAnalyzerProviderHost().ConsoleRun();

#else

            if (args.Length == 1 && args[0].Equals(CONSOLE))
            {
                new TimeSAAudioMonitorProviderHost().ConsoleRun();
            }
            else if (args.Length == 1 && args[0].Equals(INSTALL))
            {

                ManagedInstallerClass.InstallHelper(new[] { Assembly.GetExecutingAssembly().Location });


            }
            else
            {
                ServiceBase.Run(new TimeSAAudioMonitorProviderHost());
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
            _service = new AudioAnalyzer.Provider.Server.DataProvider();
            _service1 = new Server.AudioDetalis();

            _serviceHost = CustomServiceHost.CreateCustomServiceHost(_serviceHost,_service.GetType(), "http://localhost:8023/", "net.tcp://localhost:8024/", "Catch", typeof(AudioAnalyzer.Provider.Server.Contract.IDataProvider));
            _secondServiceHost = CustomServiceHost.CreateCustomServiceHost(_secondServiceHost,_service1.GetType(),"http://localhost:8025/","net.tcp://localhost:8026/","AudioDetails",typeof(AudioAnalyzer.Provider.Server.Contract.IDetalis));
            _serviceHost.Open();
            _secondServiceHost.Open();
            _service.StartApplication();
        }
        protected override void OnStop()
        {
            if (_serviceHost != null)
            {
                _serviceHost.Close();
                _serviceHost = null;
                _service = null;
            }
        }

    }
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        const string DESCRIPTION = "Data Provider for Audio monitoring system";
        const string DISPLAY_NAME = "AudioAnalyzerProvider";

        private ServiceProcessInstaller _process;

        private ServiceInstaller _service;

        public ProjectInstaller()
        {
            _process = new ServiceProcessInstaller();
            _process.Account = ServiceAccount.LocalSystem;
            _service = new ServiceInstaller();
            _service.ServiceName = AudioAnalyzerProviderHost.NAME;
            _service.Description = DESCRIPTION;
            _service.DisplayName = DISPLAY_NAME;
            _service.StartType = ServiceStartMode.Automatic;
            Installers.Add(_process);
            Installers.Add(_service);
        }
    }
}
