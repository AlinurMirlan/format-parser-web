using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace FormatParser.Library.Entities.Krsu;

public class TestGroup
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlAttribute("points")]
    public int Points { get; set; }

    [EntityName("prereq")]
    [XmlAttribute("prereq")]
    public string? Prerequisites { get; set; }
}
