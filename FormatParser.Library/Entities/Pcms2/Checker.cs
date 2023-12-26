namespace FormatParser.Library.Entities.Pcms2;

public class Checker
{
    public string? Name { get; set; }
    public string? Type { get; set; }
    public Source? Source { get; set; }
    public Source? Binary { get; set; }
    public Copy? Copy { get; set; }

    [EntityName("testset")]
    public TestSet? TestSet { get; set; }
}
