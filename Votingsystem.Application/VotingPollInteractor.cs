namespace VotingSystem.Application;

public class VotingPollInteractor
{
    private readonly IVotingPollFactory _factory;
    private readonly IVotingSystemPersistence _persistence;

    public VotingPollInteractor(IVotingPollFactory factory, IVotingSystemPersistence persistence)
    {
        _factory = factory;
        _persistence = persistence;
    }

    public void CreateVotingPoll(VotingPollFactory.Request request)
    {
        var poll = _factory.Create(request);
        _persistence.SaveVotingPoll(poll);
    }
}

