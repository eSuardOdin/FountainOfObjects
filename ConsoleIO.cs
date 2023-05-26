namespace FountainGame.Io
{
    
    public static class InputHandler {

        public static ConsoleKeyInfo GetPlayerMove() {
            ConsoleKeyInfo key;

            do {
                Console.WriteLine("Please enter next direction with arrow key...");
                key = Console.ReadKey();
            } while(
                key.Key != ConsoleKey.E &&
                key.Key != ConsoleKey.RightArrow &&
                key.Key != ConsoleKey.LeftArrow &&
                key.Key != ConsoleKey.UpArrow &&
                key.Key != ConsoleKey.DownArrow
            );
            return key;
        }

        public static int ChooseNumber(string prompt, int min, int max) 
        {
            int res;
            Console.Write($"{prompt} ({min}-{max}) :");
            while(!int.TryParse(Console.ReadLine(), out res) || (res < min || res > max))
            {
                Console.WriteLine($"Please enter a number between {min}-{max}");
            }
            return res;
        }

    }


    public static class OutputHelper 
    {
        public static void ConsolePrint(string text, ConsoleColor textColor, ConsoleColor bgColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = bgColor;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
