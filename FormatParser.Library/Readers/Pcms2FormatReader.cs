using FormatParser.Library.Entities.Pcms2;

namespace FormatParser.Library.Readers;

public class Pcms2FormatReader : FormatReader<Pcms2Problem>
{
    public Pcms2FormatReader(ParseFile<Pcms2Problem> parseProblem)
    {
        ParseProblem = parseProblem;
    }

    public ParseFile<Pcms2Problem> ParseProblem { get; set; }

    public override Pcms2Problem Parse(DirectoryInfo formatDirectory) => Parse(formatDirectory, ParseProblem);
}
