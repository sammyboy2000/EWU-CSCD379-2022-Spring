using Wordle.Api.Data;
using System.Linq;
using static Wordle.Api.Data.Game;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Wordle.Api.Controllers;
using System.Collections.Generic;

namespace Wordle.Api.Services;

public class GuessService
{
    private readonly AppDbContext _context;
    private static readonly Dictionary<int, string> _cache = new();

    public GuessService(AppDbContext context)
    {
        _context = context;
    }

    internal int GetWordListCount(string partialWord)
    {
        return _context
            .Words
            .Where(x => x.Value.StartsWith(partialWord))
            .Count();
    }

    internal IEnumerable<string> GetWordList(string partialWord)
    {
        return _context
            .Words
            .Select(x => x.Value)
            .Where(x => x.StartsWith(partialWord))
            .AsEnumerable();
    }

    internal IEnumerable<string> GetWordListPage(int page, int count)
    {
        List<string> words = new();
        words.Add("not working");
        return words.AsEnumerable();
    }
}
