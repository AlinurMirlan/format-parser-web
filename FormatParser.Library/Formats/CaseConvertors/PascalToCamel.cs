namespace FormatParser.Library.Formats.CaseConvertors;

public class PascalToCamel : ICaseConvertor
{
    public string Convert(string identificator) => 
        $"{identificator[0..1].ToLowerInvariant()}{identificator[1..]}";
}
