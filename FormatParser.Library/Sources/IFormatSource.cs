namespace FormatParser.Library.Sources;

public interface IFormatSource
{
    public DirectoryInfo Open(string filePath);
}
