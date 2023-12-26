namespace FormatParser.Library.Formats.CaseConvertors;

public class PascalToKebab : PascalToLowerCase
{
    protected override string Separator { get; set; } = "-";
}
