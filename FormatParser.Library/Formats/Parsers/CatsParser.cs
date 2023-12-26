using FormatParser.Library.Formats.CaseConvertors;
using FormatParser.Library.Formats.Grammar;
using Sprache;

namespace FormatParser.Library.Formats.Parsers;

public class CatsParser<T> : FormatParser<T> where T : class
{
    protected override ICaseConvertor? CaseConvertor { get; set; } = new PascalToCamel();

    protected override Entity ExtractEntity(Stream fileStream)
    {
        using StreamReader streamReader = new(fileStream);
        return new CatsGrammar().Parser.Parse(streamReader.ReadToEnd());
    }
}
