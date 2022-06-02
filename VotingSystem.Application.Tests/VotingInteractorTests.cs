using Moq;
using VotingSystem.Models;

namespace VotingSystem.Application.Tests;

public class VotingInteractorTests
{
    private Mock<IVotingSystemPersistence> _mockPersistence = new Mock<IVotingSystemPersistence>();
    private readonly VotingInteractor _interactor;
    private readonly Vote _vote = new Vote() { UserId = "user", CounterId = 1 };
    public VotingInteractorTests()
    {
        _interactor = new VotingInteractor(_mockPersistence.Object);
    }
    [Fact]
    public void Vote_PersistsVoteWhenUserHasNotVoted()
    {
        _mockPersistence.Setup(x => x.VoteExists(_vote)).Returns(false);

        _interactor.Vote(_vote);

        _mockPersistence.Verify(x => x.SaveVote(_vote));
    }

    [Fact]
    public void Vote_DoesnotPersistVoteWhenUserAlreadyVote()
    {
        _mockPersistence.Setup(x => x.VoteExists(_vote)).Returns(true);

        _interactor.Vote(_vote);

        _mockPersistence.Verify(x => x.SaveVote(_vote), Times.Never);
    }
}
