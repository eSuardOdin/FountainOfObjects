namespace FountainGame.Time {
    public class TimeKeeper {
        private DateTime Entered {get; init;}
        public TimeSpan TimeSpent {get; private set;}
        public TimeKeeper() {
            Entered = DateTime.Now;
        }
    }
}