using Sprache;

namespace FormatParser.Library.Formats.Grammar;

public static class ProblemGrammar
{
    static readonly Parser<string> Comment =
        CommonParse.SingleLineComment("#").Or(CommonParse.MultilineComment("[", "]"));

    public static readonly Parser<Item> SuffxiedNumber =
        from number in Parse.Number
        from suffix in Parse.IgnoreCase("m")
        select new Property(int.Parse(number));

    static readonly Parser<Item> QuotedString =
        from openQuote in Parse.Char('"')
        from content in Parse.CharExcept('"').Many().Text()
        from closeQuote in Parse.Char('"')
        select new Property(content);

    static readonly Parser<string> Identifier =
        from first in Parse.Letter.Or(Parse.Char('_')).Once().Text()
        from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Many().Text()
        select first + rest;

    static readonly Parser<KeyValuePair<string, Item>> IdentifierValue =
        from leading in Comment.Many()
        from identifier in Identifier.Token()
        from equal in Parse.Char('=').Token()
        from value in SuffxiedNumber.Or(CommonParse.Decimal).Or(CommonParse.Number)
                    .Or(QuotedString)
                    .Token()
        from trailing in Comment.Many()
        select new KeyValuePair<string, Item>(identifier, value);

    public static readonly Parser<Entity> Parser =
        from leading in Parse.WhiteSpace.Many()
        from identifierValuePairs in IdentifierValue.Many()
        select new Entity(identifierValuePairs);
}
