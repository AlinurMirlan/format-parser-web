using Sprache;

namespace FormatParser.Library.Formats.Grammar;

public class CatsGrammar : XmlGrammar
{
    protected static Parser<string> TextTag(Parser<string> tagContent) =>
        from startChar in Parse.Char('<')
        from content in tagContent
        from endChar in Parse.Char('>').Token()
        select $"{startChar}{content}{endChar}";

    protected Parser<string> BeginTextTag(string name) =>
    TextTag(
        from id in Identifier
        where id == name
        from attributes in Parse.CharExcept('>').Many().Text()
        select $"{id}{attributes}");

    protected Parser<string> EndTextTag(string name) =>
        TextTag(
            from slash in Parse.Char('/')
            from id in Identifier
            where id == name
            select id).Named("closing tag for " + name);

    protected Parser<string> ShortTextTag(string name) =>
        TextTag(
            from id in Identifier
            where id == name
            from attributes in Parse.CharExcept('/').Many().Text()
            from slash in Parse.Char('/')
            select $"{id}{attributes}{slash}");

    protected Parser<string> FullTextTag(string name) =>
        from beginTag in BeginTextTag(name)
        from content in Parse.AnyChar.Until(EndTextTag(name)).Text()
        select beginTag + content + $"</{name}>";

    protected static readonly Parser<string> PlainText = Parse.CharExcept('<').Many().Text();

    public Parser<string> StmlTag =>
        FullTextTag("img").Or(ShortTextTag("img"))
        .Or(FullTextTag("a")).Or(FullTextTag("object"))
        .Or(FullTextTag("include")).Or(FullTextTag("code"))
        .Or(FullTextTag("i")).Or(FullTextTag("em"))
        .Or(FullTextTag("b")).Or(FullTextTag("p"))
        .Or(FullTextTag("table")).Or(FullTextTag("tr"))
        .Or(FullTextTag("td")).Or(FullTextTag("th")).Text();

    protected override Parser<Item> TextContent =>
        from text in PlainText.Or(StmlTag).Many()
        select new Property(text.Aggregate("", (text, unit) => text + unit));
}
