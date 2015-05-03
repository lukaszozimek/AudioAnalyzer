using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Net;
using System.Net.Sockets;
namespace AudioAnalyzer.Helpers
{
    public class NetworkHelpers
    {
        private TcpClient _pinger;

        private bool _isAlive = false;

        private string _ipAdress;
        private int _port;

        public NetworkHelpers(string ipAdress, int port)
        {
            this._port = port;
            this._ipAdress = ipAdress;
        }
        public NetworkHelpers() { }
        public bool CheckHostAlive()
        {
            try
            {
                _pinger = new TcpClient();
                _pinger.Connect(this._ipAdress, _port);
                _isAlive = true;
                _pinger.Close();
                return _isAlive;

            }
            catch (Exception)
            {
                _pinger.Close();
                _isAlive = false;
                return _isAlive;
            }
            finally
            {
                _pinger = null;
            }
        }
        public static bool CheckHostAlive( string _ipAdress ,int _port )
        {
            bool _isAlive;
            TcpClient _staticPinger = new TcpClient();
            try
            {
             
                _staticPinger.Connect(_ipAdress, _port);
                _isAlive = true;
                _staticPinger.Close();
                _staticPinger = null;
                return _isAlive;

            }
            catch (Exception)
            {
                _staticPinger.Close();
                _isAlive = false;
                _staticPinger = null;
                return _isAlive;
            }
       
        }
        public static string ResolveIpAdress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
    }
}
