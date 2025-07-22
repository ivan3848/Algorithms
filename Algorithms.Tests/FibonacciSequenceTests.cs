using Algorithms.Core.FibonacciSequence;

namespace Algorithms.Tests
{
    public class FibonacciSequenceTests
    {
        [Theory]
        [InlineData(0, new int[] { })]
        [InlineData(1, new int[] { 0 })]
        [InlineData(2, new int[] { 0, 1 })]
        [InlineData(3, new int[] { 0, 1, 1 })]
        [InlineData(5, new int[] { 0, 1, 1, 2, 3 })]
        [InlineData(10, new int[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34 })]
        public void Generate_ReturnsExpectedFibonacciSequence(int input, int[] expected)
        {
            var result = FibonacciSequence.Generate(input);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void Generate_NegativeInput_ReturnsEmptyList()
        {
            var result = FibonacciSequence.Generate(-5);
            Assert.Empty(result);
        }
    }
}
