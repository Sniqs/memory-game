namespace MemoryGame
{
    internal class Guess
    {

        /// <summary>
        /// Gets and validates a player's guess.
        /// </summary>
        /// <param name="rows">Number of rows on the game board.</param>
        /// <param name="columns">Number of columns on the game board.</param>
        /// <param name="whichUncovered">2D array of the same size as the game board. Used to check if a word is already visible.</param>
        /// <returns>The player's guess as coordinates of a 2D array.</returns>
        public static (int, int) GetPlayerGuess(int rows, int columns, int[,] whichUncovered)
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

    }
}
