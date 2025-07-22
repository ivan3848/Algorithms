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
            int initialValue = 0;
            int nextValue = 1;
            var result = new List<int>();

            for (int i = 0; i < n; i++)
            {
                result.Add(initialValue);

                var prevValue = initialValue;
                initialValue = nextValue;
                nextValue += prevValue;

            }

            return result;
        }


        public static IEnumerable<int> Generate1(int n)
        {
            var result = new List<int>();

            if (n > 0)
            {
                result.Add(0);
            }
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
