namespace FountainGame.Commands
{
    using FountainGame.Player;    
    using FountainGame.Map;
    using FountainGame.Position;    
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
}

