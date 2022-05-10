using Wordle.Api.Data;

namespace Wordle.Api.Services
{
    public class LeaderBoardService : ILeaderBoardService
    {
        public LeaderBoardService(AppDbContext context)
        {
            _context = context;
        }

        private AppDbContext _context;

        public void AddScore(GameScore gameScore)
        {
            var player = _context.Players.FirstOrDefault(f => f.Name == gameScore.Name);
            if (player is not null)
            {
                player.AverageAttempts = ((player.GameCount * player.AverageAttempts)
                    + gameScore.Score) / (player.GameCount + 1);

                player.AverageSecondsPerGame = ((player.GameCount * player.AverageSecondsPerGame)
                    + gameScore.Seconds) / (player.GameCount + 1);

                player.GameCount++;
            }
            else
            {
                _context.Players.Add(new Player()
                {
                    Name = gameScore.Name,
                    GameCount = 1,
                    AverageAttempts = gameScore.Score,
                    AverageSecondsPerGame = gameScore.Seconds
                });
            }
            _context.SaveChanges();
        }

        public IEnumerable<Score> GetScores()
        {
            var result = _context.Players.OrderBy(x => x.AverageAttempts).ThenBy(x => x.AverageSecondsPerGame);
            return (IEnumerable<Score>)result;
        }
    }
}
