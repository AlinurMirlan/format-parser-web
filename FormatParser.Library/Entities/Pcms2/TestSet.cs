namespace FormatParser.Library.Entities.Pcms2;

public class TestSet
{
    public string? Name { get; set; }
    public int TimeLimit { get; set; }
    public int MemoryLimit { get; set; }
    public int TestCount { get; set; }
    public string? InputPathPattern { get; set; }
    public string? OutputPathPattern { get; set; }
    public string? AnswerPathPattern { get; set; }

    [ListWrapper]
    public List<Test>? Tests { get; set; }
}