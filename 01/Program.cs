namespace _01
{
    internal class Program
    {
        static StreamReader sr;
        static int lineNumber = 0, sum = 0;
        static string line;
        static readonly string[] dictionary = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

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

            while ((line = sr.ReadLine()) != null)
            {
                lineNumber++;
                int firstIndex = -1, lastIndex = -1;
                int firstNum = 0, lastNum = 0;

                for (int i = 0; i < line.Length; i++)
                    if (IsNumber(line[i]))
                    {
                        firstIndex = i;
                        firstNum = line[i] - '0';
                        break;
                    }

                for (int i = line.Length - 1; i >= 0; i--)
                    if (IsNumber(line[i]))
                    {
                        lastIndex = i;
                        lastNum = line[i] - '0';
                        break;
                    }

                if (part == '2')
                    for (int i = 0; i < dictionary.Length; i++)
                    {
                        int wordFirstIndex = line.IndexOf(dictionary[i]);

                        if (wordFirstIndex >= 0)
                            if (wordFirstIndex < firstIndex)
                            {
                                firstIndex = wordFirstIndex;
                                firstNum = i;
                            }

                        int wordLastIndex = line.LastIndexOf(dictionary[i]);

                        if (wordLastIndex >= 0)
                            if (wordLastIndex > lastIndex)
                            {
                                lastIndex = wordLastIndex;
                                lastNum = i;
                            }
                    }

                sum += (10 * firstNum) + lastNum;

                Console.WriteLine($"Line {lineNumber:000}: {firstNum}{lastNum}");
            }

            Console.WriteLine($"\nSum: {sum}");

            sr.Close();

            Console.ReadKey();
        }

        static bool IsNumber(char c)
        {
            return c >= '0' && c <= '9';
        }
    }
}
