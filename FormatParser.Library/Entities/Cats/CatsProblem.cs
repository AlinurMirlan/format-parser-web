namespace FormatParser.Library.Entities.Cats;

public class CatsProblem
{
    public string? Title { get; set; }
    public string? Lang { get; set; }
    public int Tlimit { get; set; }
    public int Mlimit { get; set; }
    public int Wlimit { get; set; } = 30;
    public string? Author { get; set; }
    public string? InputFile { get; set; }
    public string? OutputFile { get; set; }
    public int Difficulty { get; set; }
    public string? StdChecker { get; set; }
    public string? MaxPoints { get; set; }
    public string? SaveInputPrefix { get; set; }
    public string? SaveOutputPrefix { get; set; }
    public string? SaveAnswerPrefix { get; set; }

    [EntityName("Keyword")]    
    public Keyword? Keyword { get; set; }

    [EntityName("ProblemStatement")]    
    public ProblemStatement? ProblemStatement { get; set; }

    [EntityName("ProblemConstraints")]
    public string? ProblemConstraints { get; set; }

    [EntityName("InputFormat")]
    public string? InputFormat { get; set; }

    [EntityName("OutputFormat")]
    public string? OutputFormat { get; set; }

    [EntityName("JsonData")]
    public string? JsonData { get; set; }

    [EntityName("Explanation")]
    public Explanation? Explanation { get; set; }

    [EntityName("Checker")]
    public Checker? Checker { get; set; }

    [EntityName("Picture")]
    public Attachment? Picture { get; set; }

    [EntityName("Attachment")]
    public Attachment? Attachment { get; set; }

    [EntityName("Solution")]
    public Solution? Solution { get; set; }

    [EntityName("Generator")]
    public Generator? Generator { get; set; }

    [EntityName("GeneratorRange")]
    public GeneratorRange? GeneratorRange { get; set; }

    [EntityName("Validator")]
    public Validator? Validator { get; set; }

    [EntityName("Visualizer")]
    public Visualizer? Visualizer { get; set; }

    [EntityName("Interactor")]
    public Interactor? Interactor { get; set; }

    [EntityName("Run")]
    public Run? Run { get; set; }

    [EntityName("Snippet")]
    public Snippet? Snippet { get; set; }

    [EntityName("Test")]
    public Test? Test { get; set; }

    [EntityName("TestRange")]
    public TestRange? TestRange { get; set; }

    [EntityName("TestSet")]
    public TestSet? TestSet { get; set; }

    [EntityName("Module")]
    public Module? Module { get; set; }

    [EntityName("Import")]
    public Import? Import { get; set; }

    [EntityName("Sample")]
    public List<Sample>? Samples { get; set; }
}
