using FormatParser.Library.Entities.Krsu;

namespace FormatParser.Library.Readers;

public class KrsuFormatReader : FormatReader<TestInfo>
{
    public KrsuFormatReader(ParseFile<TestInfo> parseTest)
    {
        ParseTest = parseTest;
    }

    public ParseFile<TestInfo> ParseTest { get; set; }

    public override TestInfo Parse(DirectoryInfo formatDirectory) => Parse(formatDirectory, ParseTest);
}
