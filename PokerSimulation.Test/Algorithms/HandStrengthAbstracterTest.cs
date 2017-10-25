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
        public void MapToHandBucketTest()
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

            var handbucket = HandStrengthAbstracter.MapToHandBucket(board, holeCards);
            Assert.AreEqual(HandStrengthBucket.TopHands, handbucket);
        }
    }
}
