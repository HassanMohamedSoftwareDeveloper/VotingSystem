using VotingSystem.Models;

namespace VotingSystem.Application;

public interface IVotingSystemPersistence
{
    void SaveVotingPoll(VotingPoll votingPoll);
}

