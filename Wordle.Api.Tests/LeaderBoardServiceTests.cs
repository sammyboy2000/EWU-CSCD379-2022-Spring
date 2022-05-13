using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wordle.Api.Data;
using Wordle.Api.Services;

namespace Wordle.Api.Tests
{

    [TestClass]
    public class LeaderBoardServiceTests : DatabaseBaseTests
    {

        [TestMethod]
        public async Task AddNewScoreToLeaderBoard_AddsOneToTotalCountAsync()
        {
            using var context = new AppDbContext(Options);
            ILeaderBoardService sut = new LeaderBoardService(context);

            int sutCount = sut.GetScores().Count();

            sut.AddScore(new GameScore(7, Guid.NewGuid().ToString(), 9001));

            await context.SaveChangesAsync();

            Assert.AreEqual(sutCount + 1, sut.GetScores().Count());
        }

        [TestMethod]
        public async Task AddOldScoreToLeaderBoard_AveragesAndGameCountCorrectAsync()
        {
            using var context = new AppDbContext(Options);
            ILeaderBoardService sut = new LeaderBoardService(context);

            string guid = Guid.NewGuid().ToString();

            sut.AddScore(new GameScore(7, guid, 1000));
            sut.AddScore(new GameScore(9, guid, 2000));

            var score = sut.GetScores().First(s => s.Name == guid);

            await context.SaveChangesAsync();

            Assert.AreEqual(1500, score.AverageSeconds);
            Assert.AreEqual(8, score.AverageGuesses);
            Assert.AreEqual(2, score.NumberGames);
        }
    }

}
