using Microsoft.AspNetCore.Mvc;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordController : ControllerBase
{
    private readonly GuessService _guessService;
    private readonly WordService _wordService;

    public WordController(GuessService guessService, WordService wordService)
    {
        _guessService = guessService;
        _wordService = wordService;
    }

    [Route("[action]")]
    [HttpGet]
    public int GetWordCount(string partialWord)
    {
        return _guessService.GetWordListCount(partialWord);
    }

    [Route("[action]")]
    [HttpGet]
    public IEnumerable<string> GetWordList(string partialWord)
    {
        return _guessService.GetWordList(partialWord);
    }

    [Route("[action]")]
    [HttpGet]
    public IEnumerable<string> GetWordListPage(int page, int count)
    {
        return _guessService.GetWordListPage(page, count);
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult DeleteWord(string word)
    {
        bool success = false;

        //success = _wordService.DeleteWord(word);

        return Ok(success);
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult AddWord(string word)
    {
        bool success = false;

        return Ok(success);
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult MakeWordCommon(string word)
    {
        bool success = false;

        return Ok(success);
    }

    [Route("[action]")]
    [HttpPost]
    public IActionResult MakeWordUncommon(string word)
    {
        bool success = false;

        return Ok(success);
    }
}
