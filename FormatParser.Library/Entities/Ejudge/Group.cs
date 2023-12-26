namespace FormatParser.Library.Entities.Ejudge;

public class Group
{
    public int Id { get; set; }
    public Range Tests { get; set; }
    public int Score { get; set; }
    public int TestScore { get; set; }
    public List<int> Requires { get; set; } = new List<int>();
    public bool Offline { get; set; }
    public bool SetsMarked { get; set; }
    public bool Skip { get; set; }
    public List<int> SetsMarkedIfPassed { get; set; } = new List<int>();
    public int PassIfCount { get; set; }
    public bool SkipIfNotRejudge { get; set; }
    public bool StatToJudges { get; set; }
    public bool StatToUsers { get; set; }
    public int UserStatus { get; set; }
    public bool TestAll { get; set; }

    [EntityName("0_if")]
    public List<int> OIf { get; set; } = new List<int>();

}
