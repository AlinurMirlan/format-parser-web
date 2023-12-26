namespace FormatParser.Library.Entities.Ejudge;

public class EjudgeProblem
{
    public int UseStdin { get; set; }
    public int UseStdout { get; set; }
    public int UseCorr { get; set; }
    public int EnableTestlibMode { get; set; }
    public double TimeLimit { get; set; }
    public required string MaxVmSize { get; set; }
    public required string LongName { get; set; }
    public required string LongNameEn { get; set; }
    public required string InternalName { get; set; }
    public required string TestPat { get; set; }
    public required string CorrPat { get; set; }
    public required string CheckCmd { get; set; }
    public required string SolutionCmd { get; set; }

    [EntityName("extid")]
    public string? ExtId { get; set; }
    public int Revision { get; set; }
}
