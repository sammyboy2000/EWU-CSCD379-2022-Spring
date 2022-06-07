using Microsoft.AspNetCore.Mvc;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameController : ControllerBase
{
    private readonly GameService _service;

    public GameController(GameService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Post([FromBody] GamePost gamePost)
    {
        bool gameOver = 
            _service.Update(gamePost.PlayerGuid, gamePost.GameId, gamePost.Guess);
        return Ok(gameOver);
    }

    public class GamePost
    {
        public string PlayerGuid {  get; set; }
        public int GameId { get; set; }
        public string Guess { get; set; }
    }
}
