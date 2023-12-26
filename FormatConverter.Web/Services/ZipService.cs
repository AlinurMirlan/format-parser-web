using System.IO.Compression;

namespace FormatConverter.Web.Services;

public class ZipService : IZipService
{
    private readonly string _conversionFileName;

    public ZipService(IConfiguration configuration)
    {
        var key = "Files:TargetConversionFileName";
        _conversionFileName = configuration[key] ?? throw new ArgumentNullException(key, $"Configuration key is absent.");
    }

    public void ExtractZipArchive(Stream zipFileStream, string extractionDirectory)
    {
        using (var zipArchive = new ZipArchive(zipFileStream))
        {
            zipArchive.ExtractToDirectory(extractionDirectory, true);
        }
    }

    public void CreateZipArchive(string zipFilePath, Action<Stream> streamAction)
    {
        using (var zipStream = new MemoryStream())
        {
            using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
            {
                var entry = archive.CreateEntry(_conversionFileName);
                using (var entryStream = entry.Open())
                {
                    streamAction(entryStream);
                }
            }

            File.WriteAllBytes(zipFilePath, zipStream.ToArray());
        }
    }
}
