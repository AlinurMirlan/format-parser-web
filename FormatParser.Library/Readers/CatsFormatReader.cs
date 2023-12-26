using FormatParser.Library.Entities.Cats;

namespace FormatParser.Library.Readers;

public class CatsFormatReader : FormatReader<Cats>
{
    public CatsFormatReader(ParseFile<Cats> parseCats)
    {
        ParseCats = parseCats;
    }

    public ParseFile<Cats> ParseCats { get; set; }

    public override Cats Parse(DirectoryInfo formatDirectory) => Parse(formatDirectory, ParseCats);
}
