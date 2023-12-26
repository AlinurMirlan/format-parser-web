namespace FormatParser.Library.Entities.Cats;

public class Sample
{
    public string? Rank { get; set; }

    [EntityName("SampleIn")]
    public SampleText? SampleIn { get; set; }

    [EntityName("SampleOut")]
    public SampleText? SampleOut { get; set; }
}