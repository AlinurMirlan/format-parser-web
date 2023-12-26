namespace FormatParser.Library.Entities.Pcms2;

public class Judging
{
    public string? CpuName { get; set; }
    public string? CpuSpeed { get; set; }
    public string? InputFile { get; set; }
    public string? OutputFile { get; set; }

    [EntityName("testset")]
    public TestSet? TestSet { get; set; }
}
