using FormatParser.Library.Formats.CaseConvertors;
using FormatParser.Library.Formats.Grammar;
using Sprache;

namespace FormatParser.Library.Formats.Parsers.Ejudge;

public class ProblemParser<TEntity> : FormatParser<TEntity> where TEntity : class
{
    protected override ICaseConvertor? CaseConvertor { get; set; } = new PascalToSnake();

    protected override Entity ExtractEntity(Stream fileStream)
    {
        using StreamReader streamReader = new(fileStream);
        return ProblemGrammar.Parser.Parse(streamReader.ReadToEnd());
    }
}
