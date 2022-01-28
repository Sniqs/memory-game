namespace MemoryGame
{
    internal class Board
    {

        /// <summary>
        /// Randomly selects items from "words" and constructs a 2d array to use as the game board.
        /// </summary>
        /// <param name="words">Words to randomly select from.</param>
        /// <param name="rows">The number of rows to generate.</param>
        /// <param name="columns">The number of columns to generate.</param>
        /// <returns>The game board as a 2D array.</returns>
        public static string[,] CreateGameBoard(string[] words, int rows, int columns)
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
        public static void DisplayGameBoard(string[,] gameBoard, int[,] whichUncovered, int chancesLeft, string difficulty, int largestField)
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
                Console.Write(Helper.PadString($"{i + 1}", largestField) + '|');
            }
            Console.WriteLine();

            // Display the rows.
            for (int i = 0; i < rows; i++)
            {
                Console.WriteLine(dashedLine);
                Console.Write($"{Convert.ToChar(i + 65)} |");
                for (int j = 0; j < columns; j++)
                {
                    if (whichUncovered[i, j] == 0)
                    {
                        Console.Write(Helper.PadString("X", largestField) + '|');
                    }
                    else
                    {
                        Console.Write(Helper.PadString($"{gameBoard[i, j]}", largestField) + '|');
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine(dashedLine);
            Console.WriteLine();
        }

    }
}
