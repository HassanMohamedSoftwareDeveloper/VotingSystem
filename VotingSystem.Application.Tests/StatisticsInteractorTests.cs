using Moq;
using VotingSystem.Models;

namespace VotingSystem.Application.Tests;

public class StatisticsInteractorTests
{
    private Mock<IVotingSystemPersistence> _mockPersistence = new Mock<IVotingSystemPersistence>();
    private Mock<ICounterManager> _mockCounerManager = new Mock<ICounterManager>();

    [Fact]  
    public void DisplaysPollStatistics()
    {
        int pollId = 1;
        var counter1 = new Counter { Name = "One", Count = 2 };
        var counter2 = new Counter { Name = "Two", Count = 1 };


        var counterStats1 = new CounterStatistics { Name = "One", Count = 2, Percent = 60, };
        var counterStats2 = new CounterStatistics { Name = "Two", Count = 1, Percent = 40 };

        var counterStats = new List<CounterStatistics> { counterStats1, counterStats2 };
        var poll = new VotingPoll
        {
            Title = "title",
            Description = "desc",
            Counters = new List<Counter> { counter1, counter2 }
        };

        _mockPersistence.Setup(x => x.GetPoll(pollId)).Returns(poll);

        _mockCounerManager.Setup(x=>x.GetStatistics(poll.Counters)).Returns(counterStats);

        var interactor = new StatisticsInteractor(_mockPersistence.Object,_mockCounerManager.Object);

        var pollStatistics = interactor.GetStatistics(pollId);

        Equal(poll.Title, pollStatistics.Title);
        Equal(poll.Description, pollStatistics.Description);

        var stats1 = pollStatistics.Counters[0];

        Equal(counterStats1.Name, stats1.Name);
        Equal(counterStats1.Count, stats1.Count);
        Equal(counterStats1.Percent, stats1.Percent);

        var stats2 = pollStatistics.Counters[1];

        Equal(counterStats2.Name, stats2.Name);
        Equal(counterStats2.Count, stats2.Count);
        Equal(counterStats2.Percent, stats2.Percent);


        _mockCounerManager.Verify(x => x.ResolveExcess(counterStats),Times.Once);
    }
}