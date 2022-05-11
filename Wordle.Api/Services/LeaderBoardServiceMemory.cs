namespace Wordle.Api.Services
{
    public class LeaderBoardServiceMemory : ILeaderBoardService
    {

        private static readonly List<Score> _scores = new List<Score>();

        public LeaderBoardServiceMemory()
        {
            if (!_scores.Any())
            {
                _scores.Add(new Score("Ralph", 30, 3.4, 42));
                _scores.Add(new Score("Gene", 50, 4.1, 42.2));
                _scores.Add(new Score("Hildagaurd", 25, 2.6, 3));
            }
        }

        public void AddScore(GameScore gameScore)
        {
            var score = _scores.FirstOrDefault(f => f.Name == gameScore.Name);
            if (score is not null)
            {
                score.AverageGuesses = ((score.NumberGames * score.AverageGuesses)
                    + gameScore.Score) / ++score.NumberGames;
            }
            else
            {
                _scores.Add(new Score(gameScore.Name, 1, gameScore.Score, gameScore.Seconds));
            }
        }

        public IEnumerable<Score> GetScores()
        {
            return _scores;
        }
    }
}
