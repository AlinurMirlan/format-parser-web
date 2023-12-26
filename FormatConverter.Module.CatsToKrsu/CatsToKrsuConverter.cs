using FormatConverter.Library;
using FormatParser.Library.Entities.Cats;
using FormatParser.Library.Entities.Krsu;
using FormatParser.Library.Formats;
using FormatParser.Library.Formats.Parsers;
using FormatParser.Library.Readers;
using KrsuTest = FormatParser.Library.Entities.Krsu.Test;

namespace FormatConverter.Module.CatsToKrsu;

public class CatsToKrsuConverter : KrsuFormatConverter
{
    private readonly CatsFormatReader _catsReader = new(
        new ParseFile<Cats>(new CatsParser<Cats>(), "*.xml"));

    public override string ConvertFrom => "CATS";

    protected override TestInfo GetKrsuEntity(DirectoryInfo sourceFormatDirectory)
    {
        var catsEntity = _catsReader.Parse(sourceFormatDirectory);
        var catsProblem = catsEntity.Problem ?? throw new InvalidFormatException();
        var krsuEntity = new TestInfo()
        {
            Checker = catsProblem.Checker?.Src,
            Interactor = catsProblem.Interactor?.Src,
            ProblemStatement = $"{catsProblem.Title}\n{catsEntity.Problem.ProblemStatement?.ContentValue}",
            TimeLimitMilli = catsProblem.Tlimit * 1000,
            MemoryLimitByte = catsProblem.Mlimit * 1000,
            Tests = (from testNumber in Enumerable.Range(
                                    catsProblem.TestRange?.From ?? 0,
                                    catsProblem.TestRange?.To ?? 0)
                    let testRangeIn = catsProblem.TestRange?.In
                    let testRangeOut = catsProblem.TestRange?.Out
                    let inputPath = testRangeIn.Src ?? testRangeIn.Use
                    let outputPath = testRangeOut.Src ?? testRangeOut.Use
                    select new KrsuTest() 
                    { 
                        InputFile = inputPath.Contains('%') 
                            ? Path.Combine(inputPath[..inputPath.IndexOf('%')], testNumber.ToString()) 
                            : inputPath,
                        OutputFile = outputPath.Contains('%') 
                            ? Path.Combine(outputPath[..outputPath.IndexOf('%')], testNumber.ToString())
                            : outputPath,
                        Points = catsProblem.TestRange?.Points ?? 0
                    }).ToList()
        };

        return krsuEntity;
    }
}
