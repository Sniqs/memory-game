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
                Console.WriteLine("Make sure that the Words.txt file is located in the directory given above.");
                Console.ReadKey();
                return;
            }

            

        }
    }
}