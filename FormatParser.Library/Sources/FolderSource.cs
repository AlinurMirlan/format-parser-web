namespace FormatParser.Library.Sources;

public class FolderSource : IFormatSource
{
    public DirectoryInfo Open(string filePath) => new DirectoryInfo(filePath);
}
