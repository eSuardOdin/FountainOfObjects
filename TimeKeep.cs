namespace FountainGame.Time {
    public class TimeKeeper {
        public DateTime Entered {get; init;}
        public DateTime Exited {get; set;}
        public TimeSpan TimeSpent {get; private set;}
        public TimeKeeper() {
            Entered = DateTime.Now;
            Console.WriteLine($"You entered at ${Entered}");
        }

        public void Exit() {
            Exited = DateTime.Now;
            TimeSpent = Exited - Entered;
            Console.WriteLine($"You stayed {TimeSpent.Minutes}m and {TimeSpent.Seconds}s");
        }
    }
}