namespace FormatParser.Library.Entities.Cats;

public class TestSet
{
    public string? Name { get; set; }
    public string? Tests { get; set; }
    public int Points { get; set; }
    public string? HideDetails { get; set; }
    public string? Comment { get; set; }

    [EntityName("depends_on")]
    public string? DependsOn { get; set; }
}