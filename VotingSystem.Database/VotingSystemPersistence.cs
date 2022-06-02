using VotingSystem.Application;
using VotingSystem.Models;

namespace VotingSystem.Database;

public class VotingSystemPersistence : IVotingSystemPersistence
{
    private readonly AppDbContext _context;

    public VotingSystemPersistence(AppDbContext context)
    {
        _context = context;
    }

    public void SaveVote(Vote vote)
    {
        _context.Votes.Add(vote);
        _context.SaveChanges();
    }

    public void SaveVotingPoll(VotingPoll votingPoll)
    {
        _context.VotingPolls.Add(votingPoll);
        _context.SaveChanges();
    }

    public bool VoteExists(Vote vote)
    {
        return _context.Votes.Any(x => x.UserId.Equals(vote.UserId) && x.CounterId.Equals(vote.CounterId));
    }
}