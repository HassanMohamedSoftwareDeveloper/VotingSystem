using System.Collections;

namespace VotingSystem.Tests;

public class VotingPollTests
{
    [Fact]
    public void ZeroCountersWhenCreated()
    {
        var poll = new VotingPoll();
        Empty(poll.Counters);
    }
}

public class VotingPollFactoryTests
{
    private VotingPollFactory _factory = new VotingPollFactory();
    private VotingPollFactory.Request _request = new VotingPollFactory.Request
    {
        Title = "title",
        Description = "description",
        Names = new[] { "name1", "name2" },
    };

    [Fact]
    public void Create_ThrowWhenLessThanTwoCounterNames()
    {
        _request.Names = new[] { "name" };
        Throws<ArgumentException>(() => _factory.Create(_request));
        _request.Names = Array.Empty<string>();
        Throws<ArgumentException>(() => _factory.Create(_request));
    }
    [Fact]
    public void Create_AddsCounterToThePollForEachName()
    {
        var poll = _factory.Create(_request);
        foreach (var name in _request.Names)
        {
            Contains(name, poll.Counters.Select(x => x.Name));
        }
    }

    [Fact]
    public void Create_AddsTitleToThePoll()
    {
        var poll = _factory.Create(_request);
        Equal(_request.Title, poll.Title);
    }

    [Fact]
    public void Create_AddsDescriptionToThePoll()
    {
        var poll = _factory.Create(_request);
        Equal(_request.Description, poll.Description);
    }
}
public interface IVotingPollFactory
{
    VotingPoll Create(VotingPollFactory.Request request);
}
public class VotingPollFactory
{
    public class Request
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string[] Names { get; set; }
    }
    public VotingPoll Create(Request request)
    {
        if (request.Names.Length < 2)
            throw new ArgumentException();
        return new VotingPoll
        {
            Title = request.Title,
            Description = request.Description,
            Counters = request.Names.Select(name => new Counter { Name = name }),
        };
    }
}

public class VotingPoll
{
    public VotingPoll()
    {
        Counters = Enumerable.Empty<Counter>();
    }

    public IEnumerable<Counter> Counters { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}