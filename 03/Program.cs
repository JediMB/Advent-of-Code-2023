
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace _03
{
    internal class Program
    {
        static StreamReader sr;
        static string[] input;
        static readonly Dictionary<(int row, int column), bool> symbols = [];
        static int partNumberSum = 0;
        static int gearRatioSum = 0;

        static void Main()
        {
            if (!File.Exists("input.txt"))
            {
                Console.WriteLine("Input file not found.");
                Console.ReadKey();
                return;
            }

            sr = new("input.txt");

            input = sr.ReadToEnd().Split('\n');

            sr.Close();

            FindSymbols();

            FindAdjacentNumbers();

            Console.WriteLine($"\nPart Number Sum: {partNumberSum}");
            Console.WriteLine($"Gear Ratio Sum: {gearRatioSum}");

            Console.ReadKey();
        }

        static void FindSymbols()
        {
            for (int row = 0; row < input.Length; row++)
                for (int col = 0; col < input[row].Length; col++)
                    if (input[row][col] < '.' || input[row][col] == '/' || input[row][col] > '9')
                        symbols.Add((row, col), input[row][col] == '*');
        }

        static void FindAdjacentNumbers()
        {
            foreach (var symbol in symbols)
            {
                List<int> gearPartNumbers = [];

                for (int dRow = -1; dRow <= 1; dRow++)
                {
                    int iY = symbol.Key.row + dRow;
                    if (iY < 0 || iY >= input.Length) continue;

                    bool newNum = true;
                    for (int dCol = -1; dCol <= 1; dCol++)
                    {
                        if (dRow == 0 && dCol == 0)
                        {
                            newNum = true;
                            continue;
                        }

                        int iX = symbol.Key.column + dCol;
                        if (iX < 0 || iX >= input[iY].Length) continue;

                        if (input[iY][iX] < '0' || input[iY][iX] > '9')
                        {
                            newNum = true;
                            continue;
                        }

                        if (newNum)
                        {
                            int partNumber = FindPartNumber(iY, iX);

                            partNumberSum += partNumber;

                            if (symbol.Value)
                                gearPartNumbers.Add(partNumber);
                            
                            newNum = false;
                        }
                    }
                }

                if (gearPartNumbers.Count == 2)
                    gearRatioSum += (gearPartNumbers[0] * gearPartNumbers[1]);
            }
        }

        private static int FindPartNumber(int row, int col)
        {
            int startX = col, endX = col;

            int shiftedCol = col - 1;

            while(shiftedCol >= 0
                && input[row][shiftedCol] >= '0'
                && input[row][shiftedCol] <= '9')
            {
                startX = shiftedCol;
                shiftedCol--;
            }

            shiftedCol = col + 1;

            while(shiftedCol < input[row].Length
                && input[row][shiftedCol] >= '0'
                && input[row][shiftedCol] <= '9')
            {
                endX = shiftedCol;
                shiftedCol++;
            }

            return int.Parse(input[row][startX..(endX + 1)]);
        }
    }
}
