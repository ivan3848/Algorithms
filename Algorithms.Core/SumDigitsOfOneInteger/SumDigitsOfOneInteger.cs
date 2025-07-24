using Algorithm.Utilities.Helpers;

namespace Algorithms.Core.SumDigitsOfOneInteger
{
    public class SumDigitsOfOneInteger : IAlgorithm
    {

        public string Name => "Sum Digits Of An Integer";

        public void Run()
        {
            Console.Write("Write the integer with no space \n");
            var number = ConsoleHelper.ReadLineInt();

            var result = Generate(number);
            Console.WriteLine($"Number({number}) Sum: {result}");
        }

        public static int Generate(int n)
        {
            var result = 0;

            while (n > 0)
            {
                int module = n % 10;
                result += module;
                n /= 10;
            }

            return result;
        }
    }
}
