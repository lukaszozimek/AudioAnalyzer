
using AudioAnalyzer.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace AudioAnalyzer.Client.Configuration
{
    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class AudioAnalyzerClientConfiguration 
    {
       static string appPath = Directory.GetCurrentDirectory();


        private AudioAnalyzerClientDistributorAdress[] distributorAdressField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("DistributorAdress")]
        public AudioAnalyzerClientDistributorAdress[] DistributorAdress
        {
            get
            {
                return this.distributorAdressField;
            }
            set
            {
                this.distributorAdressField = value;
            }
        }

        public static void SerializeConfiguration(AudioAnalyzerClientConfiguration item)
        {
            try
            {
                
            XmlSerializer serializer
                                      = new XmlSerializer(typeof(AudioAnalyzerClientConfiguration));
            using (TextWriter writer
                             = new StreamWriter(appPath + "\\config.xml"))
            {
                serializer.Serialize(writer, item);
            }
            }
            catch (Exception ex)
            {
                Logger.Instance.ServiceClientDebug(ex);   
                throw;
            }
        }

        public static AudioAnalyzerClientConfiguration DeserializationConfiguration(AudioAnalyzerClientConfiguration item)
        {
            try
            {

                XmlSerializer deserializer
                                    = new XmlSerializer(typeof(AudioAnalyzerClientConfiguration));
                using (TextReader reader
                     = new StreamReader(appPath + "\\config.xml"))
                {
                    item
                        = (AudioAnalyzerClientConfiguration)deserializer.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.ServiceClientDebug(ex);
                throw;
            }
            return item;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AudioAnalyzerClientDistributorAdress
    {

        private string ipAdressField;

        private ushort communicationPortField;

        private byte monitorIDField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string IpAdress
        {
            get
            {
                return this.ipAdressField;
            }
            set
            {
                this.ipAdressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort CommunicationPort
        {
            get
            {
                return this.communicationPortField;
            }
            set
            {
                this.communicationPortField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte MonitorID
        {
            get
            {
                return this.monitorIDField;
            }
            set
            {
                this.monitorIDField = value;
            }
        }
    }


}
