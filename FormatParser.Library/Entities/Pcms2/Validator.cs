namespace FormatParser.Library.Entities.Pcms2;

public class Validator
{
    public Source? Source { get; set; }
    public Source? Binary { get; set; }

    [EntityName("testset")]
    public TestSet? TestSet { get; set; }
}