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
            expectedBetSize = 190;
            Assert.AreEqual(expectedBetSize, betSize);
        }

        [TestMethod]
        public void GetBetSizeBigPotRaiseTest()
        {
            int betSize = ActionAbstracter.GetBetSize(ActionBucket.LowBet, 20, 380);
            int expectedBetSize = 20;
            Assert.AreEqual(expectedBetSize, betSize);
            
            betSize = ActionAbstracter.GetBetSize(ActionBucket.MediumBet, 10, 380);
            expectedBetSize = 15;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.HighBet, 10, 300);
            expectedBetSize = 55;
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
            expectedBetSize = 185;
            Assert.AreEqual(expectedBetSize, betSize);
        }

        [TestMethod]
        public void GetBetSizeBigPotBetTest()
        {
            int betSize = ActionAbstracter.GetBetSize(ActionBucket.LowBet, 0, 380);
            int expectedBetSize = 10;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.MediumBet, 0, 380);
            expectedBetSize = 10;
            Assert.AreEqual(expectedBetSize, betSize);

            betSize = ActionAbstracter.GetBetSize(ActionBucket.HighBet, 0, 300);
            expectedBetSize = 50;
            Assert.AreEqual(expectedBetSize, betSize);
        }
    }
}
