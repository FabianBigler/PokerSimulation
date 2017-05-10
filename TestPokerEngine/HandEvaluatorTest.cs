using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using PokerEngine.Model;
using PokerEngine.Helpers;
using PokerEngine.Exceptions;
using PokerEngine.Enumerations;

namespace TestPokerEngine
{
    [TestClass]
    public class HandEvaluatorTest
    {          
        [TestMethod]
        [ExpectedException(typeof(InvalidCardCountException))]
        public void TestInvalidCardCountException()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Two));
            var board = new List<Card>();
            board.Add(new Card(CardSuit.Diamonds, CardValue.Seven));
            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
        }


        [TestMethod]
        public void TestFlush()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Two));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Nine));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Diamonds, CardValue.Eight));
            board.Add(new Card(CardSuit.Clubs, CardValue.Eight));
            board.Add(new Card(CardSuit.Clubs, CardValue.Six));
            board.Add(new Card(CardSuit.Clubs, CardValue.Seven));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.Flush);
        }       

        [TestMethod]
        public void TestFourOfAKind()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Six));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Six));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Spades, CardValue.Six));
            board.Add(new Card(CardSuit.Hearts, CardValue.Six));            
            board.Add(new Card(CardSuit.Clubs, CardValue.Eight));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.FourOfAKind);
        }

        [TestMethod]
        public void TestFullHouse()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Seven));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Seven));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Spades, CardValue.Ten));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Nine));
            board.Add(new Card(CardSuit.Hearts, CardValue.Ten));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Jack));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Ten));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.FullHouse);
        }

        [TestMethod]
        public void TestHighCard()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Seven));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Six));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Spades, CardValue.Ten));
            board.Add(new Card(CardSuit.Hearts, CardValue.Queen));
            board.Add(new Card(CardSuit.Clubs, CardValue.Ace));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.HighCard);
        }

        [TestMethod]
        public void TestPair()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Seven));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Seven));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Spades, CardValue.Eight));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Nine));
            board.Add(new Card(CardSuit.Hearts, CardValue.Ace));
            board.Add(new Card(CardSuit.Diamonds, CardValue.King));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Queen));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.Pair);
        }

        [TestMethod]
        public void TestRoyalFlushSimple()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Ace));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.King));
            var board = new List<Card>();
            board.Add(new Card(CardSuit.Clubs, CardValue.Queen));
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(HandRank.RoyalFlush, rank);
        }

        [TestMethod]
        public void TestRoyalFlushShuffled()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.King));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Queen));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            board.Add(new Card(CardSuit.Clubs, CardValue.Ace));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(HandRank.RoyalFlush, rank);
        }

        [TestMethod]
        public void TestRoyalFlushMoreCards()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.King));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Queen));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Hearts, CardValue.Ace));
            board.Add(new Card(CardSuit.Clubs, CardValue.Jack));
            board.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Eight));
            board.Add(new Card(CardSuit.Clubs, CardValue.Ace));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(HandRank.RoyalFlush, rank);
        }

        [TestMethod]
        public void TestStraight()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Six));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Ten));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Diamonds, CardValue.Nine));
            board.Add(new Card(CardSuit.Spades, CardValue.Seven));
            board.Add(new Card(CardSuit.Hearts, CardValue.Six));
            board.Add(new Card(CardSuit.Clubs, CardValue.Eight));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.Straight);
        }

        [TestMethod]
        public void TestStraightCards()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Six));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Ten));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Diamonds, CardValue.Nine));
            board.Add(new Card(CardSuit.Spades, CardValue.Seven));
            board.Add(new Card(CardSuit.Hearts, CardValue.Six));
            board.Add(new Card(CardSuit.Clubs, CardValue.Eight));

            var evaluator = new HandEvaluator(holeCards, board);
            var bestCards = evaluator.GetBestFiveCards(evaluator.GetHandRank());


            Assert.AreEqual(bestCards.Count, 5);
        }

        [TestMethod]
        public void TestStraightFlush()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Ten));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Nine));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Diamonds, CardValue.Eight));
            board.Add(new Card(CardSuit.Clubs, CardValue.Eight));
            board.Add(new Card(CardSuit.Clubs, CardValue.Six));
            board.Add(new Card(CardSuit.Clubs, CardValue.Seven));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.StraightFlush);
        }

        [TestMethod]
        public void TestThreeOfAKind()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Seven));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Seven));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Spades, CardValue.Eight));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Nine));
            board.Add(new Card(CardSuit.Hearts, CardValue.Seven));
            board.Add(new Card(CardSuit.Diamonds, CardValue.King));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Queen));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.ThreeOfAKind);
        }

        [TestMethod]
        public void TestTwo()
        {
            var holeCards = new List<Card>();
            holeCards.Add(new Card(CardSuit.Diamonds, CardValue.Seven));
            holeCards.Add(new Card(CardSuit.Clubs, CardValue.Seven));

            var board = new List<Card>();
            board.Add(new Card(CardSuit.Spades, CardValue.Eight));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Nine));
            board.Add(new Card(CardSuit.Hearts, CardValue.Two));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Two));
            board.Add(new Card(CardSuit.Diamonds, CardValue.Queen));

            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            Assert.AreEqual(rank, HandRank.TwoPairs);
        }
    }
}
