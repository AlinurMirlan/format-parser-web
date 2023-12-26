using Sprache;

namespace FormatParser.Library.Formats.Grammar;

public static class CommonParse
{
    public static readonly Parser<Item> Decimal = Parse.DecimalInvariant.Select(
    decimalString => new Property(double.Parse(decimalString)));

    public static readonly Parser<Item> Number = Parse.Number.Select(number => new Property(int.Parse(number)));

    public static Parser<string> SingleLineComment(string commentStarter) =>
        from start in Parse.String(commentStarter).Token()
        from content in Parse.CharExcept(new char[] { '\n', '\r' }).Many().Text()
        from lineTerminator in Parse.String("\r\n").Or(Parse.String("\r").Or(Parse.String("\n")))
        select content;

    public static Parser<string> MultilineComment(string commentStarter, string commentTerminator) =>
        from start in Parse.String(commentStarter).Token()
        from content in Parse.AnyChar.Until(Parse.String(commentTerminator)).Text()
        select content;
}
