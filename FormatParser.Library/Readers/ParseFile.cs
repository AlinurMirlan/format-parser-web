using FormatParser.Library.Formats.Parsers;

namespace FormatParser.Library.Readers;

public record ParseFile<T>(FormatParser<T> FileParser, string FileName) where T : class;
