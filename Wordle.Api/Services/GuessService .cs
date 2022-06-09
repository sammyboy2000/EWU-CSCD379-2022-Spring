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
        var word = partialWord.ToCharArray(); 
        int num = partialWord.Length;
        return num;
    }

    internal IEnumerable<string> GetWordList(string partialWord)
    {
        List<string> wordList = new();
        wordList.Add(partialWord);
        wordList.Add("is");
        wordList.Add("the");
        wordList.Add("word");
        wordList.Add("that");
        wordList.Add("was");
        wordList.Add("submitted");

        return wordList.AsEnumerable();
    }
}
