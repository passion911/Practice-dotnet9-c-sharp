namespace Practice
{
    public class BasketballEvent
    {
        public int HomePoints { get; set; }
        public int AwayPoints { get; set; }
        public ScoreTracker Tracker { get; set; }

        public BasketballEvent(int homePoints, int awayPoints, ScoreTracker scoreTracker)
        {
            HomePoints = homePoints;
            AwayPoints = awayPoints;
            Tracker = scoreTracker;
        }

        public void PrintScore()
        {
            Console.WriteLine($"The Current Score is: {Tracker.HomeScore}:{Tracker.AwayScore} ");
        }

    }
}
