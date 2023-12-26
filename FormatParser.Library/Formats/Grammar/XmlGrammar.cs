using Sprache;

namespace FormatParser.Library.Formats.Grammar;

public class XmlGrammar
{
    protected const char SingleQuote = '\'';
    protected const char DoubleQuote = '\"';
    protected const string ContentPropertyName = "contentValue";

    protected Parser<string> Comment =>
        CommonParse.MultilineComment("<!--", "-->");

    protected Parser<Property> Number =>
        from number in Parse.CharExcept("<").Many().Text()
                        .Where(text => int.TryParse(text, out int _))
        select new Property(int.Parse(number));

    protected Parser<Property> Decimal =>
        from decimalNumber in Parse.CharExcept("<").Many().Text()
                        .Where(text => double.TryParse(text, out double _))
        select new Property(double.Parse(decimalNumber));

    protected Parser<string> Identifier =>
        from first in Parse.Letter.Once().Text()
        from rest in Parse.LetterOrDigit.XOr(Parse.Char('-')).XOr(Parse.Char('_')).Many().Text()
        select first + rest;

    protected virtual Parser<Item> TextContent =>
        from text in Parse.CharExcept('<').Many().Text()
        select new Property(text);

    protected Parser<Item> FullNodeContent =>
        Number.Or(Decimal).Or(TextContent);

    protected Parser<Item> AttributeText(char escapeChar) =>
        from text in Parse.CharExcept(escapeChar).Many().Text()
        select new Property(text);

    protected Parser<T> Quotes<T>(char quoteChar, Parser<T> contentParser) =>
        from startQuote in Parse.Char(quoteChar)
        from content in contentParser
        from endQuote in Parse.Char(quoteChar).Token()
        select content;

    protected Parser<Item> QuotedContent(char quoteChar) =>
        Quotes(quoteChar, CommonParse.Number)
        .Or(Quotes(quoteChar, CommonParse.Decimal))
        .Or(Quotes(quoteChar, AttributeText(quoteChar)));

    protected Parser<Item> AttributeValue =>
        QuotedContent(DoubleQuote).Or(QuotedContent(SingleQuote));

    protected Parser<T> Tag<T>(Parser<T> tagContent) =>
        from startChar in Parse.Char('<')
        from content in tagContent
        from endChar in Parse.Char('>').Token()
        select content;

    protected Parser<string> EndTag(string name) =>
        Tag(from slash in Parse.Char('/')
            from id in Identifier
            where id == name
            select id).Named("closing tag for " + name);

    protected Parser<KeyValuePair<string, Item>> Attribute =>
        from identifier in Identifier
        from assignment in Parse.Char('=').Token()
        from value in AttributeValue
        select new KeyValuePair<string, Item>(identifier, value);

    protected Parser<KeyValuePair<string, Item>> ShortNode => Tag(
        from id in Identifier
        from attributes in Attribute.Token().Many()
        from slash in Parse.Char('/')
        select
            attributes.Any()
            ? new KeyValuePair<string, Item>(id, new Entity(attributes))
            : new KeyValuePair<string, Item>(id, new Property(null))
    );

    protected Parser<(T, IEnumerable<KeyValuePair<string, Item>>)> AttributeTag<T>(Parser<T> tagContent) =>
        from startChar in Parse.Char('<')
        from content in tagContent.Token()
        from attributes in Attribute.Many()
        from endChar in Parse.Char('>').Token()
        select (content, attributes);

    protected Parser<(string Tag, IEnumerable<KeyValuePair<string, Item>> Attributes)>
        BeginAttributeTag => AttributeTag(Identifier);

    protected Parser<KeyValuePair<string, Item>> FullPlainNode =>
        from beginTag in BeginAttributeTag
        let attributes = beginTag.Attributes
        let tag = beginTag.Tag
        from content in FullNodeContent
        from endTag in EndTag(tag)
        select new KeyValuePair<string, Item>(tag,
            attributes.Any()
            ? new Entity(attributes.Append(new(ContentPropertyName, content)))
            : content);

    protected Parser<KeyValuePair<string, Item>> FullNode =>
        from beginTag in BeginAttributeTag
        let attributes = beginTag.Attributes
        let tag = beginTag.Tag
        from nodes in Parse.Ref(() => Item).Many()
        from innerComment in Comment.Many().Token()
        from endTag in EndTag(tag)
        select new KeyValuePair<string, Item>(tag,
            new Entity(nodes.Concat(attributes).GroupBy(identifierValue => identifierValue.Key)
                .Select(grouping => new KeyValuePair<string, Item>(grouping.Key,
                    grouping.Count() > 1
                    ? new Property(new List<Item>(grouping
                        .Select(identifierValue => identifierValue.Value)))
                    : grouping.First().Value))
            )
        );

    protected Parser<KeyValuePair<string, Item>> Node =>
        ShortNode.Or(FullPlainNode).Or(FullNode);

    protected Parser<KeyValuePair<string, Item>> Item =>
        from leading in Comment.Many()
        from item in Node.Token()
        from trailing in Comment.Many()
        select item;

    protected Parser<string> XmlVersion =>
        CommonParse.MultilineComment("<?", "?>");

    public Parser<Entity> Parser =>
        from leading in Parse.WhiteSpace.Many()
        from xmlVersion in XmlVersion.Optional()
        from fullNode in Item.Once()
        select (Entity)fullNode.First().Value;
}
