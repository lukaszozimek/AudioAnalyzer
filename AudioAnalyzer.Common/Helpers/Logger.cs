using System;
using System.IO;
using System.Text;

namespace AudioAnalyzer.Helpers
{
    public class Logger
    {
        private static Logger _instance;
        private string _logFileDistributor = "\\AudioAnalyzerDistributorServerLog.txt";
        private string _logFileProvider = "\\AudioAnalyzerProviderServerLog.txt";
        private string _logFileClient = "\\AudioAnalyzerClientLog.txt";
        private string _logFileHost = "\\AudioAnalyzerHostLog.txt";
        private string _logFileAudioDll = "\\AudioAnalyzerAudioDllLog.txt";
        private readonly object _sync = new object();
        private Logger()
        {

        }
        public static Logger Instance
        {

            get
            {
                if (_instance == null)
                {

                    _instance = new Logger();


                }
                return _instance;
            }
        }

        public void WriteSimpleMessag(string error)
        {
            string appPath = Directory.GetCurrentDirectory();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(DateTime.Now).Append(" | ").Append(error).Append(".");


            using (var writer = new StreamWriter(appPath + "\\Log.txt", true))
            {
                lock (_sync)
                {
                    writer.WriteLine(sb.ToString());

                }
            }
        }
        public virtual void WriteDevException(Exception ex)
        {
            throw new NotImplementedException();
        }

        public void ServiceDistributorDebug(Exception ex)
        {
            string appPath = Directory.GetCurrentDirectory();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(DateTime.Now)
              .AppendLine()
              .AppendLine(" ============================================================ ")
              .AppendLine()
              .AppendLine(ex.ToString()).Append(".");


            using (var writer = new StreamWriter(appPath + _logFileDistributor, true))
            {
                lock (_sync)
                {
                    writer.WriteLine(sb.ToString());
                }
            }

        }

        public void ServiceProviderDebug(Exception ex)
        {
            string appPath = Directory.GetCurrentDirectory();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(DateTime.Now)
              .AppendLine()
              .AppendLine(" ============================================================ ")
              .AppendLine()
              .AppendLine(ex.ToString()).Append(".");

            using (var writer = new StreamWriter(appPath + _logFileProvider, true))
            {
                lock (_sync)
                {
                    writer.WriteLine(sb.ToString());
                }
            }

        }

        public void ServiceClientDebug(Exception ex)
        {
            string appPath = Directory.GetCurrentDirectory();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(DateTime.Now)
           .AppendLine()
           .AppendLine(" ============================================================ ")
           .AppendLine()
           .AppendLine(ex.ToString()).Append(".");


            using (var writer = new StreamWriter(appPath + _logFileClient, true))
            {
                lock (_sync)
                {
                    writer.WriteLine(sb.ToString());
                }
            }

        }


        public void ServiceHostDebug(Exception ex)
        {
            string appPath = Directory.GetCurrentDirectory();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(DateTime.Now)
              .AppendLine()
              .AppendLine(" ============================================================ ")
              .AppendLine()
              .AppendLine(ex.ToString()).Append(".");

            using (var writer = new StreamWriter(appPath + _logFileHost, true))
            {
                lock (_sync)
                {

                    writer.WriteLine(sb.ToString());

                }
            }
        }

        public void AudioDllDebug(Exception ex)
        {
            string appPath = Directory.GetCurrentDirectory();
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append(DateTime.Now)
              .AppendLine()
              .AppendLine(" ============================================================ ")
              .AppendLine()
              .AppendLine(ex.ToString()).Append(".");


            using (var writer = new StreamWriter(appPath + _logFileAudioDll, true))
            {
                lock (_sync)
                {
                    writer.WriteLine(sb.ToString());
                }
            }

        }
    }
}
