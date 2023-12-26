using System.Xml.Serialization;

namespace FormatParser.Library.Entities.Krsu;

[EntityName("testinfo")]
[XmlRoot("testinfo")]
public class TestInfo
{
    [XmlElement("checker")]
    public string? Checker { get; set; }

    [XmlElement("interactor")]
    public string? Interactor { get; set; }

    [EntityName("problem")]
    [XmlElement("problem")]
    public string? ProblemStatement { get; set; }

    [EntityName("memorylimit")]
    [XmlElement("memorylimit")]
    public int MemoryLimitByte { get; set; }

    [EntityName("timelimit")]
    [XmlElement("timelimit")]
    public int TimeLimitMilli { get; set; }

    [EntityName("testversion")]
    [XmlElement("testversion")]
    public int? TestVersion { get; set; }

    [EntityName("runtype")]
    [XmlElement("runtype")]
    public int RunType { get; set; }

    [EntityName("group")]
    [XmlElement("group")]
    public List<TestGroup> Groups { get; set; } = new List<TestGroup>();

    [EntityName("test")]
    [XmlElement("test")]
    public List<Test> Tests { get; set; } = new List<Test>();
}
