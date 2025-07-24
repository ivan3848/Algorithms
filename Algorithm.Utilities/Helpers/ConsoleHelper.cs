namespace Algorithm.Utilities.Helpers
{
    public static class ConsoleHelper
    {
        public static int ReadLineInt()
        {
            int result;
            do
            {
                if (int.TryParse(Console.ReadLine()?.Trim(), out result))
                {
                    break;
                }

                Console.WriteLine("Invalid Entry, try again and make sure to type a number");
            } while (true);

            return result;
        }

        public static string ReadLineString()
        {
            string? result;
            do
            {
                result = Console.ReadLine()?.Trim();

                if (!string.IsNullOrWhiteSpace(result))
                {
                    break;
                }
                Console.WriteLine("Invalid Entry, try again and make sure to type a valid string");
            } while (true);

            return result;
        }
    }
}
