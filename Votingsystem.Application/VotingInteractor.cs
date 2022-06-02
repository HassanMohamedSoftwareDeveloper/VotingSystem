using VotingSystem.Models;

namespace VotingSystem.Application;

public class VotingInteractor
{
    private readonly IVotingSystemPersistence _persistence;

    public VotingInteractor(IVotingSystemPersistence persistence)
    {
        _persistence = persistence;
    }

    public void Vote(Vote vote)
    {
        if (_persistence.VoteExists(vote).Equals(false))
            _persistence.SaveVote(vote);
    }
}