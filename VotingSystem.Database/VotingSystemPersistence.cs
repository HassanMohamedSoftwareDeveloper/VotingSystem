using Microsoft.EntityFrameworkCore;
using VotingSystem.Models;

namespace VotingSystem.Database;

public class VotingSystemPersistence : IVotingSystemPersistence
{
    private readonly AppDbContext _context;

    public VotingSystemPersistence(AppDbContext context)
    {
        _context = context;
    }

    public VotingPoll GetPoll(int pollId)
    {
        return _context.VotingPolls
            .Include(x => x.Counters)
            .Where(x => EF.Property<int>(x, "Id") == pollId)
            .Select(poll => new VotingPoll
            {
                Title = poll.Title,
                Description = poll.Description,
                Counters = poll.Counters.Select(y => new Counter
                {
                    Id=y.Id,
                    Name = y.Name,
                    Count = y.Votes.Count,
                }).ToList(),
            })
            .FirstOrDefault();
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