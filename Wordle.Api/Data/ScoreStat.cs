using System.ComponentModel.DataAnnotations;

namespace Wordle.Api.Data
{
    public class ScoreStat
    {
        [Key]
        public int ScoreStatId { get; set; }
        public int Score { get; set; }
        public int AverageSeconds { get; set; }
        public int TotalGames { get; set; }
        //Todo-Remove this comment after workflow established.

        public ScoreStat Clone()
        {
            return new ScoreStat
            {
                ScoreStatId = ScoreStatId,
                Score = Score,
                AverageSeconds = AverageSeconds,
                TotalGames = TotalGames,

            };
        }
    }
}
