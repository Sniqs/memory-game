namespace MemoryGame
{
    internal class Difficulty
    {

        /// <summary>
        /// Asks the player for their preferred difficulty and validates that they've chosen a supported one.
        /// </summary>
        /// <returns>The name of the difficulty as well as the number of rows, columns and chances for that difficulty.</returns>
        public static (int, int, int, string) GetDifficulty()
        {
            Console.WriteLine("Please select a difficulty (Easy, Hard or Nightmare).");

            // Ensure the player chooses a valid difficulty.
            bool chosenDifficulty = false;
            string difficulty = "";

            while (!chosenDifficulty)
            {
                difficulty = Console.ReadLine().ToLower();
                if (difficulty != "easy" & difficulty != "hard" & difficulty != "nightmare")
                {
                    Console.WriteLine("Please select one of the available difficulties (Easy, Hard or Nightmare).");
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
            else if (difficulty == "hard")
            {
                return (4, 4, 15, "Hard");
            }
            else
            {
                return (10, 6, 15, "Nightmare");
            }
        }

    }
}
