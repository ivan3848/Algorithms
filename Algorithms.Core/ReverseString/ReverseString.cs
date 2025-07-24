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
            var wordLength = word.Length;
            var chars = new char[wordLength];

            for (int i = 0; i < wordLength; i++)
            {
                var letter = word[i];
                chars[wordLength - (i + 1)] = letter;
            }

            return string.Join(",", chars).Replace(",", "");
        }
    }
}
