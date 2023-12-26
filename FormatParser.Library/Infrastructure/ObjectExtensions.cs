using System.Collections;
using System.Reflection;
using System.Text;

namespace FormatParser.Library.Infrastructure;

public static class ObjectExtensions
{
    private static string Stringify(object entity, PropertyInfo propertyInfo, int depth)
    {
        Type propertyType = propertyInfo.PropertyType;
        int depthAddition = 3;
        int nextDepth = depth + depthAddition;
        object? propertyValue = propertyInfo.GetValue(entity);

        if (propertyType.IsList() && propertyValue is IList propertyList)
            return StringifyList(propertyInfo, propertyList, nextDepth, nextDepth + depthAddition);
        else if (propertyType.IsEntity())
        {
            if (propertyValue is null)
                return StringifyText(propertyInfo.Name, null, nextDepth);
            else
                return propertyValue.StringifyEntity(nextDepth, propertyInfo);
        }

        return StringifyProperty(propertyInfo, propertyValue, nextDepth);
    }

    private static string StringifyProperty(PropertyInfo propertyInfo, object? propertyValue, int depth)
    {
        return StringifyText(propertyInfo.Name, propertyValue?.ToString(), depth);
    }

    private static string StringifyText(string name, string? value, int depth)
    {
        return $"{Enumerable.Range(1, depth).Aggregate("", (space, _) => space + " ")}" +
            $"{name} := {value?.ToString()}\n";
    }

    public static string StringifyEntity(this object entity, int depth, PropertyInfo? entityProperty = null)
    {
        Type entityType = entity.GetType();
        string name = entityProperty is null ? entityType.Name : entityProperty.Name;
        StringBuilder stringBuilder = new(StringifyText(name, null, depth));
        foreach (PropertyInfo propertyInfo in entityType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            stringBuilder.Append(Stringify(entity, propertyInfo, depth));
        }

        return stringBuilder.ToString();
    }

    public static string StringifyList(PropertyInfo propertyInfo, IList propertyList, int depth, int nextDepth)
    {
        StringBuilder listBuilder = new(StringifyProperty(propertyInfo, null, depth));
        Type propertyType = propertyInfo.PropertyType;
        if (propertyType.GenericTypeArguments.First().IsEntity())
            foreach (object listEntity in propertyList)
                listBuilder.Append(listEntity.StringifyEntity(nextDepth) + "\n");
        else
            foreach (object listEntity in propertyList)
                listBuilder.Append(StringifyText(string.Empty, listEntity.ToString(), nextDepth) + "\n");

        return listBuilder.ToString();
    }
}
