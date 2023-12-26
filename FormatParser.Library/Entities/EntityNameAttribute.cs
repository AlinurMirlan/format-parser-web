namespace FormatParser.Library.Entities;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
public class EntityNameAttribute : Attribute
{
    public EntityNameAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
}
