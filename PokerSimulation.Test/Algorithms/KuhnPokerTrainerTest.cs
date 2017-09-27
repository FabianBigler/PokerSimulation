using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerSimulation.Algorithms.KuhnPoker;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms;
using System.Diagnostics;

namespace PokerSimulation.Test.Algorithms
{
    [TestClass]
    public class KuhnPokerTrainerTest
    {
        private const int numberOfhands = 100000;
        private const float tolerance = 0.05f;

        private KuhnPokerTrainer trainer = null;

        [TestInitialize]
        public void Train()
        {
            trainer = new KuhnPokerTrainer();
            trainer.Train(numberOfhands);
            foreach(var node in trainer.GameNodes)
            {
                Debug.WriteLine(node.Value);
            }
        }

        /// <summary>
        /// This method verifies that some of player1's trained hands approximate a nash equilibrium
        /// </summary>
        [TestMethod]
        public void NashEquilibriumPlayer1Test()
        {
            // get information set (jack and first action)            
            var infoSet = new InformationSet<GameAction>()
            {
                CardBucket = (int)CardValue.Jack,
                Actions = new List<GameAction>() // No actions => first turn
            };

            var gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            var averageStrategy = gameNode.calculateAverageStrategy();

            // Jack's bet probability (alpha) must lie within 0 and 1/3 to be in nash equilibrium      
            var alpha = averageStrategy[(int)GameAction.Bet];
            float alphaMax = 1f / 3f;
            float alphaMin = 0f;
            Assert.IsTrue(alpha < (alphaMax + tolerance));
            Assert.IsTrue(alpha > (alphaMin - tolerance));

            // sum of bet and pass probability should be approximately one, since there are only two actions in Kuhn Poker
            var sum = averageStrategy[(int)GameAction.Pass] + averageStrategy[(int)GameAction.Bet];
            Assert.IsTrue(sum < 1 + tolerance && sum > 1 - tolerance);

            // alpha value is further used to evaluate nash equilibrium of player 1
            infoSet.CardBucket = (int)CardValue.King; // 3 = King            
            gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            averageStrategy = gameNode.calculateAverageStrategy();

            // King bet probability should be 3 * alpha
            var betProbability = averageStrategy[(int)GameAction.Bet];
            var betProbabilityExpected = (3 * alpha);
            Assert.IsTrue(Math.Abs(betProbability - betProbabilityExpected) < tolerance);

            infoSet.CardBucket = (int)CardValue.Queen;
            // player 1 checked first turn, Player 2 bet second turn.
            infoSet.Actions = new List<GameAction>() { GameAction.Pass, GameAction.Bet };
            gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            averageStrategy = gameNode.calculateAverageStrategy();

            // Queen should call (bet) with a probability of alpha + 1/3
            betProbability = averageStrategy[(int)GameAction.Bet];
            betProbabilityExpected = (alpha + 1f / 3f);
            Assert.IsTrue(Math.Abs(betProbability - betProbabilityExpected) < tolerance);
        }


        /// <summary>
        /// This method verifies that some of player2's trained hands approximate a nash equilibrium        
        /// </summary>
        [TestMethod]
        public void NashEquilibriumPlayer2Test()
        {
            // Get information set (jack and first action)            
            var infoSet = new InformationSet<GameAction>()
            {
                CardBucket = (int)CardValue.Jack,
                Actions = new List<GameAction>() { GameAction.Bet } // No actions => first turn
            };

            var gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            var averageStrategy = gameNode.calculateAverageStrategy();

            // Jack should never bet (i.e. call) after player 1 bet
            var betProbability = averageStrategy[(int)GameAction.Bet];
            var betProbabilityExpected = 0f;
            Assert.IsTrue(Math.Abs(betProbability - betProbabilityExpected) < tolerance);

            // Queen should bet (i.e. call) 1/3 after player 1 bet
            infoSet.CardBucket = (int)CardValue.Queen;
            gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            averageStrategy = gameNode.calculateAverageStrategy();
            betProbability = averageStrategy[(int)GameAction.Bet];
            betProbabilityExpected = (1f / 3f);
            Assert.IsTrue(Math.Abs(betProbability - betProbabilityExpected) < tolerance);


            // Jack should bet 1/3 of the time after being passed (i.e. checked) to
            infoSet.CardBucket = (int)CardValue.Jack;
            infoSet.Actions = new List<GameAction> { GameAction.Pass };
            gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            averageStrategy = gameNode.calculateAverageStrategy();
            betProbability = averageStrategy[(int)GameAction.Bet];
            betProbabilityExpected = (1f / 3f);
            Assert.IsTrue(Math.Abs(betProbability - betProbabilityExpected) < tolerance);
        }
    }
}
