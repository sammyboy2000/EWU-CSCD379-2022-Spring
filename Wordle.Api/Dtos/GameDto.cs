using Wordle.Api.Data;

namespace Wordle.Api.Dtos
{
    public class GameDto
    {
        public string Word { get; set; }
        public int GameId { get; set; }
        public bool WasPlayed { get; set; }
        public string[] Guesses { get; set; }

        public GameDto(Game game)
        {
            Word = game.Word?.Value ?? "null";
            GameId = game.GameId;
            WasPlayed = game.DateEnded.HasValue;

            //not in use yet since guesses are not yet being posted
            List<string> tempGuesses = new();
            if (Word != "null" && game.Guesses is not null) 
            { 
                foreach (var guesses in game.Guesses)
                {
                    tempGuesses.Add(guesses.Value);
                }
            }
            Guesses = tempGuesses.ToArray();
        }
    }
}
