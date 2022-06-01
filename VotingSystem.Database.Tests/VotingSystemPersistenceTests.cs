using Microsoft.EntityFrameworkCore;
using VotingSystem.Application;
using VotingSystem.Database.Tests.Infrastructure;
using VotingSystem.Models;

namespace VotingSystem.Database.Tests;

public class VotingSystemPersistenceTests
{
    [Fact]
    public void PersistVotingPoll()
    {
        var poll = new VotingPoll
        {
            Title = "title",
            Description = "desc",
            Counters = new List<Counter>
            {
                new Counter{ Name="One"},
                new Counter{ Name="Two"},
            }
        };
        using (var ctx = DbContextFactory.Create(nameof(PersistVotingPoll)))
        {
            IVotingSystemPersistence persistence = new VotingSystemPersistence(ctx);
            persistence.SaveVotingPoll(poll);
        }

        using (var ctx = DbContextFactory.Create(nameof(PersistVotingPoll)))
        {
            var savedPoll = ctx.VotingPolls.Include(x => x.Counters).Single();

            Equal(poll.Title,savedPoll.Title);
            Equal(poll.Description,savedPoll.Description);
            Equal(poll.Counters.Count(), savedPoll.Counters.Count());

            foreach (var name in poll.Counters.Select(x=>x.Name))
            {
                Contains(name, savedPoll.Counters.Select(x => x.Name));
            }
        }

    }
}

public class VotingSystemPersistence : IVotingSystemPersistence
{
    private readonly AppDbContext _context;

    public VotingSystemPersistence(AppDbContext context)
    {
        _context = context;
    }
    public void SaveVotingPoll(VotingPoll votingPoll)
    {
        _context.VotingPolls.Add(votingPoll);
        _context.SaveChanges();
    }
}