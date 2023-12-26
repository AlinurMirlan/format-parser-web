namespace FormatParser.Library.Entities.Pcms2;

public class Assets
{
    public Checker? Checker { get; set; }

    [ListWrapper]
    public List<Validator>? Validators { get; set; }

    [ListWrapper]
    public List<Solution>? Solutions { get; set; }
}
