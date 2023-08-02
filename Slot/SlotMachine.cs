using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;

namespace CryptoRandom.Slot
{
    internal class SlotMachine
    {
        private const int Row = 3;
        private const int Column = 5;

        private readonly int _symbolsCounts;
        private readonly string[] _symbols;
        private readonly RNGCryptoServiceProvider _cryptoRandomGenerator;

        public int Income { get; private set; } = 0;
        public int AllBet { get; private set; } = 0;
     
        public SlotMachine(string[] symbols)
        {
            _symbols = symbols;
            _symbolsCounts = symbols.Length;
            _cryptoRandomGenerator = new RNGCryptoServiceProvider();
        }
  
        public void Spin(int bet)
        {
            Console.WriteLine("\nSpinning the slot machine...");
            //Thread.Sleep(1000);

            Dictionary<(int, int), string> matrix = GenerateSymbolMatrix();

            DisplayMatrix(matrix);

            Dictionary<string, int> symbolCounts = CountSymbols(matrix);

            bool isTopCasino = CheckTopCasinoLogic(symbolCounts);

            Console.WriteLine();

            if (isTopCasino)
            {
                Console.WriteLine("Congratulations! You hit the Top Casino Jackpot!");
                Income -= bet;
            }
            else
            {
                Console.WriteLine("Sorry, try again.");
                Income += bet;
            }
            AllBet += bet;
        }

        private Dictionary<(int, int), string> GenerateSymbolMatrix()
        {
            Dictionary<(int, int), string> matrix = new Dictionary<(int, int), string>();
            for (int row = 0; row < Row; ++row)
            {
                for (int col = 0; col < Column; ++col)
                {
                    byte[] randomBytes = new byte[1];
                    _cryptoRandomGenerator.GetBytes(randomBytes);
                    int symbolIndex = randomBytes[0] % _symbolsCounts;
                    matrix[(row, col)] = _symbols[symbolIndex];
                }
            }
            return matrix;
        }

        private void DisplayMatrix(Dictionary<(int, int), string> matrix)
        {
            for (int row = 0; row < Row; ++row)
            {
                for (int col = 0; col < Column; ++col)
                {
                    PrintSymbolColored(matrix[(row, col)]);
                }
                Console.WriteLine();
            }
        }

        private Dictionary<string, int> CountSymbols(Dictionary<(int, int), string> matrix)
        {
            Dictionary<string, int> symbolCounts = new Dictionary<string, int>();
            foreach (var kvp in matrix)
            {
                string symbol = kvp.Value;
                if (symbolCounts.ContainsKey(symbol))
                {
                    symbolCounts[symbol]++;
                }
                else
                {
                    symbolCounts[symbol] = 1;
                }
            }
            return symbolCounts;
        }

        private bool CheckTopCasinoLogic(Dictionary<string, int> symbolCounts)
        {
            int count = 0;
            foreach (var kvp in symbolCounts)
            {
                if (Math.Abs( kvp.Value - Column)== Row)
                {
                    ++count;
                    
                }
            }
            if(count > symbolCounts.Count - 3) 
            {
                return true;
            }
            return false;
        }

        private void PrintSymbolColored(string symbol)
        {
            ConsoleColor originalForeground = Console.ForegroundColor;
            ConsoleColor originalBackground = Console.BackgroundColor;

            Console.ForegroundColor = GetRandomConsoleColor();
            Console.BackgroundColor = GetRandomConsoleColor();

            Console.Write($" {symbol} ");

            Console.ForegroundColor = originalForeground;
            Console.BackgroundColor = originalBackground;
        }

        private ConsoleColor GetRandomConsoleColor()
        {
            Array values = Enum.GetValues(typeof(ConsoleColor));
            byte[] randomBytes = new byte[4];
            _cryptoRandomGenerator.GetBytes(randomBytes);
            int randomInt = BitConverter.ToInt32(randomBytes, 0);
            int index = Math.Abs(randomInt) % values.Length;
            return (ConsoleColor)values.GetValue(index);
        }
    }
}
