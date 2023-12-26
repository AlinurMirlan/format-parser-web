namespace FormatParser.Library.Formats.EntityBuilders;

public interface IEntityBuilder
{
    public object BuildEntity(Type entityType, Entity entity);
}
