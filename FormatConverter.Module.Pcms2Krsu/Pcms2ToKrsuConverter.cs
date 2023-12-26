using FormatConverter.Library;
using FormatParser.Library.Entities.Krsu;
using FormatParser.Library.Entities.Pcms2;
using FormatParser.Library.Formats;
using FormatParser.Library.Formats.Parsers;
using FormatParser.Library.Readers;
using KrsuTest = FormatParser.Library.Entities.Krsu.Test;

namespace FormatConverter.Module.Pcms2Krsu;

public class Pcms2ToKrsuConverter : KrsuFormatConverter
{
    private readonly Pcms2FormatReader _pcms2Reader = new(
        new ParseFile<Pcms2Problem>(new XmlParser<Pcms2Problem>(), "problem.xml"));

    public override string ConvertFrom => "PCMS2";

    protected override TestInfo GetKrsuEntity(DirectoryInfo sourceFormatDirectory)
    {
        Pcms2Problem pcms2Entity = _pcms2Reader.Parse(sourceFormatDirectory) 
            ?? throw new InvalidFormatException();
        var krsuEntity = new TestInfo()
        {
            TestVersion = int.TryParse(pcms2Entity.Revision, out int revision) ? revision : null,
            Checker = pcms2Entity.Assets?.Checker?.Name ?? string.Empty,
            ProblemStatement = pcms2Entity.Statements?.Aggregate(string.Empty,
                (accumulator, statement) => accumulator + $"language = {statement.Language}, path = {statement.Path}; "),
            MemoryLimitByte = pcms2Entity.Judging?.TestSet?.MemoryLimit ?? 0,
            TimeLimitMilli = pcms2Entity.Judging?.TestSet?.TimeLimit ?? 0,
            Groups = pcms2Entity.Judging?.TestSet?.Tests
                ?.Select(test => int.TryParse(test.Group, out int group) ? group : -1).Where(groupId => groupId >= 0)
                    .Select(groupId => new TestGroup() { Id = groupId })?.ToList() ?? new(),
            Tests = (from test in pcms2Entity.Judging?.TestSet?.Tests
                    let inputPattern = pcms2Entity.Judging?.TestSet?.InputPathPattern
                    let outputPattern = pcms2Entity.Judging?.TestSet?.AnswerPathPattern
                    select new KrsuTest()
                    {
                        InputFile = test.FromFile is not null && inputPattern is not null 
                            ? Path.Combine(inputPattern[..inputPattern.IndexOf('%')], test.FromFile ?? string.Empty)
                            : test.FromFile,
                        OutputFile = test.FromFile is not null && outputPattern is not null
                            ? Path.Combine(outputPattern[..outputPattern.IndexOf('%')],
                                test.FromFile is null ? string.Empty : $"{test.FromFile}.a")
                            : test.FromFile,
                        GroupId = int.TryParse(test.Group, out int groupId) ? groupId : 0
                    })?.ToList() ?? new()
        };

        return krsuEntity;
    }
}
