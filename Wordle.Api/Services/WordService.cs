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
        partialWord = partialWord.ToLower();

        return _context
            .Words
            .Where(x => !x.Deleted)
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
            .Where(x => !x.Deleted)
            .Skip(page * pageSize)
            .Take(pageSize)
            .Select(x => x.Value)
            .AsEnumerable();
    }

    internal int GetNumberOfPages(double count)
    {
        return (int)Math.Ceiling(_context.Words.Where(x => !x.Deleted).Count()/count);
    }

    internal int GetNumberOfPages(string partialWord, double count)
    {
        partialWord = partialWord.ToLower();

        var total = _context
            .Words
            .Where(x => !x.Deleted)
            .Select(x => x.Value)
            .Where(x => x.StartsWith(partialWord)).Count();

        return (int)Math.Ceiling(total / count);
    }

    internal bool DeleteWord(string word)
    {
        word = word.ToLower();

        var existence = WordExists(word);
        if (!existence.exists || existence.deleted)
        {
            return false;
        }

        _context.Words.First(x => x.Value == word).Deleted = true;
        _context.SaveChanges();
        return true;
    }

    internal bool AddWord(string word)
    {
        word = word.ToLower();

        var existence = WordExists(word);
        bool success = false;
        if (existence.exists && existence.deleted)
        {
            _context.Words.First(x => x.Value == word).Deleted = false;
            _context.SaveChanges();
            success = true;

        }else if (!existence.exists){
            _context.Words.Add(new Word()
            {
                Value = word
            });
            _context.SaveChanges();
            success = true;
        }

        return success;
    }

    internal bool MakeWordCommon(string word)
    {
        word = word.ToLower();

        var existence = WordExists(word);
        bool success = false;

        if (existence.exists && !existence.deleted)
        {
            Word tempWord = _context.Words.First(x => x.Value == word);
            if (tempWord.Common == false)
            {
                tempWord.Common = true;
                _context.SaveChanges();
                success = true;
            }
        }
        return success;
    }

    internal bool MakeWordUncommon(string word)
    {
        word = word.ToLower();

        var existence = WordExists(word);
        bool success = false;

        if (existence.exists && !existence.deleted)
        {
            Word tempWord = _context.Words.First(x => x.Value == word);
            if (tempWord.Common == true)
            {
                tempWord.Common = false;
                _context.SaveChanges();
                success = true;
            }
            
        }
        return success;
    }

    private (bool deleted, bool exists) WordExists(string word)
    {
        word = word.ToLower();

        bool deleted;
        bool exists;

        Word? selection = _context.Words.FirstOrDefault(x => x.Value.ToLower() == word);
        if(selection is null)
        {
            exists = false;
            deleted = false;
        }
        else
        {
            exists = true;
            deleted = selection.Deleted;
        }
        return (deleted, exists);
    }
}
