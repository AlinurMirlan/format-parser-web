using FormatParser.Library.Entities;
using FormatParser.Library.Formats.CaseConvertors;
using FormatParser.Library.Infrastructure;
using System.Collections;
using System.Reflection;

namespace FormatParser.Library.Formats.EntityBuilders;

public class EntityBuilder : IEntityBuilder
{
    private readonly ICaseConvertor? _caseConvertor;
    private readonly string _contentPropertyName;

    public EntityBuilder(ICaseConvertor? caseConvertor, string contentPropertyName = "ContentValue")
    {
        _caseConvertor = caseConvertor;
        _contentPropertyName = contentPropertyName;
    }

    public object BuildEntity(Type entityType, Entity entity)
    {
        object entityInstance = Activator.CreateInstance(entityType)
            ?? throw new InvalidOperationException("Could not isntantiate an entity class.");
        foreach (PropertyInfo propertyInfo in entityType.GetProperties())
        {
            Type propertyType = propertyInfo.PropertyType;
            Attribute? attribute = propertyInfo.GetCustomAttribute<EntityNameAttribute>();
            string name =
                attribute is EntityNameAttribute entityNameAttribute
                ? entityNameAttribute.Name
                : _caseConvertor?.Convert(propertyInfo.Name) ?? propertyInfo.Name;

            if (entity.Properties.TryGetValue(name, out Item? property))
            {
                object? propertyInstance = Build(propertyType, property, propertyInfo);
                propertyInfo.SetValue(entityInstance, propertyInstance);
            }
        }

        return entityInstance;
    }

    private object BuildContentProperty(Type entityProperty, Property property)
    {
        object entity = Activator.CreateInstance(entityProperty) 
            ?? throw new InvalidOperationException("Could not isntantiate an entity.");

        PropertyInfo contentProperty = entityProperty.GetProperty(_contentPropertyName) ??
            throw new InvalidOperationException($"No content property named '{_contentPropertyName}' defined");

        Type contentType = contentProperty.PropertyType;
        object? propertyValue = property.Value;
        if (propertyValue is not null)
        {
            if (!contentType.IsAssignableFrom(propertyValue.GetType()))
                propertyValue = Convert.ChangeType(propertyValue, contentType);
        }

        contentProperty.SetValue(entity, propertyValue);
        return entity;
    }

    private static object BuildProperty(Type entityProperty, Property property)
    {
        object? propertyValue = property.Value;
        if (entityProperty.IsAssignableFrom(propertyValue!.GetType()))
            return propertyValue;

        return Convert.ChangeType(propertyValue, entityProperty);
    }

    private object BuildEntityList(Type listType, Entity listEntity, PropertyInfo propertyInfo)
    {
        ListWrapperAttribute? listWrapper = propertyInfo.GetCustomAttribute<ListWrapperAttribute>();
        if (listWrapper is not null)
        {
            if (listEntity.Properties.Count > 1)
                throw new InvalidFormatException("Invalid wrapper list format.");

            Item item = listEntity.Properties.Values.First();
            Property wrapperListProperty = item switch
            {
                Entity => new Property(item),
                Property property => property,
                _ => throw new InvalidOperationException("Unknown type.")
            };
            return BuildList(listType, wrapperListProperty);
        }

        Property listProperty = new(listEntity);
        return BuildList(listType, listProperty);
    }

    private object BuildList(Type listType, Property listProperty)
    {
        Type genericTypeArgument = listType.GenericTypeArguments.First();
        IList resultingList = (IList)(Activator.CreateInstance(listType)
            ?? throw new InvalidOperationException("Could not isntantiate a list."));

        object? propertyValue = listProperty.Value;
        if (genericTypeArgument.IsEntity())
        {
            if (propertyValue is IList list)
                foreach (object entity in list)
                    resultingList.Add(BuildEntity(genericTypeArgument, (Entity)entity));
            else
                resultingList.Add(BuildEntity(genericTypeArgument, (Entity)propertyValue!));
        }
        else
        {
            if (propertyValue is IList list)
                foreach (object property in list)
                    resultingList.Add(BuildProperty(genericTypeArgument, (Property)property));
            else
                resultingList.Add(BuildProperty(genericTypeArgument, listProperty));
        }

        return resultingList;
    }

    private object? Build(Type type, Item? item, PropertyInfo propertyInfo)
    {
        return item switch
        {
            Entity entity when type.IsEntity() => BuildEntity(type, entity),
            Entity entity when type.IsList() => BuildEntityList(type, entity, propertyInfo),
            Property property when property.Value is null => null,
            Property property when type.IsList() => BuildList(type, property),
            Property property when type.IsEntity() => BuildContentProperty(type, property),
            Property property => BuildProperty(type, property),
            _ => throw new InvalidOperationException("Unknown type.")
        };
    }
}
