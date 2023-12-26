namespace FormatParser.Library.Entities.Cats;

public class Visualizer
{
    public string? Name { get; set; }
    public string? Src { get; set; }

    [EntityName("de_code")]
    public string? DeCode { get; set; }
    public string? Export { get; set; }
}
