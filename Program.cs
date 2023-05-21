

Player player = new Player();
Map map = new Map(new Position(4,2), new Position(5,3), 10, 8);
Game game = new Game(player, map);

game.RunGame();


public class Game 
{
    public Player Player {get; private set;}
    private Map Map {get; set;}
    private ISense[] _senses;



    public Game(Player player, Map map) 
    {
        Player = player;
        Map = map;


        // Add all possible senses
        _senses = new ISense[] {
            new SenseLight(),
            new SenseAdjacentPit(),
        };
    }

    /// <summary>
    /// Returns the type of room in the location.
    /// </summary>
    /// <returns>Type of the current room</returns>
    public RoomType GetRoomTypeAtLocation(int x, int y) 
    {
        return Map.Rooms[x, y].Type;
    } 

    /// <summary>
    /// Checks if a Room type is adjacent to the player's position
    /// </summary>
    /// <param name="type">The type of room checked</param>
    /// <returns>True if adjacent</returns>
    public bool IsRoomTypeAdjacent(RoomType type) 
    {
        (int x, int y) = (Player.Position.X, Player.Position.Y);
        return (
            GetRoomTypeAtLocation(x + 1, y) == type ||
            GetRoomTypeAtLocation(x - 1, y) == type ||
            GetRoomTypeAtLocation(x, y + 1) == type ||
            GetRoomTypeAtLocation(x, y - 1) == type ||
            GetRoomTypeAtLocation(x + 1, y + 1) == type ||
            GetRoomTypeAtLocation(x - 1, y + 1) == type ||
            GetRoomTypeAtLocation(x + 1, y - 1) == type ||
            GetRoomTypeAtLocation(x - 1, y - 1) == type
        );
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
            IPlayerCommand command = InputHandler.GetPlayerMove().Key switch
            {
                ConsoleKey.RightArrow => new East(),
                ConsoleKey.LeftArrow => new West(),
                ConsoleKey.UpArrow => new North(),
                ConsoleKey.DownArrow => new South(),
                ConsoleKey.E => new ActivateFountain(),
            };
            RunCommand(command);

            // Senses check
            foreach (var sense in _senses)
            {
                if(sense.CanSense(this)) Console.WriteLine(sense.SenseDisplay());
            }

            if (GetRoomTypeAtLocation(Player.Position.X, Player.Position.Y) == RoomType.Pit) {
                Console.WriteLine("You falled into a pit to your death.");
                Player.Kill();
            }
        }
    }
}

public class Map 
{
    public Room[,] Rooms {get;}
    public Position Fountain {get; init;}
    public Position Spawn {get; init;}
    public bool IsFountainActive {get; private set;}

    public Map(Position spawn, Position fountain, int width, int length)
    {
        IsFountainActive = false;
        Spawn = new Position(spawn.X, spawn.Y);
        Fountain = new Position(fountain.X, fountain.Y);
        Rooms = new Room[width,length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                RoomType type;
                if (x == 0 || x == width - 1 || y == 0 || y == length - 1) type = RoomType.OffMap;
                else if(x == spawn.X && y == spawn.Y) type = RoomType.Spawn;
                else if(x == fountain.X && y == fountain.Y) type = RoomType.Fountain;
                else if(x == 5 && y == 5) type = RoomType.Pit;
                else type = RoomType.Normal;

                Rooms[x,y] = new Room(type);

            } 
        }
        Console.WriteLine("Enter a key to exit map generation...");
    }

    public void SwitchFountainOn() 
    {
        IsFountainActive = true;
    }
}

public record Position(int X, int Y);

public class Player 
{
    public bool IsAlive {get; private set;}
    public Position Position {get; set;}


    public Player() {
        IsAlive = true;
        Position = new Position(1,1);
    }

    /// <summary>
    /// Kills the player
    /// </summary>
    public void Kill() 
    {
        IsAlive = false;
    }

}

public class Room 
{
    public RoomType Type {get; private init;}
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
        // if(player.Position.Y < map.Rooms.GetLength(1) - 1) 
        if(map.Rooms[player.Position.X, player.Position.Y+1].Type != RoomType.OffMap)
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
        // if(player.Position.Y > 0)
        if(map.Rooms[player.Position.X, player.Position.Y-1].Type != RoomType.OffMap) 
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
        // if(player.Position.X < map.Rooms.GetLength(0) - 1)
        if(map.Rooms[player.Position.X+1, player.Position.Y].Type != RoomType.OffMap) 
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
        // if(player.Position.X > 0)  
        if(map.Rooms[player.Position.X-1, player.Position.Y].Type != RoomType.OffMap)
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
    public bool CanSense(Game game);
    public string SenseDisplay();
}

public class SenseLight : ISense
{
    public bool CanSense(Game game) {
        return game.GetRoomTypeAtLocation(game.Player.Position.X, game.Player.Position.Y) == RoomType.Spawn;
    }

    public string SenseDisplay() {
        return "You can sense the heat of sunlight, this must be the cavern entrance.";
    }
}

public class SenseAdjacentPit : ISense
{
    public bool CanSense(Game game) {
        return game.IsRoomTypeAdjacent(RoomType.Pit);
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
}

public enum RoomType {Spawn, Fountain, Normal, Pit, OffMap};