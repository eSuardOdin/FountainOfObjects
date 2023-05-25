using FountainGame.Player;

Game game = new Game();

game.RunGame();


public class Game 
{
    public Player Player {get; private set;}
    public Map Map {get; private set;}
    private ISense[] _senses;



    public Game() 
    {
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
            }
        }
    }
}

public class Room 
{
    public RoomType Type {get; set;}
    public Room(RoomType type) {
        Type = type;
    }
}


// Commands
public interface IPlayerCommand
{
    public (bool, string) RunCommand(Player player, Map map);
}


public class North : IPlayerCommand
{
    public (bool, string) RunCommand(Player player, Map map)
    {
        if(player.Position.Y < map.Rooms.GetLength(1) - 1) 
        // if(map.Rooms[player.Position.X, player.Position.Y+1].Type != RoomType.OffMap)
        {
            player.Position = new Position(player.Position.X, player.Position.Y+1);
            return (true, "");
        }

        return (false, "You're facing a wall.");
    }
}

public class South : IPlayerCommand
{
    public (bool, string) RunCommand(Player player, Map map)
    {
        if(player.Position.Y > 0)
        // if(map.Rooms[player.Position.X, player.Position.Y-1].Type != RoomType.OffMap) 
        {
            player.Position = new Position(player.Position.X, player.Position.Y-1);
            return (true, "");

        }
        return (false, "You're facing a wall.");
    }
}

public class East : IPlayerCommand
{
    public (bool, string) RunCommand(Player player, Map map)
    {
        if(player.Position.X < map.Rooms.GetLength(0) - 1)
        // if(map.Rooms[player.Position.X+1, player.Position.Y].Type != RoomType.OffMap) 
        {
            player.Position = new Position(player.Position.X+1, player.Position.Y);
            return (true, "");
        }
        return (false, "You're facing a wall.");
    }
}

public class West : IPlayerCommand
{
    public (bool, string) RunCommand(Player player, Map map)
    {
        if(player.Position.X > 0)  
        // if(map.Rooms[player.Position.X-1, player.Position.Y].Type != RoomType.OffMap)
        {
            player.Position = new Position(player.Position.X-1, player.Position.Y);
            return (true, "");
        }

        return (false, "You're facing a wall.");
    }
}

public class ActivateFountain : IPlayerCommand
{
    public (bool, string) RunCommand(Player player, Map map)
    {
        if(!map.IsFountainActive && player.Position.X == map.Fountain.X && player.Position.Y == map.Fountain.Y) {
            map.SwitchFountainOn();
            return (true, "You found a lever and pull it to trigger the fountain.\nYou can hear water flows loudly now.\nTime to leave this place, go back to the entrance...");
        }
        return (false, "No fountain to activate");
    }
}




// Senses
public interface ISense
{
    public bool CanSense(Map map);
    public string SenseDisplay();
}

public class SenseLight : ISense
{
    public bool CanSense(Map map) {
        return map.GetRoomTypeAtLocation(map.PlayerPosition.X, map.PlayerPosition.Y) == RoomType.Spawn;
    }

    public string SenseDisplay() {
        return "You can sense the heat of sunlight, this must be the cavern entrance.";
    }
}

public class SensFountain : ISense
{
    public bool CanSense(Map map) {
        return map.GetRoomTypeAtLocation(map.PlayerPosition.X, map.PlayerPosition.Y) == RoomType.Fountain;
    }

    public string SenseDisplay() {
        return "You hear water flowing.";
    }
}

public class SenseAdjacentPit : ISense
{
    public bool CanSense(Map map) {
        return map.IsRoomTypeAdjacent(RoomType.Pit, map.PlayerPosition);
    }

    public string SenseDisplay() {
        return "You hear the wind blowing and noise echoing. There must be a pit somewhere near.";
    }
}




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
public enum RoomType {Spawn, Fountain, Normal, Pit, OffMap};

