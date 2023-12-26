namespace FormatParser.Library.Entities.Ejudge;

public class Valuer
{
    public Global? Global { get; set; }

    [EntityName("group")]
    public List<Group> Groups { get; set; } = new List<Group>();
}
