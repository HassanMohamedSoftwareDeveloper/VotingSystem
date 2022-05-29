using Moq;

namespace VotingSystem.Tests
{
    public class MathOne
    {
        private readonly ITestOne _testOne;

        public MathOne(ITestOne testOne)
        {
            _testOne = testOne;
        }
        public int Add(int a, int b) => _testOne.Add(a, b);
    }

    public class MathOneTests
    {
        [Fact]
        public void MathOneAddsTwoNumbers()
        {
            var testOne = new TestOne();
            var mathOne = new MathOne(testOne);
            Assert.Equal(2, mathOne.Add(1, 1));
        }
        [Fact]
        public void MathOneAddsTwoNumbers_WithMoq()
        {
            var testOneMock = new Mock<ITestOne>();
            testOneMock.Setup(x => x.Add(1, 1)).Returns(2);
            var mathOne = new MathOne(testOneMock.Object);
            Assert.Equal(2, mathOne.Add(1, 1));
        }
    }



    public interface ITestOne
    {
        int Add(int a, int b);
    }
    public class TestOne : ITestOne
    {
        public int Add(int a, int b) => a + b;
    }
    
    public class TestOneTests
    {
        [Fact]
        public void Add_AddsTwoNumbersTogether()
        {
            var result = new TestOne().Add(1, 1);
            Assert.Equal(2, result);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 1, 1)]
        public void Add_AddsTwoNumbersTogether_Theory(int a, int b, int expected)
        {
            var result = new TestOne().Add(a, b);
            Assert.Equal(expected, result);
        }
        [Fact]
        public void TestListContainsValue()
        {
            List<int> list = new() { 1, 2, 3 };
            Assert.Contains(1, list);
        }
    }
}