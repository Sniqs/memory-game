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
                Console.WriteLine("Make sure that the \"Words.txt\" file is located in the directory given above. Press any key.");
                Console.ReadKey();
                return;
            }

            // TODO: Enable the user to choose difficulty
            //int numberOfWords = GetDifficulty();
            int numberOfWords = 8;


            string[,] gameBoard = CreateGameBoard(words, numberOfWords);
            int[,] whichUncovered = new int[numberOfWords / 4, 4];

            

            DisplayGameBoard(gameBoard, whichUncovered);
        

        }


        static int GetDifficulty()
        {
            bool chosenDifficulty = false;
            string difficulty = "";

            Console.Clear();
            Console.WriteLine("Hello, Dave, welcome to Memory Game.");
            Console.WriteLine("Please select a difficulty (Easy or Hard)");


                // Ensure the player chooses either Easy or Hard.
                while (!chosenDifficulty)
                {
                    difficulty = Console.ReadLine().ToLower();
                    if (difficulty != "easy" & difficulty != "hard")
                    {
                        Console.WriteLine("Please select one of the available difficulties (Easy or Hard).");
                        continue;
                    }
                    else
                    {
                        chosenDifficulty = true;
                    }

                }
            if (difficulty == "easy")
            {
                return 8;
            }
            else
            {
                return 16;
            }
                
        }

        static string[,] CreateGameBoard(string[] words, int numberOfWords)
        {
            List<string> wordList = new List<string>();
            List<int> alreadyVisited = new List<int>();
            string[,] gameBoard = new string[numberOfWords / 4, 4];

            Random random = new Random();

            while (wordList.Count < numberOfWords)
            {
                int randomIndex = random.Next(words.Length);
                if (!alreadyVisited.Contains(randomIndex))
                {
                    alreadyVisited.Add(randomIndex);
                    wordList.Add(words[randomIndex]);
                    wordList.Add(words[randomIndex]);
                }
            }

            for (int i = 0; i < numberOfWords / 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int randomIndex = random.Next(wordList.Count);
                    gameBoard[i, j] = wordList[randomIndex];
                    wordList.RemoveAt(randomIndex);

                }
            }
            return gameBoard;
        }

        static void DisplayGameBoard(string[,] gameBoard, int[,] whichUncovered)
        {
            int columns = gameBoard.GetLength(0);
            int rows = gameBoard.GetLength(1);

            Console.Write(" ");
            for (int i = 1; i < 5; i++)
            {
                Console.Write($" {i}");
            }
            Console.WriteLine();

                        
            for (int i = 0; i < columns; i++)
            {
                Console.Write($"{Convert.ToChar(i+65)} ");
                    for (int j = 0; j < rows; j++)
                {
                    if (whichUncovered[i,j] == 0)
                    {
                        Console.Write("X ");
                    }
                    else
                    {
                        Console.Write($"{gameBoard[i, j]} ");
                    }
                    

                }
                Console.WriteLine();
            }
        }

    }
}