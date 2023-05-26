namespace FountainGame.Room
{
    public class Room 
    {
        public RoomType Type {get; set;}
        public Room(RoomType type) {
            Type = type;
        }
    }
    public enum RoomType {Spawn, Fountain, Normal, Pit, OffMap};
}


