namespace MemoryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] words;

            try
            {
                words = File.ReadAllLines(@"Words.txt");
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine($"{e.Message}");
                Console.WriteLine("Make sure that the \"Words.txt\" file is located in the directory given above. Press any key.");
                Console.ReadKey();
                return;
            }


            bool keepPlaying = true;

            while (keepPlaying)
            {
                Console.Title = "Memory Game";
                Console.Clear();
                Console.WriteLine("Hello, welcome to Memory Game.");
                Console.WriteLine();
                Console.WriteLine("The objective of this game is to find matching words in a grid." +
                    "\nEach round you will be asked to select two fields." +
                    "\nIf these fields contain the same word, they will be uncovered." +
                    "\nIf not, they will be hidden again and you will lose one chance." +
                    "\nTo win, uncover all the words before you run out of chances.\nGood luck!\n\nPress any key to start.");
                Console.ReadKey();
                Console.Clear();

                // Start the main game loop
                GameLoop(words);

                // Handle repeat games
                bool invalidAnswer = true;

                while (invalidAnswer)
                {
                    Console.WriteLine();
                    Console.WriteLine("Would you like to play again? (Yes/No)");
                    string playAgain = Console.ReadLine().ToLower();
                    if (playAgain == "y" || playAgain == "yes")
                    {
                        invalidAnswer = false;
                    }
                    else if (playAgain == "n" || playAgain == "no")
                    {
                        invalidAnswer = false;
                        keepPlaying = false;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.Write("I don't understand.");
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Thank you for playing Memory Game. Have a nice day. Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Main game loop.
        /// </summary>
        /// <param name="words">Words to randomly select from.</param>
        static void GameLoop(string[] words)
        {
            // Setup for the game: get difficulty, create word array, create array for hiding/uncovering words
            // and calculate the biggest field on the game board (used for displaying a nice table as the board).
            (int rows, int columns, int chancesLeft, string difficulty) = Difficulty.GetDifficulty();
            string[,] gameBoard = Board.CreateGameBoard(words, rows, columns);
            int[,] whichUncovered = new int[rows, columns];
            int largestField = Helper.GetLengthOfLongestWord(gameBoard) + 2;

            while (chancesLeft >= 0)
            {
                // Get first guess and uncover it
                Board.DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
                (int firstGuessRow, int firstGuessColumn) = Guess.GetPlayerGuess(rows, columns, whichUncovered);
                whichUncovered[firstGuessRow, firstGuessColumn] = 1;

                // Get second guess and uncover it
                Board.DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
                (int secondGuessRow, int secondGuessColumn) = Guess.GetPlayerGuess(rows, columns, whichUncovered);
                whichUncovered[secondGuessRow, secondGuessColumn] = 1;

                // Compare guesses: leave uncovered if they match; hide if they don't match.
                Board.DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
                Console.WriteLine();
                if (gameBoard[firstGuessRow, firstGuessColumn] == gameBoard[secondGuessRow, secondGuessColumn])
                {
                    Console.WriteLine("Good job, the words match.");
                    Thread.Sleep(3000);
                }
                else
                {
                    Console.WriteLine("These words don't match.");
                    Thread.Sleep(3000);
                    whichUncovered[firstGuessRow, firstGuessColumn] = 0;
                    whichUncovered[secondGuessRow, secondGuessColumn] = 0;
                    chancesLeft --;
                }

                // Check if all words are uncovered.
                if (Helper.IsGameWon(whichUncovered))
                {
                    Console.Clear();
                    Console.WriteLine("Congratulations! You've uncovered all the words!");
                    Thread.Sleep(2000);
                    return;
                }
            }
            Console.Clear();
            Console.WriteLine("Sorry, you ran out of chances.");
        }
    }
}