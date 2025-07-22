using Algorithm.Utilities.Helpers;

namespace Algorithms.Core.FibonacciSequence
{

    public class FibonacciSequence : IAlgorithm
    {
        public string Name => "Fibonacci Sequence";

        public void Run()
        {
            Console.Write("¿How many terms would you like to see? \n");
            var terms = ConsoleHelper.ReadLineInt();

            var sequence = Generate(terms);
            Console.WriteLine($"Fibonacci({terms}): {string.Join(", ", sequence)}");
        }

        public static IEnumerable<int> Generate(int n)
        {
            var result = new List<int> { 0 };

            if (n > 1)
            {
                result.Add(1);
            }

            for (int i = 2; i < n; i++)
            {
                result.Add(result[i - 2] + result[i - 1]);
            }

            return result;
        }
    }
}
