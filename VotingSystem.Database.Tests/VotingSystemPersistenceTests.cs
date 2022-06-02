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

    [Fact]
    public void PersistVote()
    {
        var vote = new Vote
        {
           UserId="user",
           CounterId=1
        };
        using (var ctx = DbContextFactory.Create(nameof(PersistVote)))
        {
           var persistence= new VotingSystemPersistence(ctx);
            persistence.SaveVote(vote);
        }

        using (var ctx = DbContextFactory.Create(nameof(PersistVote)))
        {
            var savedVote = ctx.Votes.Single();
            Equal(vote.UserId, savedVote.UserId);
            Equal(vote.CounterId, savedVote.CounterId);
        }

    }

    [Fact]
    public void VoteExists_ReturnsFalseWhenNoVote()
    {

        var vote = new Vote
        {
            UserId = "user",
            CounterId = 1
        };

        using (var ctx = DbContextFactory.Create(nameof(VoteExists_ReturnsFalseWhenNoVote)))
        {
            var persistence = new VotingSystemPersistence(ctx);
            False(persistence.VoteExists(vote));
        }

    }

    [Fact]
    public void VoteExists_ReturnsTrueWhenVoteExists()
    {

        var vote = new Vote
        {
            UserId = "user",
            CounterId = 1
        };
        using (var ctx = DbContextFactory.Create(nameof(VoteExists_ReturnsTrueWhenVoteExists)))
        {
            var persistence = new VotingSystemPersistence(ctx);
            persistence.SaveVote(vote);
        }
        using (var ctx = DbContextFactory.Create(nameof(VoteExists_ReturnsTrueWhenVoteExists)))
        {
            var persistence = new VotingSystemPersistence(ctx);
            True(persistence.VoteExists(vote));
        }

    }
}
