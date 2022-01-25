using System;


namespace MemoryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] words;
            try
            {
                words = System.IO.File.ReadAllLines(@"Words.txt");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"{e.Message}");
                Console.WriteLine("Make sure that the \"Words.txt\" file is located in the directory given above.");
                Console.ReadKey();
                return;
            }

            string gameDifficulty = GetDifficulty();
            Console.WriteLine($"Chosen difficulty: {gameDifficulty}");



        static string GetDifficulty()
        {
            bool chosenDifficulty = false;
            string difficulty = "";

            Console.Clear();
            Console.WriteLine("Welcome to Memory Game");
            Console.WriteLine("Please select a difficulty (Easy or Hard)");
            // string difficulty = Console.ReadLine().ToLower();

                while (!chosenDifficulty)
                {
                    difficulty = Console.ReadLine().ToLower();
                    if (difficulty != "easy" & difficulty != "hard")
                    {
                        Console.WriteLine("Please select one of the available difficulties (Easy or Hard)");
                        continue;
                    }
                    else
                    {
                        chosenDifficulty = true;
                    }

                }
                return difficulty;

        }
            


        }
    }
}