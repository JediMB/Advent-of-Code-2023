namespace _05
{
    internal class Program
    {
        static StreamReader sr;
        static long[] seeds;
        static readonly Dictionary<string, (string destType, List<(long src, long dest, long range)> srcToDest)> maps = [];
        static readonly string[] srcDestSeparators = ["-to-", " map:"];
        static List<long> threadedDestinations = [];

        static void Main()
        {
            if (!File.Exists("input.txt"))
            {
                Console.WriteLine("Input file not found.");
                Console.ReadKey();
                return;
            }

            sr = new("input.txt");

            char part;
            do
            {
                Console.WriteLine("Which part? 1 or 2?");
            }
            while ((part = Console.ReadKey().KeyChar) != '1' && part != '2');

            Console.WriteLine();

            string[] input = sr.ReadToEnd().Trim().Split("\n\n");

            sr.Close();

            seeds = input.First()
                .Split(": ").Last()
                .Split(' ')
                .Select(seed => long.Parse(seed))
                .ToArray();

            for (int mapNumber = 1; mapNumber < input.Length; mapNumber++)
            {
                string[] mapLines = input[mapNumber].Split('\n');
                string[] srcDest = mapLines.First()
                    .Split(srcDestSeparators, StringSplitOptions.RemoveEmptyEntries);
                (string source, string destination) = (srcDest.First(), srcDest.Last());

                maps[source] = (destination, new List<(long src, long dest, long range)>());

                for (int rangeLine = 1; rangeLine < mapLines.Length; rangeLine++)
                {
                    long[] rangeNumbers = mapLines[rangeLine].Split(' ').Select(n => long.Parse(n)).ToArray();

                    maps[source].srcToDest.Add((rangeNumbers[1], rangeNumbers[0], rangeNumbers[2]));
                }
            }

            int steps = part - '0';
            long lowestDestination = long.MaxValue;
            Stack<Thread> threads = [];
            for (int i = 0; i <= seeds.Length-steps; i+=steps)
            {
                int thread = 0;
                string type = "seed";
                long number = seeds[i];

                if (steps > 1)
                {
                    int start = i;
                    int range = i + 1;
                    threads.Push(new Thread(
                        () => FindLowestDestination(start, range)
                        ) { Name = $"Thread{thread}" }
                        );
                    threads.Peek().Start();
                }
                else
                {
                    while (maps.ContainsKey(type))
                    {
                        Console.Write($"Using {type} {number} ");
                        (type, number) = FindNext(type, number);
                        Console.WriteLine($"with {type} {number}.");
                    }

                    Console.WriteLine($"Found {type} number {number}");
                    Console.WriteLine();

                    if (number < lowestDestination && type == "location")
                        lowestDestination = number;
                }

                thread++;
            }

            if (steps > 1)
            {
                foreach (Thread thread in threads)
                {
                    thread?.Join();
                }

                lowestDestination = threadedDestinations.Min();
            }

            Console.WriteLine($"The lowest destination number is {lowestDestination}");
            Console.ReadKey();
        }

        static void FindLowestDestination(int seedIndex, int rangeIndex)
        {
            Console.WriteLine($"\nStarted seed index {seedIndex}");
            long lowestDestination = long.MaxValue;

            for (int rangeSteps = 0; rangeSteps < seeds[rangeIndex]; rangeSteps++)
            {
                string type = "seed";
                long number = seeds[seedIndex] + rangeSteps;

                while (maps.ContainsKey(type))
                {
                    (type, number) = FindNext(type, number);
                }

                if (number < lowestDestination && type == "location")
                    lowestDestination = number;
            }

            threadedDestinations.Add(lowestDestination);
            Console.WriteLine($"\nFinished seed index {seedIndex}. Found location {lowestDestination}.");
        }

        static (string type, long number) FindNext(string type, long number)
        {
            foreach (var (src, dest, range) in maps[type].srcToDest)
            {
                if (number >= src && number <= src + range)
                    return (maps[type].destType, dest + (number - src));
            }

            return (maps[type].destType, number);
        }
    }
}
