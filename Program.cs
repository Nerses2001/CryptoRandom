using CryptoRandom.Slot;
using System;

namespace CryptoRandom
{
    internal class Program
    {
        static void Main()
        {
            Random random = new Random();
            int[] bets = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

            Console.WriteLine("Welcome to the Secure Slot Machine Game!");

            string[] symbols = { "Cherry", "Lemon", "Orange", "Plum", "Bell", "Bar", "Seven" };
            SlotMachine slotMachine = new SlotMachine(symbols);

         /*   while (true)
            {
                Console.WriteLine("\nPress Enter to spin or type 'exit' to quit...");
                string input = Console.ReadLine();

                if (input.ToLower() == "exit")
                {
                    Console.WriteLine("Exiting the game. Goodbye!");
                    break;
                }

                slotMachine.Spin();
            }*/

            for(int i = 0; i < 30000; ++i) 
            {
                slotMachine.Spin(bets[random.Next(bets.Length)]);
            }
            Console.WriteLine("Incom is {0}, All bet is {1}",slotMachine.Income, slotMachine.AllBet);
            Console.ReadKey();
        }
    }
}
