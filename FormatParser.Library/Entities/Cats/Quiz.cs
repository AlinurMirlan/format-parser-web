namespace FormatParser.Library.Entities.Cats;

public class Quiz
{
    [EntityName("Text")]
    public string? Text { get; set; }

    [EntityName("Answer")]
    public string? Answer { get; set; }

    [EntityName("Choice")]
    public Choice? Choice { get; set; }
}
