using Microsoft.EntityFrameworkCore;
using VotingSystem.Application;
using VotingSystem.Database;
using VotingSystem.Database.Tests.Infrastructure;
using VotingSystem.Models;

namespace VotingSystem.Integration.Tests
{
    public class VotingTests
    {
        private AppDbContext _ctx;
        private IVotingSystemPersistence _persistence;
        private readonly VotingInteractor _votingInteractor;
        private VotingPollInteractor _pollInteractor;
        public VotingTests()
        {
            _ctx= DbContextFactory.Create(Guid.NewGuid().ToString());
            _persistence = new VotingSystemPersistence(_ctx);
            _votingInteractor = new VotingInteractor(_persistence);
            _pollInteractor = new VotingPollInteractor(new VotingPollFactory(), _persistence);
        }

        private VotingPoll NewPoll()=> new VotingPoll
        {
            Title = "title",
            Description = "desc",
            Counters = new List<Counter>
            {
                new Counter{ Name="One"},
                new Counter{ Name="Two"},
            }
        };

        [Fact]
        public void SavesVoteToDatabaseWhenVotingPollExists()
        {
            _ctx.VotingPolls.Add(NewPoll());
            _ctx.SaveChanges();

            _votingInteractor.Vote(new Vote { UserId = "user", CounterId = 1 });

            var vote = _ctx.Votes.Single();

            Equal("user",vote.UserId);
            Equal(1,vote.CounterId);

            var counter = _ctx.Counters.Include(x => x.Votes).First(x => x.Id == 1);

            Single(counter.Votes);
        }
        [Fact]
        public void SavesVoteToDatabaseAfterPollCreatedWithInteractor()
        {
            _pollInteractor.CreateVotingPoll(new VotingPollFactory.Request()
            {
                Title = "title",
                Description = "desc",
                Names = new[] { "One", "Two" }
            });

            _votingInteractor.Vote(new Vote { UserId = "user", CounterId = 1 });

            var vote = _ctx.Votes.Single();

            Equal("user", vote.UserId);
            Equal(1, vote.CounterId);

            var counter = _ctx.Counters.Include(x => x.Votes).First(x => x.Id == 1);

            Single(counter.Votes);
        }
    }
}