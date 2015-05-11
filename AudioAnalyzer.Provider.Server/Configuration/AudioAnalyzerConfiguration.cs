using System.IO;
using System.Xml.Serialization;
namespace AudioAnalyzer.Provider.Server.Configuration
{

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
[System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
public partial class AudioAnalyzerConfiguration
{
    static string appPath = Directory.GetCurrentDirectory();


    private AudioAnalyzerConfigurationMonitorID[] monitorIDField;

    private byte intervalField;

    private string serviceAdressField;

    private ushort servicePortField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("MonitorID")]
    public AudioAnalyzerConfigurationMonitorID[] MonitorID
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

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte Interval
    {
        get
        {
            return this.intervalField;
        }
        set
        {
            this.intervalField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string ServiceAdress
    {
        get
        {
            return this.serviceAdressField;
        }
        set
        {
            this.serviceAdressField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public ushort ServicePort
    {
        get
        {
            return this.servicePortField;
        }
        set
        {
            this.servicePortField = value;
        }
    }
            public static AudioAnalyzerConfiguration DeserializationConfiguration(AudioAnalyzerConfiguration item)
        {
            try
            {

                XmlSerializer deserializer
                                    = new XmlSerializer(typeof(AudioAnalyzerConfiguration));
                using (TextReader reader
                     = new StreamReader(appPath + "\\config.xml"))
                {
                    item
                        = (AudioAnalyzerConfiguration)deserializer.Deserialize(reader);
                }
            }
            catch (FileNotFoundException)
            {
            }
            return item;
        }
       public static void SerializeConfiguration(AudioAnalyzerConfiguration item)
        {
            XmlSerializer serializer
                                   = new XmlSerializer(typeof(AudioAnalyzerConfiguration));
            using (TextWriter writer
                             = new StreamWriter(appPath + "\\config.xml"))
            {
                serializer.Serialize(writer, item);
            }
        }

}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class AudioAnalyzerConfigurationMonitorID
{

    private AudioAnalyzerConfigurationMonitorIDStation[] stationField;

    private byte monitorIDField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("Station")]
    public AudioAnalyzerConfigurationMonitorIDStation[] Station
    {
        get
        {
            return this.stationField;
        }
        set
        {
            this.stationField = value;
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

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class AudioAnalyzerConfigurationMonitorIDStation
{

    private AudioAnalyzerConfigurationMonitorIDStationMeterID[] meterIDField;

    private string stationNameField;

    private byte stationIDField;

    /// <remarks/>
    [System.Xml.Serialization.XmlElementAttribute("MeterID")]
    public AudioAnalyzerConfigurationMonitorIDStationMeterID[] MeterID
    {
        get
        {
            return this.meterIDField;
        }
        set
        {
            this.meterIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string StationName
    {
        get
        {
            return this.stationNameField;
        }
        set
        {
            this.stationNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte StationID
    {
        get
        {
            return this.stationIDField;
        }
        set
        {
            this.stationIDField = value;
        }
    }
}

/// <remarks/>
[System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
public partial class AudioAnalyzerConfigurationMonitorIDStationMeterID
{

    private string sourceNameField;

    private bool isActiveField;

    private byte meterIDField;

    private string friendlyNameField;

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string SourceName
    {
        get
        {
            return this.sourceNameField;
        }
        set
        {
            this.sourceNameField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public bool IsActive
    {
        get
        {
            return this.isActiveField;
        }
        set
        {
            this.isActiveField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public byte MeterID
    {
        get
        {
            return this.meterIDField;
        }
        set
        {
            this.meterIDField = value;
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlAttributeAttribute()]
    public string FriendlyName
    {
        get
        {
            return this.friendlyNameField;
        }
        set
        {
            this.friendlyNameField = value;
        }
    }
}


 
}