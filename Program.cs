/*
    Player navigates in a grid based map while in darkness
    Relies on senses (Smell, Hearing) to determine what room they are in
    and what danger there is


    Game flow : 
        Player is told what they can sens in the dark (see, hear, smell)
        Get chance to type action
        Action checked and executed, loop.

    
    
Découpage des tâches :

    - Avoir un joueur avec une position
    - Avoir des commandes pour le déplacer


*/


Player player = new Player();
Map map = new Map(new Position(4,2), new Position(5,3), 7, 4);
Game game = new Game(player, map);

game.RunGame();


public class Game 
{
    private Player Player {get; set;}
    private Map Map {get; set;}

    public Game(Player player, Map map) 
    {
        Player = player;
        Map = map;
    }
    private void RunCommand(IPlayerCommand command) 
    {
        if(command.RunCommand(Player)) 
        {
            Console.Clear();
            Console.WriteLine("Game.RunCommand()");
            Console.WriteLine(Player.Position.ToString());
        }
    }

    public void RunGame() 
    {
        while(Player.IsAlive)
        {
            IPlayerCommand command = InputHandler.GetPlayerMove().Key switch
            {
                ConsoleKey.RightArrow => new East(),
                ConsoleKey.LeftArrow => new West(),
                ConsoleKey.UpArrow => new North(),
                ConsoleKey.DownArrow => new South(),
            };
            Console.WriteLine(command);
            RunCommand(command);
        }
    }
}

public class Map 
{
    Room[,] Rooms;
    public Map(Position spawn, Position fountain, int width, int length)
    {
        Rooms = new Room[width,length];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < length; y++)
            {
                RoomType type;
                if(x == spawn.X && y == spawn.Y) type = RoomType.Spawn;
                else if(x == fountain.X && y == fountain.Y) type = RoomType.Fountain;
                else type = RoomType.Normal;

                Rooms[x,y] = new Room(type);
                Console.Write($"   {type}   ");
            }
            Console.WriteLine();
        }
        Console.WriteLine("Enter a key to exit map generation...");
    }
}

public record Position(int X, int Y);

public class Player 
{
    public bool IsAlive {get; private set;}
    public Position Position {get; set;}


    public Player() {
        IsAlive = true;
        Position = new Position(0,0);
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
    RoomType Type {get; init;}
    public Room(RoomType type) {
        Type = type;
    }
}

public interface IPlayerCommand
{
    public bool RunCommand(Player player);
}

public class North : IPlayerCommand
{
    public bool RunCommand(Player player)
    {
        if(player.IsAlive) 
        {
            player.Position = new Position(player.Position.X, player.Position.Y+1);
            return true;
        }

        return false;
    }
}

public class South : IPlayerCommand
{
    public bool RunCommand(Player player)
    {
        if(player.IsAlive) 
        {
            player.Position = new Position(player.Position.X, player.Position.Y-1);
            return true;
        }

        return false;
    }
}

public class East : IPlayerCommand
{
    public bool RunCommand(Player player)
    {
        if(player.IsAlive) 
        {
            player.Position = new Position(player.Position.X+1, player.Position.Y);
            return true;
        }

        return false;
    }
}

public class West : IPlayerCommand
{
    public bool RunCommand(Player player)
    {
        if(player.IsAlive) 
        {
            player.Position = new Position(player.Position.X-1, player.Position.Y);
            return true;
        }

        return false;
    }
}

public static class InputHandler {

    public static ConsoleKeyInfo GetPlayerMove() {
        ConsoleKeyInfo key;

        do {
            Console.WriteLine("Please enter next direction with arrow key...");
            key = Console.ReadKey();
        } while(
            key.Key != ConsoleKey.RightArrow &&
            key.Key != ConsoleKey.LeftArrow &&
            key.Key != ConsoleKey.UpArrow &&
            key.Key != ConsoleKey.DownArrow
        );
        Console.WriteLine($"Key : {key.Key}");
        return key;
    }
}

public enum RoomType {Spawn, Fountain, Normal, OffMap};