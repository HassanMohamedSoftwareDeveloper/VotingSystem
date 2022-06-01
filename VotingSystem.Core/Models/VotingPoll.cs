namespace VotingSystem.Models;

public class VotingPoll
{
    public VotingPoll()
    {
        Counters = Enumerable.Empty<Counter>();
    }

    public IEnumerable<Counter> Counters { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}