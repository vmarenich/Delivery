using System.Xml.Serialization;

namespace WebApplication1.DTO
{
    [XmlRoot("xml")]
    public class Api3Request
    {
        [XmlElement("source")]
        public string Source { get; set; }

        [XmlElement("destination")]
        public string Destination { get; set; }

        [XmlArray("packages")]
        [XmlArrayItem("package")]
        public double[] Packages { get; set; }
    }
}