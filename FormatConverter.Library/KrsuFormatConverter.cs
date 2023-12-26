using FormatParser.Library.Entities.Krsu;
using System.Xml.Serialization;

namespace FormatConverter.Library;

public abstract class KrsuFormatConverter : IFormatConverter
{
    private readonly XmlSerializer _xmlSerializer = new(typeof(TestInfo));

    public abstract string ConvertFrom { get; }
    public string ConvertTo => "KRSU";

    protected abstract TestInfo GetKrsuEntity(DirectoryInfo sourceFormatDirectory);
    public void Convert(DirectoryInfo sourceFormatDirectory, Stream targetFormatStream)
    {
        var krsuFormat = GetKrsuEntity(sourceFormatDirectory);
        _xmlSerializer.Serialize(targetFormatStream, krsuFormat);
    }
}
