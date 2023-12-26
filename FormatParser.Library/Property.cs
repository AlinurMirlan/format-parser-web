namespace FormatParser.Library;

/// <summary>
/// Simple value property.
/// </summary>
public class Property : Item
{
    public Property(object? value)
    {
        Value = value;
    }

    public object? Value { get; set; }
}
