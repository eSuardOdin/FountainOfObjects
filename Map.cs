namespace FountainGame.Map
{
    using FountainGame.Position;    
    public class Map 
    {
        public Room[,] Rooms {get; private set;}
        public Position Fountain {get; init;}
        public Position Spawn {get; init;}
        public Position PlayerPosition {get; private set;}
        public bool IsFountainActive {get; private set;}

        public Map(int size, int pitNb)
        {
            // Get random pos of Spawn
            Spawn = GetRandomPosition(0, size);
            PlayerPosition = new Position(Spawn.X, Spawn.Y);
            // Do while Spawn == Fountain -> Random pos fountain
            do
            {
                Fountain = GetRandomPosition(0, size);
            } while (Fountain.Equals(Spawn));
            // Populate map
            IsFountainActive = false;
            Rooms = new Room[size,size];
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    RoomType type;
                    if(x == Spawn.X && y == Spawn.Y) type = RoomType.Spawn;
                    else if(x == Fountain.X && y == Fountain.Y) type = RoomType.Fountain;
                    else type = RoomType.Normal; 

                    Rooms[x,y] = new Room(type);

                } 
            }
            // Have a number of pits to put on normal rooms, make sur they are not adjacent between them
            for (int i = 0; i < pitNb; i++)
            {
                Position pit;
                do
                {
                    pit = GetRandomPosition(0, size);
                } while (Rooms[pit.X, pit.Y].Type != RoomType.Normal || IsRoomTypeAdjacent(RoomType.Pit, pit));
                Rooms[pit.X, pit.Y].Type = RoomType.Pit;
            }
        }

        public void UpdatePlayerPosition(int x, int y)
        {
            PlayerPosition = new Position(x,y);
        }

        public void SwitchFountainOn() 
        {
            IsFountainActive = true;
        }

        private Position GetRandomPosition(int minLimit, int maxLimit) 
        {
            var rand = new Random();
            int x,y;
            x = rand.Next(minLimit, maxLimit);
            y = rand.Next(minLimit, maxLimit);
            return new Position(x, y);
        }

        
        /// <summary>
        /// Returns the type of room in the location. Returns OffMap if index is outside Rooms array bounds
        /// </summary>
        /// <param name="x">x axis position of the room in the array</param>
        /// <param name="y">y axis position of the room in the array</param>
        /// <returns>Type of the room at location [x,y]</returns>
        public RoomType GetRoomTypeAtLocation(int x, int y) 
        {
            if(
                x >= Rooms.GetLength(0)  ||
                x < 0                    ||
                y >= Rooms.GetLength(1)  ||
                y < 0
                )
            {
                return RoomType.OffMap;
            }
            return Rooms[x, y].Type;
        } 

        /// <summary>
        /// Checks if a Room type is adjacent to the player's position
        /// </summary>
        /// <param name="type">The type of room checked</param>
        /// <param name="player">The player whose position is checked</param>
        /// <returns>True if adjacent</returns>
        public bool IsRoomTypeAdjacent(RoomType type, Position playerPos) 
        {
            (int x, int y) = (playerPos.X, playerPos.Y);
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
    }
}

