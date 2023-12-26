using FormatParser.Library.Entities.Ejudge;

namespace FormatParser.Library.Readers;

public class EjudgeFormatReader : FormatReader<EjudgeInfo>
{
    public EjudgeFormatReader(ParseFile<EjudgeProblem> parseProblem, ParseFile<Valuer> parseValuer)
    {
        ParseProblem = parseProblem;
        ParseValuer = parseValuer;
    }

    public ParseFile<EjudgeProblem> ParseProblem { get; set; }
    public ParseFile<Valuer> ParseValuer { get; set; }

    public override EjudgeInfo Parse(DirectoryInfo formatDirectory)
    {
        EjudgeInfo ejudgeInfo = new(
            Parse(formatDirectory, ParseProblem),
            Parse(formatDirectory, ParseValuer));
        return ejudgeInfo;
    }
}
