namespace Wordle.Api.Services
{
    public class GameScore
    {
        public int Score { get; set; }
        public string Name { get; set; }
        public int Seconds { get; set; }

        public GameScore(int score, string name, int seconds)
        {
            Score = score;
            Name = name;
            Seconds = seconds;
        }
    }
}
