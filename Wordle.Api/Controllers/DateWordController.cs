using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Wordle.Api.Data;
using Wordle.Api.Dtos;
using Wordle.Api.Services;
using static Wordle.Api.Data.Game;

namespace Wordle.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DateWordController : Controller
{
    private readonly GameService _gameService;


    public DateWordController(GameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet]
    public IEnumerable<DateWordDto> Get(string playerGuid = "Fine if not a Guid")
    {
        bool hasGuid = false;
        if(Guid.TryParse(playerGuid, out _)){
            hasGuid = true;
        }
        var dateWordInfoList = _gameService.CreateDataWordInfo(playerGuid, hasGuid);
        /*
        //pretty sure something is slow in dateword fetching causing this list to not populate
        if(dateWordInfoList.Count() < 10)
        {
            dateWordInfoList = _gameService.CreateDataWordInfo(playerGuid, hasGuid);
        }
        */
        foreach (var I in dateWordInfoList)
        {
            yield return new DateWordDto(I.date, I.numPlays, I.averageScore, I.averageTime, I.hasPlayed, hasGuid);
        }
        
    }


[HttpPost]
    public GameDto CreateGame([FromBody] CreateGameDto newGame)
    {
        if (!Guid.TryParse(newGame.PlayerGuid, out _)) //done to catch invalid guids
        {
            newGame.PlayerGuid = Guid.NewGuid().ToString(); //Generate a fresh GUID if catching an invalid guid
        }
        Game game = _gameService.CreateGame(new Guid(newGame.PlayerGuid), GameTypeEnum.WordOfTheDay, newGame.Date);
        return new GameDto(game);
    }

    public class CreateGameDto
    {
        public DateTime? Date { get; set; }
        public string PlayerGuid { get; set; } = null!;
    }
}

internal record struct NewStruct(object Item1, object Item2)
{
    public static implicit operator (object, object)(NewStruct value)
    {
        return (value.Item1, value.Item2);
    }

    public static implicit operator NewStruct((object, object) value)
    {
        return new NewStruct(value.Item1, value.Item2);
    }
}