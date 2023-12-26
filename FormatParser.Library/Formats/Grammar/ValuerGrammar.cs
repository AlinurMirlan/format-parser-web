using Sprache;

namespace FormatParser.Library.Formats.Grammar;

public static class ValuerGrammar
{
    private const char StatementTerminator = ';';
    static readonly Parser<string> Comment =
        CommonParse.SingleLineComment("#");

    static readonly Parser<string> Identifier =
        from first in Parse.Letter.Or(Parse.Char('_')).Once().Text()
        from rest in Parse.LetterOrDigit.Or(Parse.Char('_')).Many().Text()
        select first + rest;

    static readonly Parser<Item> Range =
        from leftInclusive in Parse.Number
        from hyphen in Parse.Char('-')
        from rightExclusive in Parse.Number
        from terminator in Parse.Char(StatementTerminator)
        select new Property(new Range(
            new(int.Parse(leftInclusive)),
            new(int.Parse(rightExclusive))));

    static readonly Parser<Item> List =
        from numbers in Parse.Number.DelimitedBy(Parse.Char(','))
        from terminator in Parse.Char(StatementTerminator)
        select new Property(numbers.Select(int.Parse).ToList());

    static readonly Parser<Item> Number =
        from number in CommonParse.Number
        from terminator in Parse.Char(StatementTerminator)
        select number;

    static readonly Parser<KeyValuePair<string, Item>> IdentifierValue =
        from leading in Comment.Many()
        from identifier in Identifier.Token()
        from value in Range.Or(Number).Or(List)
        from trailing in Comment.Many()
        select new KeyValuePair<string, Item>(identifier, value);

    static readonly Parser<IEnumerable<KeyValuePair<string, Item>>> ObjectContent =
        from openBrackets in Parse.Char('{')
        from identifierValuePairs in IdentifierValue.Many()
        from closeBrackets in Parse.Char('}').Token()
        select identifierValuePairs;

    public static readonly Parser<KeyValuePair<string, Entity>> GroupObject =
        from identifier in Identifier.Token()
        from id in Parse.Number.Token()
        from identifierValuePairs in ObjectContent
        select new KeyValuePair<string, Entity>(identifier,
            new Entity(identifierValuePairs.Append(
                new KeyValuePair<string, Item>("id", new Property(int.Parse(id))))
            )
        );

    public static readonly Parser<KeyValuePair<string, Entity>> Object =
        from identifier in Identifier.Token()
        from identifierValuePairs in ObjectContent
        select new KeyValuePair<string, Entity>(identifier, new Entity(identifierValuePairs));

    static readonly Parser<KeyValuePair<string, Entity>> Item =
        from leading in Comment.Many()
        from item in GroupObject.Or(Object)
        from trailing in Comment.Many()
        select item;

    public static readonly Parser<Entity> Parser =
        from leading in Parse.WhiteSpace.Many()
        from items in Item.Many()
        select new Entity(items.GroupBy(identifierValue => identifierValue.Key)
                .Select(grouping => new KeyValuePair<string, Item>(grouping.Key,
                    grouping.Count() > 1
                    ? new Property(new List<Entity>(grouping
                        .Select(identifierValue => identifierValue.Value)))
                    : grouping.First().Value))
        );
}
