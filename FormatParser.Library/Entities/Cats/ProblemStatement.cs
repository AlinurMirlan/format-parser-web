namespace FormatParser.Library.Entities.Cats;

public class ProblemStatement
{
    public string? Attachment { get; set; }
    public string? Url { get; set; }
    public string? ContentValue { get; set; }

    [EntityName("Quiz")]
    public Quiz? Quiz { get; set; }
}