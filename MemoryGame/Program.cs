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


        static (int, int, int, string) GetDifficulty()
        {
            bool chosenDifficulty = false;
            string difficulty = "";

            Console.WriteLine("Please select a difficulty (Easy or Hard).");

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
                return (2, 4, 10, "Easy");
            }
            else
            {
                return (4, 4, 15, "Hard");
            }
                
        }

        static string[,] CreateGameBoard(string[] words, int rows, int columns)
        {
            List<string> wordList = new();
            List<int> alreadyVisited = new();
            string[,] gameBoard = new string[rows, columns];
            

            Random random = new();

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

        static void DisplayGameBoard(string[,] gameBoard, int[,] whichUncovered, int chancesLeft, string difficulty, int largestField)
        {
            int rows = gameBoard.GetLength(0);
            int columns = gameBoard.GetLength(1);
            string dashedLine = new string('-', (largestField * columns) + 3 + columns);

            Console.Clear();

            Console.WriteLine($"Difficulty: {difficulty}");
            if (chancesLeft > 0)
            {
                Console.WriteLine($"Chances left: {chancesLeft}");
            }
            else
            {
                Console.WriteLine("Last chance!");
            }

            Console.WriteLine();
                        

            Console.Write("  |");
            for (int i = 0; i < columns; i++)
            {
                Console.Write(PadString($"{i+1}", largestField) + '|');
            }
            Console.WriteLine();
            
            

                        
            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine(dashedLine);
                Console.Write($"{Convert.ToChar(i+65)} |");
                    for (int j = 0; j < columns; j++)
                {
                    if (whichUncovered[i,j] == 0)
                    {
                        Console.Write(PadString("X", largestField) + '|');
                    }
                    else
                    {
                        Console.Write(PadString($"{gameBoard[i, j]}", largestField) + '|');
                    }
                    

                }
                
                Console.WriteLine();
            }
            Console.WriteLine(dashedLine);
            Console.WriteLine();
        }

        
        static (int, int) GetPlayerGuess(int rows, int columns, int[,]whichUncovered)
        {
            bool validGuess = false;
            int convertedGuessRow = new int();
            int convertedGuessColumn = new int();



            while (!validGuess)
            {
                Console.Write("Select one of the fields by entering its coordinates (Example: A1): ");
                string playerGuess = Console.ReadLine().ToUpper();
                Console.WriteLine();

                if (playerGuess == "")
                {
                    Console.WriteLine("You haven't chosen anything. Try again.");
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
                        Console.WriteLine("These coordinates are outside of the board. Try again.");
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

        static int GetLengthOfLongestWord(string[,] gameBoard)
        {
            int length = 0;

            foreach(string word in gameBoard)
            {
                if(word.Length > length)
                {
                    length = word.Length;
                }
            }
            return length;
        }


        static string PadString (string stringToPad, int desiredLength)
        {
            
            int padding = (desiredLength - stringToPad.Length) / 2;
            string leftPadding = new String(' ', padding);
            stringToPad = leftPadding + stringToPad;
            string paddedString = stringToPad.PadRight(desiredLength, ' ');

            return paddedString;
        }

        static void GameLoop(string[] words)
        {

            // TODO: Enable the user to choose difficulty
            // (int rows, int columns, int chancesLeft, string difficulty) = GetDifficulty();

            int rows = 4;
            int columns = 4;
            int chancesLeft = 2;
            string difficulty = "Test";


            string[,] gameBoard = CreateGameBoard(words, rows, columns);
            int largestField = GetLengthOfLongestWord(gameBoard) + 2;
            int[,] whichUncovered = new int[rows, columns];
            

            while (chancesLeft >= 0)
            {
                
                DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
                (int firstGuessRow, int firstGuessColumn) = GetPlayerGuess(rows, columns, whichUncovered);
                whichUncovered[firstGuessRow, firstGuessColumn] = 1;

                DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
                (int secondGuessRow, int secondGuessColumn) = GetPlayerGuess(rows, columns, whichUncovered);
                whichUncovered[secondGuessRow, secondGuessColumn] = 1;

                DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
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
                    chancesLeft -= 1;
                }

                if (IsGameWon(whichUncovered))
                {
                    Console.Clear();
                    Console.WriteLine("Congratulations! You've uncovered all the words!");
                    Thread.Sleep(2000);
                    return;
                }
                
            }
            Console.WriteLine("Sorry, you ran out of chances.");
        }

    }
}