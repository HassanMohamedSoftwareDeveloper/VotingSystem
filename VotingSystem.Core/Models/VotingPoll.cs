namespace VotingSystem.Models;

public class VotingPoll
{
    public VotingPoll()
    {
        Counters = new List<Counter>();
    }

    public ICollection<Counter> Counters { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}