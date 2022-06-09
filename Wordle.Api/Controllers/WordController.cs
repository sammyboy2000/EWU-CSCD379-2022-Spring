using Microsoft.AspNetCore.Mvc;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordController : ControllerBase
{
    private readonly GuessService _service;

    public WordController(GuessService service)
    {
        _service = service;
    }

    [Route("[action]")]
    [HttpGet]
    public int GetWordCount(string partialWord)
    {
        return _service.GetWordListCount(partialWord);
    }

    [Route("[action]")]
    [HttpGet]
    public IEnumerable<string> GetWordList(string partialWord)
    {
        return _service.GetWordList(partialWord);
    }

}
