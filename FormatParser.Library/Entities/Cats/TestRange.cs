namespace FormatParser.Library.Entities.Cats;

public class TestRange
{
    public int From { get; set; }
    public int To { get; set; }
    public int Points { get; set; }

    [EntityName("In")]
    public In? In { get; set; }

    [EntityName("Out")]
    public Out? Out { get; set; }
}