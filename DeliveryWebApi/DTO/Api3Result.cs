using System.Xml.Serialization;

namespace DeliveryWebApi.DTO
{
    [XmlRoot("xml")]
    public class Api3Result
    {
        [XmlElement("quote")]
        public double Quote { get; set; }
    }
}