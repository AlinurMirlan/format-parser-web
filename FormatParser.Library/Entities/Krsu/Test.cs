using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace FormatParser.Library.Entities.Krsu;

public class Test
{
    [EntityName("input")]
    [XmlAttribute("input")]
    public string? InputFile { get; set; }

    [EntityName("output")]
    [XmlAttribute("output")]
    public string? OutputFile { get; set; }

    [EntityName("groupid")]
    [XmlAttribute("groupid")]
    public int GroupId { get; set; }

    [XmlAttribute("points")]
    public int Points { get; set; }
}
