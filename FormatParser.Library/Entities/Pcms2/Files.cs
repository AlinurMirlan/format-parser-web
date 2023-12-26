namespace FormatParser.Library.Entities.Pcms2;

public class Files
{
    [ListWrapper]
    public List<Resource>? Resources { get; set; }

    [ListWrapper]
    public List<Executable>? Executables { get; set; }
}
