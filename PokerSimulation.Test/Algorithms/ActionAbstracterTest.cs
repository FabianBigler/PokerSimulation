using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Test.Algorithms
{
    [TestClass]
    public class ActionAbstracterTest
    {
        [TestMethod]
        public void GetBetSizeSmallPotRaiseTest()
        {
            int betSize = ActionAbstracter.GetBetSize(ActionBucket.LowBet, 10, 20);
            int expectedBetSize = 20;
            Assert.AreEqual(expectedBetSize, betSize);
            
            betSize = ActionAbstracter.GetBetSize(ActionBucket.MediumBet, 10, 20);
            expectedBetSize = 30;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.HighBet, 10, 30);
            expectedBetSize = 90;
            Assert.AreEqual(expectedBetSize, betSize);
        }

        [TestMethod]
        public void GetBetSizeBigPotRaiseTest()
        {
            int betSize = ActionAbstracter.GetBetSize(ActionBucket.LowBet, 20, 190);
            int expectedBetSize = 10;
            Assert.AreEqual(expectedBetSize, betSize);
            
            betSize = ActionAbstracter.GetBetSize(ActionBucket.MediumBet, 10, 190);
            expectedBetSize = 10;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.HighBet, 10, 190);
            expectedBetSize = 10;
            Assert.AreEqual(expectedBetSize, betSize);
        }


        [TestMethod]
        public void GetBetSizeSmallPotBetTest()
        {
            int betSize = ActionAbstracter.GetBetSize(ActionBucket.LowBet, 0, 20);
            int expectedBetSize = 10;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.MediumBet, 0, 20);
            expectedBetSize = 20;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.HighBet, 0, 30);
            expectedBetSize = 85;
            Assert.AreEqual(expectedBetSize, betSize);
        }

        [TestMethod]
        public void GetBetSizeBigPotBetTest()
        {
            int betSize = ActionAbstracter.GetBetSize(ActionBucket.LowBet, 0, 180);
            int expectedBetSize = 10;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.MediumBet, 0, 180);
            expectedBetSize = 10;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.HighBet, 0, 150);
            expectedBetSize = 25;
            Assert.AreEqual(expectedBetSize, betSize);
        }
    }
}
