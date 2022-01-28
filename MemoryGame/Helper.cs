namespace MemoryGame
{
    internal class Helper
    {

        /// <summary>
        /// Checks if the conditions required to win the game are fulfilled.
        /// </summary>
        /// <param name="whichUncovered">2D array containing 0 for hidden words and 1 for uncovered words.</param>
        /// <returns>True if all values are equal to 1. Otherwise, false.</returns>
        public static bool IsGameWon(int[,] whichUncovered)
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
        public static int GetLengthOfLongestWord(string[,] gameBoard)
        {
            int length = 0;

            foreach (string word in gameBoard)
            {
                if (word.Length > length)
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
        public static string PadString(string stringToPad, int desiredLength)
        {
            // Calculate how much padding is needed on the left and right of the string.
            int padding = (desiredLength - stringToPad.Length) / 2;

            string leftPadding = new String(' ', padding);
            stringToPad = leftPadding + stringToPad;
            string paddedString = stringToPad.PadRight(desiredLength, ' ');

            return paddedString;
        }

    }
}
