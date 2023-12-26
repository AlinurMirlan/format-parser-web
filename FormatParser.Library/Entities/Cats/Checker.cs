namespace FormatParser.Library.Entities.Cats;

public class Checker
{
    public string? Name { get; set; }
    public string? Src { get; set; }

    [EntityName("de_code")]
    public string? DeCode { get; set; }
    public string? Style { get; set; }
    public string? Export { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public int WriteLimit { get; set; }
}