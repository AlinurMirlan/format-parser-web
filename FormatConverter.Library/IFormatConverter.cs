namespace FormatConverter.Library;

public interface IFormatConverter
{
    public string ConvertFrom { get; }
    public string ConvertTo { get; }
    public void Convert(DirectoryInfo sourceFormatDirectory, Stream targetFormatStream);
}
