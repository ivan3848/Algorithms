using Algorithm.Utilities.Helpers;

namespace Algorithms.Core.SumDigitsOfOneInteger
{
    public class ReverseString : IAlgorithm
    {

        public string Name => "Reverse Word";

        public void Run()
        {
            Console.Write("Write the word to reverse\n");
            var word = ConsoleHelper.ReadLineString();

            var result = Generate(word);
            Console.WriteLine($"Original Word: {word} - Reverse Word: {result}");
        }

        public static string Generate(string word)
        {
            var chars = new List<char>(word.Length);

            for (int i = word.Length - 1; i >= 0; i--)
            {
                chars.Add(word[i]);
            }

            return string.Join(",", chars).Replace(",", "");
        }
    }
}
