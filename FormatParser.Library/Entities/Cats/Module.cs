namespace FormatParser.Library.Entities.Cats;

public class Module
{
    public string? Name { get; set; }
    public string? Src { get; set; }
    public string? FileName { get; set; }

    [EntityName("de_code")]
    public string? DeCode { get; set; }
    public string? Type { get; set; }
    public string? Export { get; set; }
    public string? Main { get; set; }
}