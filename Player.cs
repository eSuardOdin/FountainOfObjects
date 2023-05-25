namespace FountainGame.Player
{
    using FountainGame.Position;
    
    public class Player 
    {
        public bool IsAlive {get; private set;}
        public Position Position {get; set;}


        public Player(Position spawn) 
        {
            IsAlive = true;
            Position = new Position(spawn.X, spawn.Y);
        }

        /// <summary>
        /// Kills the player
        /// </summary>
        public void Kill() 
        {
            IsAlive = false;
        }

    }
}



