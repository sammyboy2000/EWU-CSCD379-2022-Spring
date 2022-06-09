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

    [HttpGet]
    public int GetWordCount(string partialWord = "Default")
    {
        return _service.GetWordListCount(partialWord);
    }

    [Route("[action]")]
    [HttpGet]
    public IEnumerable<string> GetWordList(string partialWord = "Default")
    {
        return _service.GetWordList(partialWord);
    }

}
