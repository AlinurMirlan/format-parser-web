using FormatConverter.Library;
using FormatParser.Library.Entities.Ejudge;
using FormatParser.Library.Entities.Krsu;
using FormatParser.Library.Formats;
using FormatParser.Library.Formats.Parsers.Ejudge;
using FormatParser.Library.Readers;

namespace FormatConverter.Module.EjudgeToKrsu;

public class EjudgeToKrsuConverter : KrsuFormatConverter
{
    private readonly EjudgeFormatReader _ejudgeReader = new(
        new ParseFile<EjudgeProblem>(new ProblemParser<EjudgeProblem>(), "problem.cfg"),
        new ParseFile<Valuer>(new ValuerParser<Valuer>(), "valuer.cfg"));

    public override string ConvertFrom => "EJUDGE";

    protected override TestInfo GetKrsuEntity(DirectoryInfo sourceFormatDirectory)
    {
        EjudgeInfo ejudgeEntity = _ejudgeReader.Parse(sourceFormatDirectory);
        if (ejudgeEntity.Valuer is null || ejudgeEntity.Problem is null)
            throw new InvalidFormatException();

        var krsuEntity = new TestInfo()
        {
            TestVersion = ejudgeEntity.Problem.Revision,
            ProblemStatement = $"{ejudgeEntity.Problem.LongName} {ejudgeEntity.Problem.LongNameEn} {ejudgeEntity.Problem.InternalName}",
            Checker = ejudgeEntity.Problem.CheckCmd,
            MemoryLimitByte = int.Parse(
                ejudgeEntity.Problem.MaxVmSize.Aggregate("",
                    (accumulator, @char) => accumulator + (char.IsDigit(@char) ? @char.ToString() : ""))
                ) * 1000,
            TimeLimitMilli = (int)ejudgeEntity.Problem.TimeLimit * 1000,
            Groups = ejudgeEntity.Valuer.Groups.Select(group => new TestGroup()
                {
                    Id = group.Id,
                    Points = group.Score,
                    Prerequisites = group.Requires.Count > 0 
                        ? group.Requires.Aggregate("", (accumulator, groupId) => accumulator + $"{groupId},")[..^1]
                        : string.Empty
                }).ToList(),
            Tests = (from testGroup in ejudgeEntity.Valuer.Groups
                    let fromTest = testGroup.Tests.Start.Value
                    let toTest = testGroup.Tests.End.Value
                    from test in Enumerable.Range(fromTest, toTest - fromTest)
                    select new Test() 
                    { 
                        InputFile = test.ToString(), 
                        GroupId = testGroup.Id, 
                        Points = testGroup.TestScore 
                    }).ToList()
        };

        return krsuEntity;
    }
}
