using Moq;

namespace VotingSystem.Tests;

public class VotingPollInteractorTests
{
    private VotingPollFactory.Request _request = new VotingPollFactory.Request();
    private Mock<IVotingPollFactory> _mockFactory = new Mock<IVotingPollFactory>();
    private Mock<IVotingSystemPersistence> _mockPersistence = new Mock<IVotingSystemPersistence>();
    private VotingPollInteractor _interactor;
    public VotingPollInteractorTests()
    {
        _interactor = new VotingPollInteractor(_mockFactory.Object, _mockPersistence.Object);
    }
    [Fact]
    public void CreateVotingPoll_UsesVotingPollFactoryToCreateVotingPoll()
    {
        _interactor.CreateVotingPoll(_request);
        _mockFactory.Verify(x => x.Create(_request));
    }

    [Fact]
    public void CreateVotingPoll_PersistsCreatePoll()
    {
        var poll = new VotingPoll();
        _mockFactory.Setup(x => x.Create(_request)).Returns(poll);
        _interactor.CreateVotingPoll(_request);
        _mockPersistence.Verify(x => x.SaveVotingPoll(poll));
    }
}

public interface IVotingSystemPersistence
{
    void SaveVotingPoll(VotingPoll votingPoll);
}

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

