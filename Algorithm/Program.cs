using Algorithms.Core;
using System.Reflection;
using System.Text;

namespace Algorithms
{
    internal class Program
    {
        private static void Main()
        {
            int? selectedOption;

            do
            {
                var algorithms = DiscoverAlgorithms();
                var (optionText, exitOption) = ShowMenu(algorithms);
                selectedOption = ValidateOption(optionText, exitOption);

                if (selectedOption is null)
                {
                    break;
                }

                var selectedAlgorithm = algorithms.FirstOrDefault(a => a.Id == selectedOption);
                Console.Clear();

                if (selectedAlgorithm?.Instance is not null)
                {
                    Console.WriteLine($"Executing: {selectedAlgorithm.Instance.Name} \n");
                    selectedAlgorithm.Instance.Run();
                }
                else
                {
                    Console.WriteLine("Invalid algorithm.");
                }

                Console.WriteLine("\nPress any key to return to the main menu...");
                Console.ReadKey();
                Console.Clear();

            } while (true);
        }

        private static List<AlgorithmOption> DiscoverAlgorithms()
        {
            var asm = Assembly.GetAssembly(typeof(IAlgorithm))!;
            var algorithmTypes = asm.GetTypes()
                .Where(t => typeof(IAlgorithm).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToList();

            var list = new List<AlgorithmOption>();
            for (int i = 0; i < algorithmTypes.Count; i++)
            {
                if (Activator.CreateInstance(algorithmTypes[i]) is IAlgorithm instance)
                {
                    list.Add(new AlgorithmOption { Id = i + 1, Instance = instance });
                }
            }

            return list;
        }

        private static (string? Input, int ExitOption) ShowMenu(List<AlgorithmOption> algorithms)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Welcome to algorithm software by Ivan Segura");
            sb.AppendLine("Choose an algorithm to execute:\n");

            foreach (var a in algorithms)
            {
                sb.AppendLine($"{a.Id}) {a.Instance.Name}");
            }

            int exitOption = algorithms.Count + 1;
            sb.AppendLine($"{exitOption}) Exit");
            Console.WriteLine(sb.ToString());

            return (Console.ReadLine(), exitOption);
        }

        private static int? ValidateOption(string? input, int exit)
        {
            return !int.TryParse(input, out int parsed) ? 0 : parsed == exit ? null : parsed > 0 && parsed < exit ? parsed : 0;
        }
    }
}

public class AlgorithmOption
{
    public int Id { get; set; }
    public required IAlgorithm Instance { get; set; }
}