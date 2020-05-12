using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ReadEdgeCore.Models
{
    public class AudioModel
    {
        public string start { get; set; }
        public string end { get; set; }
        public string text { get; set; }
    }

    [XmlRoot(ElementName = "smil", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Smil
    {
        [XmlElement(ElementName = "body", Namespace = "http://www.w3.org/ns/SMIL")]
        public Body Body { get; set; }
        [XmlAttribute(AttributeName = "xmlns")]
        public string Xmlns { get; set; }
        [XmlAttribute(AttributeName = "version")]
        public string Version { get; set; }
        [XmlAttribute(AttributeName = "profile")]
        public string Profile { get; set; }
    }

    [XmlRoot(ElementName = "body", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Body
    {
        [XmlElement(ElementName = "par", Namespace = "http://www.w3.org/ns/SMIL")]
        public List<Par> Par { get; set; }
    }

    [XmlRoot(ElementName = "par", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Par
    {
        [XmlElement(ElementName = "text", Namespace = "http://www.w3.org/ns/SMIL")]
        public Text Text { get; set; }
        [XmlElement(ElementName = "audio", Namespace = "http://www.w3.org/ns/SMIL")]
        public Audio Audio { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; }
    }

    [XmlRoot(ElementName = "text", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Text
    {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
    }

    [XmlRoot(ElementName = "audio", Namespace = "http://www.w3.org/ns/SMIL")]
    public class Audio
    {
        [XmlAttribute(AttributeName = "src")]
        public string Src { get; set; }
        [XmlAttribute(AttributeName = "clipBegin")]
        public string ClipBegin { get; set; }
        [XmlAttribute(AttributeName = "clipEnd")]
        public string ClipEnd { get; set; }
    }
}
