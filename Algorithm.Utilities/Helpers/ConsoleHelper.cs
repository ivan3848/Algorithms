namespace Algorithm.Utilities.Helpers
{
    public static class ConsoleHelper
    {
        public static int ReadLineInt()
        {
            int result;
            do
            {
                if (int.TryParse(Console.ReadLine(), out result))
                {
                    break;
                }

                Console.WriteLine("Invalid Entry, try again and make sure to type a number");
            } while (true);

            return result;
        }
    }
}
