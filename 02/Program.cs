using System.Text.RegularExpressions;

namespace _02
{
    internal class Program
    {
        static StreamReader sr;
        static int sum = 0, powerSum = 0;
        static string line;
        static readonly Dictionary<string, int> maxCubes = new() { { "red", 12 }, { "green", 13 }, { "blue", 14 } };
        static Dictionary<string, int> cubeRecord = [];

        static void Main()
        {
            if (!File.Exists("input.txt"))
            {
                Console.WriteLine("Input file not found.");
                Console.ReadKey();
                return;
            }

            sr = new("input.txt");
            
            try
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] splitLine = line.Split(": ");

                    int gameNumber = int.Parse(splitLine.First().Split(' ').Last());
                    string[] subsets = splitLine.Last().Split("; ");

                    Console.Write($"Game {gameNumber}: ");

                    if (IsPossible(subsets))
                    {
                        sum += gameNumber;
                        Console.WriteLine($"possible (sum: {sum}) (power sum: {powerSum})");
                    }
                    else
                        Console.WriteLine($"impossible (power sum: {powerSum})");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                sr.Close();
            }

            Console.WriteLine($"\nSum: {sum}");
            Console.WriteLine($"Power sum: {powerSum}");

            Console.ReadKey();
        }

        static bool IsPossible(string[] subsets)
        {
            bool isPossible = true;
            cubeRecord["red"] = cubeRecord["green"] = cubeRecord["blue"] = 0;

            foreach (string subset in subsets)
            {
                (string color, int amount)[] cubes = subset
                    .Split(", ")
                    .Select(set => set.Split(" "))
                    .Select(set =>
                        (set.Last(), int.Parse(set.First()))
                    ).ToArray();

                foreach (var (color, amount) in cubes)
                {
                    if (amount > maxCubes[color])
                        isPossible = false;

                    if (cubeRecord.TryGetValue(color, out int value) is false || value < amount)
                        cubeRecord[color] = amount;
                }
            }

            powerSum += cubeRecord["red"] * cubeRecord["green"] * cubeRecord["blue"];

            return isPossible;
        }
    }
}
