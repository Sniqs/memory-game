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
                        Console.Write("I don't understand. ");
                    }
                }
            }
            Console.Clear();
            Console.WriteLine("Thank you for playing Memory Game. Have a nice day. Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Asks the player for their preferred difficulty and validates that they've chosen a supported one.
        /// </summary>
        /// <returns>The name of the difficulty as well as the number of rows, columns and chances for that difficulty.</returns>
        static (int, int, int, string) GetDifficulty()
        {
            Console.WriteLine("Please select a difficulty (Easy or Hard).");

            // Ensure the player chooses a valid difficulty.
            bool chosenDifficulty = false;
            string difficulty = "";

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

            // Return rows, columns, chances and name of the chosen difficulty
            if (difficulty == "easy")
            {
                return (2, 4, 10, "Easy");
            }
            else
            {
                return (4, 4, 15, "Hard");
            }  
        }

        /// <summary>
        /// Randomly selects items from "words" and constructs a 2d array to use as the game board.
        /// </summary>
        /// <param name="words">Words to randomly select from.</param>
        /// <param name="rows">The number of rows to generate.</param>
        /// <param name="columns">The number of columns to generate.</param>
        /// <returns>The game board as a 2D array.</returns>
        static string[,] CreateGameBoard(string[] words, int rows, int columns)
        {
            List<string> wordList = new();
            List<int> alreadyVisited = new();
            Random random = new();
            string[,] gameBoard = new string[rows, columns];
            
            // Randomly select words, making sure that the same word isn't selected twice.
            while (wordList.Count < rows * columns)
            {
                int randomIndex = random.Next(words.Length);
                if (!alreadyVisited.Contains(randomIndex))
                {
                    alreadyVisited.Add(randomIndex);

                    // Add the selected word to a list twice (because the board needs two instances of the same word in two different fields).
                    wordList.Add(words[randomIndex]);
                    wordList.Add(words[randomIndex]);
                }
            }

            // Create the actual game board as a 2D array by randomly assigning words from wordList to specific fields.
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    int randomIndex = random.Next(wordList.Count);
                    gameBoard[i, j] = wordList[randomIndex];

                    // Remove the word from wordList to make sure that every item is only placed in the array once.
                    wordList.RemoveAt(randomIndex);
                }
            }
            return gameBoard;
        }

        /// <summary>
        /// Displays the game board on the screen.
        /// </summary>
        /// <param name="gameBoard">2D array of words.</param>
        /// <param name="whichUncovered">2D array of the same size as gameBoard. Used to check if a word from gameBoard should be visible.</param>
        /// <param name="chancesLeft">Current number of chances left.</param>
        /// <param name="difficulty">Current difficulty.</param>
        /// <param name="largestField">Number representing the largest field required (all fields on the board will be of this length).</param>
        static void DisplayGameBoard(string[,] gameBoard, int[,] whichUncovered, int chancesLeft, string difficulty, int largestField)
        {
            int rows = gameBoard.GetLength(0);
            int columns = gameBoard.GetLength(1);

            // Create a dashed line that will span the whole width of the game board.
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
            Console.WriteLine();

            // Display the header with numbers.
            Console.Write("  |");
            for (int i = 0; i < columns; i++)
            {
                Console.Write(PadString($"{i+1}", largestField) + '|');
            }
            Console.WriteLine();
            
            // Display the rows.
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

        /// <summary>
        /// Gets and validates a player's guess.
        /// </summary>
        /// <param name="rows">Number of rows on the game board.</param>
        /// <param name="columns">Number of columns on the game board.</param>
        /// <param name="whichUncovered">2D array of the same size as the game board. Used to check if a word is already visible.</param>
        /// <returns>The player's guess as coordinates of a 2D array.</returns>
        static (int, int) GetPlayerGuess(int rows, int columns, int[,] whichUncovered)
        {
            bool validGuess = false;
            int convertedGuessRow = new int();
            int convertedGuessColumn = new int();

            while (!validGuess)
            {
                Console.WriteLine();
                Console.Write("Select one of the fields by entering its coordinates (Example: A1): ");
                string playerGuess = Console.ReadLine().ToUpper();
                Console.WriteLine();

                // Ensure the player's guess is a valid field.
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
                    // Convert the player's guess into coordinates of the game board 2D array.
                    convertedGuessRow = playerGuess[0] - 65;
                    convertedGuessColumn = playerGuess[1] - 49;

                    // Check to make sure the guess is within bounds of the game board.
                    if (convertedGuessRow < 0 || convertedGuessRow > rows - 1 || convertedGuessColumn < 0 || convertedGuessColumn > columns - 1)
                    {
                        Console.WriteLine("These coordinates are outside of the board. Try again.");
                        continue;
                    }

                    // Check to see if the field contains an already uncovered word.
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

        /// <summary>
        /// Checks if the conditions required to win the game are fulfilled.
        /// </summary>
        /// <param name="whichUncovered">2D array containing 0 for hidden words and 1 for uncovered words.</param>
        /// <returns>True if all values are equal to 1. Otherwise, false.</returns>
        static bool IsGameWon(int[,] whichUncovered)
        {
            // Iterate through whichUncovered. The game is won if all values are equal to 1.
            foreach (int item in whichUncovered)
            {
                if (item == 0)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Calculates the length of the longest string in an array.
        /// </summary>
        /// <param name="gameBoard">2D array of words.</param>
        /// <returns>Length of the longest string.</returns>
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

        /// <summary>
        /// Pads an input string with spaces on both sides so the original string is centered.
        /// </summary>
        /// <param name="stringToPad">Input string that needs to be padded.</param>
        /// <param name="desiredLength">Required number of characters in the output string.</param>
        /// <returns>Input string padded on both sides to the desired length.</returns>
        static string PadString (string stringToPad, int desiredLength)
        {
            // Calculate how much padding is needed on the left and right of the string.
            int padding = (desiredLength - stringToPad.Length) / 2;

            string leftPadding = new String(' ', padding);
            stringToPad = leftPadding + stringToPad;
            string paddedString = stringToPad.PadRight(desiredLength, ' ');

            return paddedString;
        }

        /// <summary>
        /// Main game loop.
        /// </summary>
        /// <param name="words">Words to randomly select from.</param>
        static void GameLoop(string[] words)
        {
            // Setup for the game: get difficulty, create word array, create array for hiding/uncovering words
            // and calculate the biggest field on the game board (used for displaying a nice table as the board).
            (int rows, int columns, int chancesLeft, string difficulty) = GetDifficulty();
            string[,] gameBoard = CreateGameBoard(words, rows, columns);
            int[,] whichUncovered = new int[rows, columns];
            int largestField = GetLengthOfLongestWord(gameBoard) + 2;

            while (chancesLeft >= 0)
            {
                // Get first guess and uncover it
                DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
                (int firstGuessRow, int firstGuessColumn) = GetPlayerGuess(rows, columns, whichUncovered);
                whichUncovered[firstGuessRow, firstGuessColumn] = 1;

                // Get second guess and uncover it
                DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
                (int secondGuessRow, int secondGuessColumn) = GetPlayerGuess(rows, columns, whichUncovered);
                whichUncovered[secondGuessRow, secondGuessColumn] = 1;

                // Compare guesses: leave uncovered if they match; hide if they don't match.
                DisplayGameBoard(gameBoard, whichUncovered, chancesLeft, difficulty, largestField);
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
                    chancesLeft -= 1;
                }

                // Check if all words are uncovered.
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