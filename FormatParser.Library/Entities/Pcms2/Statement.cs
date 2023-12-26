using System.ComponentModel;

namespace FormatParser.Library.Entities.Pcms2;

public class Statement
{
    public string? Charset { get; set; }
    public string? Language { get; set; }
    public string? Path { get; set; }
    public string? Type { get; set; }
}
