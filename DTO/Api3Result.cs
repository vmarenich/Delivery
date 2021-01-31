using System.Xml.Serialization;

namespace WebApplication1.DTO
{
    [XmlRoot("xml")]
    public class Api3Result
    {
        [XmlElement("quote")]
        public double Quote { get; set; }
    }
}