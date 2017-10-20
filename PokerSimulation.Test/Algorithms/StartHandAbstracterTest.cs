using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Core.Model;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Test.Algorithms
{
    [TestClass]
    public class StartHandAbstracterTest
    {
        [TestMethod]
        public void FromStartHandTest()
        {
            var card1 = new Card(CardSuit.Clubs, CardValue.Ace);
            var card2 = new Card(CardSuit.Clubs, CardValue.King);
            var bucket = StartHandAbstracter.FromStartHand(card1, card2);
            Assert.AreEqual(bucket, StartHandBucket.VeryGood);

            card1 = new Card(CardSuit.Diamonds, CardValue.Ace);
            card2 = new Card(CardSuit.Clubs, CardValue.King);
            bucket = StartHandAbstracter.FromStartHand(card1, card2);
            Assert.AreEqual(bucket, StartHandBucket.VeryGood);

            card1 = new Card(CardSuit.Diamonds, CardValue.Two);
            card2 = new Card(CardSuit.Diamonds, CardValue.Eight);
            bucket = StartHandAbstracter.FromStartHand(card1, card2);
            Assert.AreEqual(bucket, StartHandBucket.VeryBad);

            card1 = new Card(CardSuit.Diamonds, CardValue.Two);
            card2 = new Card(CardSuit.Clubs, CardValue.Eight);
            bucket = StartHandAbstracter.FromStartHand(card1, card2);
            Assert.AreEqual(bucket, StartHandBucket.Worst);

            card1 = new Card(CardSuit.Diamonds, CardValue.Eight);
            card2 = new Card(CardSuit.Clubs, CardValue.Eight);
            bucket = StartHandAbstracter.FromStartHand(card1, card2);
            Assert.AreEqual(bucket, StartHandBucket.Best);

            card1 = new Card(CardSuit.Diamonds, CardValue.Jack);
            card2 = new Card(CardSuit.Clubs, CardValue.Two);
            bucket = StartHandAbstracter.FromStartHand(card1, card2);
            Assert.AreEqual(bucket, StartHandBucket.VeryBad);

            card1 = new Card(CardSuit.Diamonds, CardValue.Jack);
            card2 = new Card(CardSuit.Diamonds, CardValue.Two);
            bucket = StartHandAbstracter.FromStartHand(card1, card2);
            Assert.AreEqual(bucket, StartHandBucket.AverageBad);
        }
    }
}
