﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerEngine.Model;
using PokerEngine.Model.Bots;
using System.Collections.Generic;
using PokerEngine.Enumerations;
using PokerEngine.Entities;

namespace TestPokerEngine
{
    [TestClass]
    public class HeadsupGameTest
    {
        [TestMethod]
        public void TestSimpleHandByCallingStationBots()
        {
            var callingStationBot1 = new CallingStationBot(new PlayerEntity { Id = new Guid() });
            var callingStationBot2 = new CallingStationBot(new PlayerEntity { Id = new Guid() });
            var players = new List<Player> { callingStationBot1, callingStationBot2 };
            var dealer = new DeterministicDealer();
            dealer.HoleCardsPlayer1 = new List<Card> { new Card(CardSuit.Diamonds, CardValue.Ten), new Card(CardSuit.Diamonds, CardValue.Nine) };
            dealer.HoleCardsPlayer2 = new List<Card> { new Card(CardSuit.Clubs, CardValue.Ace), new Card(CardSuit.Clubs, CardValue.Eight) };            
            dealer.BoardCards = new List<Card> {
                                new Card(CardSuit.Diamonds, CardValue.Two),
                                new Card(CardSuit.Diamonds, CardValue.Three),
                                new Card(CardSuit.Diamonds, CardValue.Four),
                                new Card(CardSuit.Spades, CardValue.Jack),
                                new Card(CardSuit.Spades, CardValue.Queen)};

            var game = new HeadsupGame(players, dealer);
            var result = game.PlayHand();
            //player 1 wins with a flush
            Assert.IsTrue(result.Winner == callingStationBot1);
            //small blind calls and both players check until showdown => PotSize must be 2 big blinds
            Assert.IsTrue(result.PotSize == (HeadsupGame.BigBlindSize * 2));
        }


        [TestMethod]
        public void TestSimpleHandByAlwaysRaiseBots()
        {
            var alwaysRaiseBot1 = new AlwaysMinRaiseBot(new PlayerEntity { Id = new Guid() });
            var alwaysRaiseBot2 = new AlwaysMinRaiseBot(new PlayerEntity { Id = new Guid() });
            var players = new List<Player> { alwaysRaiseBot1, alwaysRaiseBot2 };
            var dealer = new DeterministicDealer();
            dealer.HoleCardsPlayer1 = new List<Card> { new Card(CardSuit.Diamonds, CardValue.Ten), new Card(CardSuit.Diamonds, CardValue.Nine) };
            dealer.HoleCardsPlayer2 = new List<Card> { new Card(CardSuit.Clubs, CardValue.Ace), new Card(CardSuit.Clubs, CardValue.Eight) };
            dealer.BoardCards = new List<Card> {
                                new Card(CardSuit.Diamonds, CardValue.Two),
                                new Card(CardSuit.Diamonds, CardValue.Three),
                                new Card(CardSuit.Diamonds, CardValue.Four),
                                new Card(CardSuit.Spades, CardValue.Jack),
                                new Card(CardSuit.Spades, CardValue.Queen)};

            var game = new HeadsupGame(players, dealer);
            var result = game.PlayHand();
            //player 1 wins with a flush
            Assert.IsTrue(result.Winner == alwaysRaiseBot1);
            //pot size should be all ins of both bots
            Assert.IsTrue(result.PotSize == HeadsupGame.StackSize * 2);
        }
    }
}
