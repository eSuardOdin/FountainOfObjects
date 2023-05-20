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
                if(x == spawn.X && y == spawn.Y) type = RoomType.Spawn;
                else if(x == fountain.X && y == fountain.Y) type = RoomType.Fountain;
                else type = RoomType.Normal;

                Rooms[x,y] = new Room(type);

                // --------------------------
                // To delete, here for debug
                switch (type)
                {
                    case RoomType.Fountain:
                        Console.Write("F");
                        break;
                    case RoomType.Spawn:
                        Console.Write("O");
                        break;
                    default:
                        Console.Write("#");
                        break;
                }
            }
            Console.WriteLine();
                // End delete
                // --------------------------
            
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
    public (bool, string) RunCommand(Player player, Map map);
}

public class North : IPlayerCommand
{
    public (bool, string) RunCommand(Player player, Map map)
    {
        if(player.Position.Y < map.Rooms.GetLength(1) - 1) 
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

public enum RoomType {Spawn, Fountain, Normal, OffMap};