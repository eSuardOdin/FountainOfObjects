namespace FountainGame.Senses
{
    using FountainGame.Map;
    using FountainGame.Room;
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

}


