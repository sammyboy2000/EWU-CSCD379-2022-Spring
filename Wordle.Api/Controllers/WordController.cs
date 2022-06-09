using Microsoft.AspNetCore.Mvc;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WordController : ControllerBase
{
    private readonly WordService _wordService;

    public WordController( WordService wordService)
    {
        _wordService = wordService;
    }

    [Route("[action]")]
    [HttpGet]
    public IEnumerable<string> GetWordListPage(string partialWord, int page, int count)
    {
        return _wordService.GetWordListPage(partialWord, page, count);
    }

    [Route("[action]")]
    [HttpGet]
    public IEnumerable<string> GetAllWordListPage(int page, int count)
    {
        return _wordService.GetAllWordListPage(page, count);
    }

    [Route("[action]")]
    [HttpGet]
    public int GetNumberOfPages(string partialWord, int count)
    {
        return _wordService.GetNumberOfPages(partialWord, (double)count);
    }

    [Route("[action]")]
    [HttpGet]
    public int GetAllNumberOfPages(int count)
    {
        return _wordService.GetNumberOfPages((double) count);
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
