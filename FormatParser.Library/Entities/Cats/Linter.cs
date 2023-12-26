namespace FormatParser.Library.Entities.Cats;

public class Linter
{
    public string? Name { get; set; }
    public string? Src { get; set; }
    public string? Stage { get; set; }

    [EntityName("de_code")]
    public string? DeCode { get; set; }
    public string? Export { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public int WriteLimit { get; set; }
}
