namespace MemoryGame
{
    class Program
    {
        static void Main(string[] args)
        {
            bool keepPlaying = true;
            
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

            while (keepPlaying)
            {
                GameLoop(words);

                bool invalidAnswer = true;
                while (invalidAnswer)
                {
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
                        Console.Write("I don't understand. ");
                    }
                }
                

            }

            Console.WriteLine("Thank you for playing Memory Game. Have a nice day. Press any key to exit.");
            Console.ReadKey();
            

            

        }


        static (int, int) GetDifficulty()
        {
            bool chosenDifficulty = false;
            string difficulty = "";

            Console.Clear();
            Console.WriteLine("Hello, welcome to Memory Game.");
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
                return (2, 4);
            }
            else
            {
                return (4, 4);
            }
                
        }

        static string[,] CreateGameBoard(string[] words, int rows, int columns)
        {
            List<string> wordList = new List<string>();
            List<int> alreadyVisited = new List<int>();
            string[,] gameBoard = new string[rows, columns];
            

            Random random = new Random();

            while (wordList.Count < rows * columns)
            {
                int randomIndex = random.Next(words.Length);
                if (!alreadyVisited.Contains(randomIndex))
                {
                    alreadyVisited.Add(randomIndex);
                    wordList.Add(words[randomIndex]);
                    wordList.Add(words[randomIndex]);
                }
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
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
            int rows = gameBoard.GetLength(0);
            int columns = gameBoard.GetLength(1);

            Console.Write(" ");
            for (int i = 0; i < columns; i++)
            {
                Console.Write($" {i+1}");
            }
            Console.WriteLine();

                        
            for (int i = 0; i < rows; i++)
            {
                Console.Write($"{Convert.ToChar(i+65)} ");
                    for (int j = 0; j < columns; j++)
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

        
        static (int, int) GetPlayerGuess(int rows, int columns, int[,]whichUncovered)
        {
            bool validGuess = false;
            int convertedGuessRow = new int();
            int convertedGuessColumn = new int();



            while (!validGuess)
            {
                Console.Write("Take a guess: ");
                string playerGuess = Console.ReadLine().ToUpper();
                if (playerGuess == "")
                {
                    Console.WriteLine("You haven't chosen anything. Take a valid guess.");
                }
                else if (playerGuess.Length > 2)
                {
                    Console.WriteLine("Your guess is too long. Try again.");
                }
                else if (playerGuess.Length < 2)
                {
                    Console.WriteLine("Your guess is too short. Try again.");
                }
                else if (Char.IsLetter(playerGuess, 0) == false)
                {
                    Console.WriteLine("The first character needs to be a letter. Try again.");
                }
                else if (Char.IsDigit(playerGuess, 1) == false)
                {
                    Console.WriteLine("The second character needs to be a digit. Try again.");
                }
                else
                {
                    convertedGuessRow = playerGuess[0] - 65;
                    convertedGuessColumn = playerGuess[1] - 49;
                    if (convertedGuessRow < 0 || convertedGuessRow > rows - 1 || convertedGuessColumn < 0 || convertedGuessColumn > columns - 1)
                    {
                        Console.WriteLine("Your guess is out of bounds. Try again.");
                        continue;
                    }

                    else if (whichUncovered[convertedGuessRow, convertedGuessColumn] == 1)
                    {
                        Console.WriteLine("This word is already visible. Try again.");
                        continue;
                    }

                    validGuess = true;
                }
            }
            return (convertedGuessRow, convertedGuessColumn);

        }

        static bool IsGameWon(int[,] whichUncovered)
        {
            foreach (int item in whichUncovered)
            {
                if (item == 0)
                {
                    return false;
                }
            }
            return true;
        }

        static void GameLoop(string[] words)
        {
            
            // TODO: Enable the user to choose difficulty
            //(int rows, int columns) = GetDifficulty();

            int rows = 2;
            int columns = 2;


            string[,] gameBoard = CreateGameBoard(words, rows, columns);
            int[,] whichUncovered = new int[rows, columns];

            while (true)
            {
                Console.Clear();
                DisplayGameBoard(gameBoard, whichUncovered);
                (int firstGuessRow, int firstGuessColumn) = GetPlayerGuess(rows, columns, whichUncovered);
               
                whichUncovered[firstGuessRow, firstGuessColumn] = 1;

                Console.Clear();
                DisplayGameBoard(gameBoard, whichUncovered);
                (int secondGuessRow, int secondGuessColumn) = GetPlayerGuess(rows, columns, whichUncovered);
                whichUncovered[secondGuessRow, secondGuessColumn] = 1;

                Console.Clear();
                DisplayGameBoard(gameBoard, whichUncovered);

                if (gameBoard[firstGuessRow, firstGuessColumn] == gameBoard[secondGuessRow, secondGuessColumn])
                {
                    Console.WriteLine("Good job, the words match.");
                    Thread.Sleep(3000);
                }
                else
                {
                    Console.WriteLine("These words don't match. Try again.");
                    Thread.Sleep(3000);
                    whichUncovered[firstGuessRow, firstGuessColumn] = 0;
                    whichUncovered[secondGuessRow, secondGuessColumn] = 0;
                }

                if (IsGameWon(whichUncovered))
                {
                    Console.Clear();
                    Console.WriteLine("Congratulations! You've uncovered all the words!");
                    Thread.Sleep(2000);
                    return;
                }
                
            }
        }

    }
}