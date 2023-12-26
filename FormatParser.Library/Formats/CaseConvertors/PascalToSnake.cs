namespace FormatParser.Library.Formats.CaseConvertors;

public class PascalToSnake : PascalToLowerCase
{
    protected override string Separator { get; set; } = "_";
}
