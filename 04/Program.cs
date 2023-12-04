namespace _04
{
    internal class Program
    {
        static StreamReader sr;
        static int pointSum = 0;

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

            string input = sr.ReadToEnd().Trim();

            sr.Close();

            (int[] winningNumbers, int[] cardNumbers, int copies)[] scratchCards = input.Split('\n')
                .Select(sc => sc.Split(": ").Last())
                .Select(sc => sc.Split(" | "))
                .Select(sc => (
                    sc.First().ExtractNumbers(),
                    sc.Last().ExtractNumbers(),
                    1
                ))
                .ToArray();

            for (int card = 0; card < scratchCards.Length; card++)
            {
                int matchingNumbers = 0;

                for (int number = 0; number < scratchCards[card].cardNumbers.Length; number++)
                {
                    if (scratchCards[card].winningNumbers.Contains(scratchCards[card].cardNumbers[number]))
                    {
                        matchingNumbers++;
                    }
                }

                if (part == '1')
                    pointSum += (matchingNumbers > 1)
                            ? (int)Math.Pow(2, matchingNumbers - 1)
                            : (matchingNumbers < 1)
                                ? 0
                                : 1;
                else
                {
                    Console.WriteLine($"Card {card + 1:0000}: {matchingNumbers} matches");

                    for (int copy = 1; copy <= scratchCards[card].copies; copy++)
                    {
                        for (int bonus = card + 1; bonus < scratchCards.Length && bonus <= card + matchingNumbers; bonus++)
                        {
                            scratchCards[bonus].copies++;
                        }
                    }
                }
            }

            if (part == '1')
                Console.WriteLine($"Point sum: {pointSum}");
            else
            {
                Console.WriteLine($"Total scratchcards: {scratchCards.Select(sc => sc.copies).Sum()}");
            }

            Console.ReadKey();
        }
    }

    public static class StringExtension
    {
        public static int[] ExtractNumbers(this string str)
        {
            return str.Trim()
                    .Replace("  ", " ").Split(" ")
                    .Select(num => int.Parse(num))
                    .ToArray();
        }
    }
}
