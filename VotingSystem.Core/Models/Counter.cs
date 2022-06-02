namespace VotingSystem.Models;

public class Counter
{
    public Counter()
    {
        Votes = new List<Vote>();
    }
    public string Name { get; set; }
    public int Count { get; set; }
    public double Percent { get; set; }
    public ICollection<Vote> Votes { get; set; }
}
