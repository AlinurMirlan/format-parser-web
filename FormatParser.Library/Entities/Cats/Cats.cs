namespace FormatParser.Library.Entities.Cats;

public class Cats
{
    public string? Version { get; set; }

    [EntityName("Problem")]
    public CatsProblem? Problem { get; set; }
}
