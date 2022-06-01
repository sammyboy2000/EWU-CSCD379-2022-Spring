using Wordle.Api.Data;
using System.Linq;
using static Wordle.Api.Data.Game;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Wordle.Api.Controllers;

namespace Wordle.Api.Services;

public class GameService
{
    private readonly AppDbContext _context;
    private readonly static object _mutex = new();
    private static readonly ConcurrentDictionary<DateTime, DateWord> _cache = new();

    public GameService(AppDbContext context)
    {
        _context = context;
    }

    internal bool Update(string playerGuid, int gameId, string guess)
    {
        var game = _context
            .Games
            .Where(x => x.GameId == gameId)
            .Include(x => x.Player)
            .Include(x => x.Guesses)
            .Include(x => x.Word)
            .Take(1);

        game.First().Guesses.Add(new Guess() { Value = guess });
        _context.SaveChanges();

        bool gameOver = false;

        if(game.First().Word.Value == guess)
        {
            FinishGame(gameId);
            gameOver = true;
        }
        return gameOver;
    }

    public Game CreateGame(Guid playerGuid, GameTypeEnum gameType, DateTime? date = null)
    {

        var player = _context.Players
            .FirstOrDefault(x => x.Guid == playerGuid);
        if (player is null)
        {
            player = new Player { Guid = playerGuid };
            _context.Players.Add(player);
            _context.SaveChanges();
        }

        //Return the game if it already exists
        Word word;
        if (gameType == GameTypeEnum.WordOfTheDay)
        {
            if (date == null) throw new ArgumentException("Date cannot be null if the game type is WordOfTheDay");
            
            var existingGame = _context.Games
                .Include(x => x.Guesses)
                .Include(x => x.Word)
                .FirstOrDefault(x => x.PlayerId == player.PlayerId &&
                                     x.GameType == GameTypeEnum.WordOfTheDay &&
                                     x.WordDate == date.Value);
            if (existingGame is not null)
            {
                return existingGame;
            }
            // If this is a new game, get the word of the day.
            word = GetDailyWord(date.Value) ?? throw new ArgumentException("Date is too far in the future");
        }
        else
        {
            // If this is a new game, get a random word.
            word = GetWord();
        }

        var game = new Game()
        {
            WordId = word.WordId,
            PlayerId = player.PlayerId,
            DateStarted = DateTime.UtcNow,
            GameType = gameType,
            WordDate = date
        };
        _context.Games.Add(game);

        _context.SaveChanges();

        game.Word = word;
        return game;

    }

    public void FinishGame(int gameId)
    {
        var game = _context.Games
            .FirstOrDefault(x => x.GameId == gameId);
        if (game is null) throw new ArgumentException("Game does not exist");

        game.DateEnded = DateTime.UtcNow;
        _context.SaveChanges();
    }

    public Word GetWord()
    {
        int wordCount = _context.Words.Count(f => f.Common);
        int randomIndex = new Random().Next(0, wordCount);
        Word chosenWord = _context.Words
            .Where(f => f.Common)
            .OrderBy(w => w.WordId)
            .Skip(randomIndex)
            .Take(1)
            .First();
        return chosenWord;
    }

    public IEnumerable<DateWord> GetLast10DateWords()
    {
        //Making sure the last ten days are initialized
        int numOfValidWords = 10;

        for (var i = 0; i < numOfValidWords; i++)
        {
           yield return GetDailyDateWord(DateTime.UtcNow.AddDays(-i));
        }
    }

    public IEnumerable<(DateTime date, int numPlays, int averageScore, int averageTime, bool hasPlayed)> CreateDataWordInfo(string playerGuid, bool hasGuid)
    {
        foreach(DateWord dateword in GetLast10DateWords())
        {
            var games = _context
          .Games
          .Include(x => x.Guesses)
          .Include(x => x.Player)
          .Where(x => x
                        .GameType == Game.GameTypeEnum.WordOfTheDay
                                     && x.DateEnded != null
                                     && x.WordDate == dateword.Date);
            
            (DateTime date, int numPlays, int averageScore, int averageTime, bool hasPlayed) info;

            info.date = dateword.Date;
            info.numPlays = games.Count();
            info.averageScore = (int)games.Select(x => x.Guesses.Count).ToList().DefaultIfEmpty().Average();
            info.averageTime = (int)games.Select(x => ((x.DateEnded - x.DateStarted) ?? TimeSpan.MaxValue).TotalSeconds).ToList().DefaultIfEmpty().Average();
            if (hasGuid)
            {
                info.hasPlayed = games.Any(x => x.Player.Guid.ToString() == playerGuid);
            }
            else
            {
                info.hasPlayed = false;
            }
            yield return info;
        }
    }

    public Word? GetDailyWord(DateTime date)
    {
        return GetDailyDateWord(date)?.Word;
    }
    

    public DateWord GetDailyDateWord(DateTime date)
    {
        if (date.ToUniversalTime() >= System.DateTime.Today.ToUniversalTime().AddDays(.75))
        {
            return GetDailyDateWord(default); //I was not a fan of returning null so I just give people trying to play in the future the oldest dateword possible
        }
        //Sanitize the date by dropping time data
        date = date.Date;
        //Check if the day has a word in the database
        if (_cache.TryGetValue(date, out DateWord? word))
        {
            if (word is not null)
            {
                return word;
            }
        }
        DateWord? wordOfTheDay = _context.DateWords
            .Include(x => x.Word)
            .FirstOrDefault(dw => dw.Date == date);

        if (wordOfTheDay != null)
        //Yes: return the word
        {
            _cache.TryAdd(date, wordOfTheDay);
            return wordOfTheDay;
        }
        else
        {
            //Mutex magic
            lock (_mutex)
            {
                wordOfTheDay = _context.DateWords
                    .Include(x => x.Word)
                    .FirstOrDefault(dw => dw.Date == date);
                if (wordOfTheDay != null)
                //Yes: return the word
                {
                    _cache.TryAdd(date, wordOfTheDay);
                    return wordOfTheDay;
                }
                else
                {
                    //No: get a random word from our list
                    Word chosenWord = GetWord();
                    //Save the word to the database with the date
                    _context.DateWords.Add(new DateWord { Date = date, Word = chosenWord });
                    _context.SaveChanges();
                    
                    wordOfTheDay = _context.DateWords
                    .Include(x => x.Word)
                    .FirstOrDefault(dw => dw.Date == date);
                    if(wordOfTheDay is null)
                    {
                        throw new DbUpdateException("Word of the day did not update.");
                    }
                    //Yes: return the word
                    _cache.TryAdd(date, wordOfTheDay);
                    return wordOfTheDay;
                }
            }
        }
    }
}
