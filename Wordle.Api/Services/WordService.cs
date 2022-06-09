using Wordle.Api.Data;
using System.Linq;
using static Wordle.Api.Data.Game;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using Wordle.Api.Controllers;
using System.Collections.Generic;

namespace Wordle.Api.Services;

public class WordService
{
    private readonly AppDbContext _context;

    public WordService(AppDbContext context)
    {
        _context = context;
    }

    internal IEnumerable<string> GetWordListPage(string partialWord, int page, int pageSize)
    {
        return _context
            .Words
            .Select(x => x.Value)
            .Where(x => x.StartsWith(partialWord))
            .Skip(page * pageSize)
            .Take(pageSize)
            .AsEnumerable();
    }

    internal IEnumerable<string> GetAllWordListPage(int page, int pageSize)
    {
        return _context
            .Words
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(x => x.Value)
            .AsEnumerable();
    }

    internal int GetNumberOfPages(double count)
    {
        return (int)Math.Ceiling(_context.Words.Count()/count);
    }

    internal int GetNumberOfPages(string partialWord, double count)
    {
       var total = _context
            .Words
            .Select(x => x.Value)
            .Where(x => x.StartsWith(partialWord)).Count();

        return (int)Math.Ceiling(total / count);
    }
}
