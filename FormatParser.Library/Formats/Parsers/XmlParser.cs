using FormatParser.Library.Formats.CaseConvertors;
using FormatParser.Library.Formats.Grammar;
using Sprache;

namespace FormatParser.Library.Formats.Parsers;

public class XmlParser<T> : FormatParser<T> where T : class
{
    protected override ICaseConvertor? CaseConvertor { get; set; } = new PascalToKebab();

    protected override Entity ExtractEntity(Stream fileStream)
    {
        using StreamReader streamReader = new(fileStream);
        return  new XmlGrammar().Parser.Parse(streamReader.ReadToEnd());
    }
}
