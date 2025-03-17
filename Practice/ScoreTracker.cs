namespace Practice;

public class ScoreTracker
{
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }

    public void AddToHomeScore(int score)
    {
        HomeScore += score;
    }

    public void AddToAwayScore(int score)
    {
        AwayScore += score;
    }

}
