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

public class Game 
{
    private Player player {get; set;}
    private Map map {get; set;}

}

public class Map 
{
    Room[] Rooms;

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
    public bool Run(Player player);
}

public class North : IPlayerCommand
{
    public bool Run(Player player)
    {
        if(player.IsAlive) 
        {
            player.Position = new Position(player.Position.X, player.Position.Y+1);
        }

        return false;
    }
}

public class South : IPlayerCommand
{
    public bool Run(Player player)
    {
        if(player.IsAlive) 
        {
            player.Position = new Position(player.Position.X, player.Position.Y-1);
        }

        return false;
    }
}

public class East : IPlayerCommand
{
    public bool Run(Player player)
    {
        if(player.IsAlive) 
        {
            player.Position = new Position(player.Position.X+1, player.Position.Y);
        }

        return false;
    }
}

public class West : IPlayerCommand
{
    public bool Run(Player player)
    {
        if(player.IsAlive) 
        {
            player.Position = new Position(player.Position.X-1, player.Position.Y);
        }

        return false;
    }
}

public enum RoomType {Start, Fountain, Normal, OffMap};