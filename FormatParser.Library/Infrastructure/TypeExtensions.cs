using System.Collections;

namespace FormatParser.Library.Infrastructure;

public static class TypeExtensions
{
    public static bool IsList(this Type type) =>
        typeof(IList).IsAssignableFrom(type)
        && type.GetGenericArguments().Length == 1;

    public static bool IsEntity(this Type type) =>
        !type.IsPrimitive
        && type != typeof(string)
        && type != typeof(decimal)
        && type != typeof(Range)
        && !type.IsList();
}
