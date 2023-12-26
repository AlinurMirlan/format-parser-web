namespace FormatParser.Library.Entities.Cats;

public class Test
{
    public string? Rank { get; set; }
    public int Points { get; set; }
    public string? Descr { get; set; }

    [EntityName("In")]
    public In? In { get; set; }

    [EntityName("Out")]
    public Out? Out { get; set; }
}