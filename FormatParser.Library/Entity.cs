namespace FormatParser.Library;

/// <summary>
/// key-<see cref="Item"/> entity representation.
/// </summary>
public class Entity : Item
{
    public Entity(IEnumerable<KeyValuePair<string, Item>> properties)
    {
        Properties = new Dictionary<string, Item>(properties);
    }

    public Dictionary<string, Item> Properties { get; set; }
}
