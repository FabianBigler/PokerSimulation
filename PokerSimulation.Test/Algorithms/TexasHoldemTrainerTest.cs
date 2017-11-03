using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms;
using PokerSimulation.Algorithms.TexasHoldem;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using PokerSimulation.Test.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Test.Algorithms
{
    [TestClass]
    public class TexasHoldemTrainerTest
    {                
        private TexasHoldemTrainer trainer = null;

        [TestInitialize]
        public void TrainOneSpecificHand()
        {
            var dealer = new DeterministicDealer();
            dealer.HoleCardsPlayer1 = new List<Card> { new Card(CardSuit.Diamonds, CardValue.Ace), new Card(CardSuit.Clubs, CardValue.Ace) };
            dealer.HoleCardsPlayer2 = new List<Card> { new Card(CardSuit.Hearts, CardValue.Seven), new Card(CardSuit.Clubs, CardValue.Two) };
            dealer.BoardCards = new List<Card> {
                                new Card(CardSuit.Diamonds, CardValue.Two),
                                new Card(CardSuit.Diamonds, CardValue.Three),
                                new Card(CardSuit.Diamonds, CardValue.Four),
                                new Card(CardSuit.Spades, CardValue.Jack),
                                new Card(CardSuit.Spades, CardValue.Queen)};

            trainer = new TexasHoldemTrainer(dealer);
            trainer.Train(1);
            foreach (var node in trainer.GameNodes)
            {
                Debug.WriteLine(node.Value);
            }
        }

        [TestMethod]
        public void TrainedOneHandTest()
        {
            // this number heavily depends on abstraction (i.e. stack size, bet size, hand strength and actions)
            int numberOfStatesPerBucket = 15889;           
            Assert.AreEqual(numberOfStatesPerBucket, trainer.GameNodes.Count);

            //check if information sets are complete
            var infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = (int)StartHandBucket.Best,
                ActionHistory = new List<ActionBucket>()
            };

            //check if some nodes has been added to the dictionary
            var gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            Assert.IsNotNull(gameNode);
            
            infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = (int)StartHandBucket.Best,
                ActionHistory = new List<ActionBucket>() { ActionBucket.Call, ActionBucket.LowBet }
            };
            gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            Assert.IsNotNull(gameNode);
            
            infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = (int)StartHandBucket.Best,
                ActionHistory = new List<ActionBucket>() { ActionBucket.LowBet, ActionBucket.LowBet, ActionBucket.LowBet, ActionBucket.LowBet }
            };
            gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            Assert.IsNotNull(gameNode);

            infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = (int)StartHandBucket.Worst,
                ActionHistory = new List<ActionBucket>() { ActionBucket.LowBet, ActionBucket.LowBet, ActionBucket.LowBet}
            };
            gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            Assert.IsNotNull(gameNode);
        }
    }
}
