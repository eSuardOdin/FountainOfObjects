namespace FountainGame.Game
{    
using FountainGame.Player;
using FountainGame.Map;
using FountainGame.Commands;
using FountainGame.Room;
using FountainGame.Senses;
using FountainGame.Io;
using FountainGame.Time;
    public class Game 
    {
        public Player Player {get; private set;}
        public Map Map {get; private set;}
        private ISense[] _senses;
        private TimeKeeper TimeKeep {get; init;}


        public Game() 
        {
            TimeKeep = new TimeKeeper(); 
            int mapSize = InputHandler.ChooseNumber("Please choose cavern size", 5, 8);
            Map = new Map(mapSize, 2);
            Player = new Player(Map.Spawn);
            Console.WriteLine(Player.Position.ToString()); // Print it at first. To refactor


            // Add all possible senses
            _senses = new ISense[] {
                new SenseLight(),
                new SenseAdjacentPit(),
                new SensFountain(),
            };
        }

        

        /// <summary>
        /// Wait for an input command and display the result
        /// </summary>
        /// <param name="command">The command to run</param>
        private void RunCommand(IPlayerCommand command) 
        {
            (bool isCommand, string message) = command.RunCommand(Player, Map);
            if(isCommand) 
            {
                Console.Clear();
                Console.WriteLine(Player.Position.ToString());
                if(message != "") Console.WriteLine(message); 
            }
            else
            {
                Console.Clear();
                Console.WriteLine(Player.Position.ToString());
                Console.WriteLine(message);
            }
            Map.UpdatePlayerPosition(Player.Position.X, Player.Position.Y);
        }

        /// <summary>
        /// Game loop
        /// </summary>
        public void RunGame() 
        {
            while(Player.IsAlive)
            {
                if(Map.IsFountainActive && Player.Position.Equals(Map.Spawn)) 
                {
                    Console.Clear();
                    Console.WriteLine("You escaped, congratulations.");
                    TimeKeep.Exit();
                    break;
                }

                // Senses check
                foreach (var sense in _senses)
                {
                    if(sense.CanSense(Map)) Console.WriteLine(sense.SenseDisplay());
                }

                IPlayerCommand command = InputHandler.GetPlayerMove().Key switch
                {
                    ConsoleKey.RightArrow => new East(),
                    ConsoleKey.LeftArrow => new West(),
                    ConsoleKey.UpArrow => new North(),
                    ConsoleKey.DownArrow => new South(),
                    ConsoleKey.E => new ActivateFountain(),
                };
                RunCommand(command);
                if (Map.GetRoomTypeAtLocation(Player.Position.X, Player.Position.Y) == RoomType.Pit) {
                    Console.WriteLine("You falled into a pit to your death.");
                    Player.Kill();
                    TimeKeep.Exit();
                }
            }
        }
    }
}


