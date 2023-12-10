namespace _06
{
    internal class Program
    {
        static StreamReader sr;

        static void Main()
        {
            if (!File.Exists("input.txt"))
            {
                Console.WriteLine("Input file not found.");
                Console.ReadKey();
                return;
            }

            int part;
            do
            {
                Console.WriteLine("Which part? 1 or 2?");
            }
            while ((part = Console.ReadKey().KeyChar - '0') != 1 && part != 2);

            Console.WriteLine("\n");

            sr = new("input.txt");

            string[] input = sr.ReadToEnd().Split('\n', StringSplitOptions.RemoveEmptyEntries);

            sr.Close();
            
            ulong[] times = ExtractNumbers(input.First(), part);
            ulong[] distances = ExtractNumbers(input.Last(), part);

            if (times.Length != distances.Length)
                return;

            ulong solutionProduct = 1;
            for (int race = 0; race < times.Length; race++)
            {
                decimal midpoint = times[race] * 0.5m;

                for (ulong charge = 1; charge < midpoint; charge++)
                    if (charge * (times[race] - charge) > distances[race])
                    {
                        solutionProduct *= (ulong)((midpoint - charge) * 2) + 1;
                        break;
                    }
            }

            Console.WriteLine($"Ways to beat the record{ (part == 1 ? "s multiplied" : null) }: {solutionProduct}");

            Console.ReadKey();
        }

        static ulong[] ExtractNumbers(string input, int part)
        {
            input = input
                .Split(':', StringSplitOptions.TrimEntries)
                .Last();

            if (part == 2)
                return [ ulong.Parse(input.Replace(" ", "")) ];

            return input
                .Split(' ', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(num => ulong.Parse(num)).ToArray();
        }
    }
}
