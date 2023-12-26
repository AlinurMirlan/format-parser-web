namespace FormatConverter.Web.Models;

public class ConverterVm
{
    public required string ConvertFrom { get; set; }
    public required string ConvertTo { get; set; }
    public string? ConverterClassName { get; set; }
}
