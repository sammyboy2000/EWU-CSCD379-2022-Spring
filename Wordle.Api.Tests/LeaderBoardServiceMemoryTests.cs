using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Wordle.Api.Services;

namespace Wordle.Api.Tests
{
    [TestClass]
    public class LeaderBoardServiceMemoryTests
    {
        [TestMethod]
        public void AddScore_AddsNewPlayer()
        {
            LeaderBoardServiceMemory sut = new LeaderBoardServiceMemory();
            Assert.AreEqual(3, sut.GetScores().Count());
            sut.AddScore(new GameScore(1, "test", 0));
            Assert.AreEqual(4, sut.GetScores().Count());
        }
        
        [TestMethod]
        public void AddScore_UpdatesExistingPlayer()
        {
            LeaderBoardServiceMemory sut = new LeaderBoardServiceMemory();
            sut.AddScore(new GameScore(5, "Ralph", 0));
            Assert.AreEqual(31, sut.GetScores().First(x => x.Name == "Ralph").NumberGames);
        }
    }
}