namespace VotingSystem.Application;

public class StatisticsInteractor
{
    private readonly IVotingSystemPersistence _persistence;
    private readonly ICounterManager _counterManager;

    public StatisticsInteractor(IVotingSystemPersistence persistence, ICounterManager counterManager)
    {
        _persistence = persistence;
        _counterManager = counterManager;
    }

    public PollStatistics GetStatistics(int pollId)
    {
        var poll = _persistence.GetPoll(pollId);

        var statistics = _counterManager.GetStatistics(poll.Counters);

        _counterManager.ResolveExcess(statistics);

        return new PollStatistics
        {
            Title = poll.Title,
            Description = poll.Description,
            Counters = statistics
        };
    }
}

