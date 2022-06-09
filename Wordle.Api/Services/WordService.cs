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
    


}
