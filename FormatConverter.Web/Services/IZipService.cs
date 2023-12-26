

namespace FormatConverter.Web.Services;

public interface IZipService
{
    public void CreateZipArchive(string zipFilePath, Action<Stream> streamAction);
    public void ExtractZipArchive(Stream zipFileStream, string extractionDirectory);
}