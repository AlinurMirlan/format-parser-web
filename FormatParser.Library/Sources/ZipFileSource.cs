using System.IO.Compression;

namespace FormatParser.Library.Sources;

public class ZipFileSource : IFormatSource
{
    public ZipFileSource(string outputPath)
    {
        OutPath = outputPath;
    }

    public string OutPath { get; set; }

    public DirectoryInfo Open(string filePath)
    {
        using ZipArchive zipArchive = ZipFile.OpenRead(filePath);
        zipArchive.ExtractToDirectory(OutPath, true);
        return new DirectoryInfo(OutPath);
    }
}
