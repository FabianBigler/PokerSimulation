using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
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
    public class HandStrengthAbstracterTest
    {
        [TestMethod]
        public void MapToHandBucketTopHandsTest()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.King));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Queen));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Clubs, CardValue.Ace));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Ace));
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Two));
            board.Add(new Card(CardSuit.Spades, CardValue.Two));

            var handbucket = HandStrengthAbstracter.MapToBucket(board, holeCards);
            Assert.AreEqual(HandStrengthBucket.TopHands, handbucket);
        }

        [TestMethod]
        public void MapToHandBucketTopPairTest()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Ace));
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Three));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Ace));
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Four));
            board.Add(new Card(CardSuit.Spades, CardValue.Two));

            var handbucket = HandStrengthAbstracter.MapToBucket(board, holeCards);
            Assert.AreEqual(HandStrengthBucket.TopPair, handbucket);
        }

        [TestMethod]
        public void MapToHandBucketMiddlePairTest()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Three));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Ace));
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Four));
            board.Add(new Card(CardSuit.Spades, CardValue.Two));

            var handbucket = HandStrengthAbstracter.MapToBucket(board, holeCards);
            Assert.AreEqual(HandStrengthBucket.MiddlePair, handbucket);
        }

        [TestMethod]
        public void MapToHandBucketLowPairTest()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.King));
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Three));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Ace));
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Two));
            board.Add(new Card(CardSuit.Spades, CardValue.Two));

            var handbucket = HandStrengthAbstracter.MapToBucket(board, holeCards);
            Assert.AreEqual(HandStrengthBucket.LowPair, handbucket);
        }

        [TestMethod]
        public void MapToHandBucketHighCardAceTest()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Six));
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Three));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Ace));
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Two));
            board.Add(new Card(CardSuit.Spades, CardValue.Five));

            var handbucket = HandStrengthAbstracter.MapToBucket(board, holeCards);
            Assert.AreEqual(HandStrengthBucket.HighCardAce, handbucket);
        }

        [TestMethod]
        public void MapToHandBucketHighCardElseTest()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Six));
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Three));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Queen));
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Two));
            board.Add(new Card(CardSuit.Spades, CardValue.Five));

            var handbucket = HandStrengthAbstracter.MapToBucket(board, holeCards);
            Assert.AreEqual(HandStrengthBucket.HighCardElse, handbucket);
        }
    }
}
