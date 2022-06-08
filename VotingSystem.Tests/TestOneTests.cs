using Moq;
using Xunit.Abstractions;
using Xunit.Sdk;

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

    [TestCaseOrderer("VotingSystem.Tests.PriorityCaseOrder", "VotingSystem.Tests")]
    public class Tests
    {
        [Fact]
        [Trait("Cat 1","1")]
        [TestPriority(2)]
        public void T1()
        {
            True(true);
        }
        [Fact(/*Skip ="ttt",DisplayName ="Test 2"*/)]
        [Trait("Cat 2", "2")]
        [TestPriority(1)]
        public void T2()
        {
            True(true);
        }
        [Fact]
        [Trait("Cat 1", "1")]
        [TestPriority(1)]
        public void T3()
        {
            True(true);
        }
        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void T4(bool expected)
        {
            True(expected);
        }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TestPriorityAttribute : Attribute
    {
        private static List<int> _priorities = new List<int>();
        public int Priority { get; private set; }
        public TestPriorityAttribute(int priority)
        {
            if (_priorities.Any(x => x == priority))
            {
                return;
            }
            Priority = priority;
            _priorities.Add(priority);
        }
    }
    public class PriorityCaseOrder : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) 
            where TTestCase : ITestCase
        {
            string assemblyName = typeof(TestPriorityAttribute).AssemblyQualifiedName!;
            var sortedMethods = new SortedDictionary<int, List<TTestCase>>();
            foreach (TTestCase testCase in testCases)
            {
                int priority = testCase.TestMethod.Method
                    .GetCustomAttributes(assemblyName)
                    .FirstOrDefault()
                    ?.GetNamedArgument<int>(nameof(TestPriorityAttribute.Priority)) ?? 0;

                GetOrCreate(sortedMethods, priority).Add(testCase);
            }

            foreach (TTestCase testCase in
                sortedMethods.Keys.SelectMany(
                    priority => sortedMethods[priority].OrderBy(
                        testCase => testCase.TestMethod.Method.Name)))
            {
                yield return testCase;
            }
        }

        private static TValue GetOrCreate<TKey, TValue>(
            IDictionary<TKey, TValue> dictionary, TKey key)
            where TKey : struct
            where TValue : new() =>
            dictionary.TryGetValue(key, out TValue result)
                ? result
                : (dictionary[key] = new TValue());
    }
}