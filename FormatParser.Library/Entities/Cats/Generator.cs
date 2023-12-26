namespace FormatParser.Library.Entities.Cats;

public class Generator
{
    public string? Name { get; set; }
    public string? Src { get; set; }

    [EntityName("de_code")]
    public string? DeCode { get; set; }
    public string? OutputFile { get; set; }
    public string? Export { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public int WriteLimit { get; set; } = 999;
}