using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms;
using PokerSimulation.Algorithms.TexasHoldem;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
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
        private const int numberOfhands = 1;
        private const float tolerance = 0.01f;

        private TexasHoldemTrainer trainer = null;

        [TestInitialize]
        public void Train()
        {
            trainer = new TexasHoldemTrainer();
            trainer.Train(numberOfhands);
            foreach (var node in trainer.GameNodes)
            {
                Debug.WriteLine(node.Value);
            }
        }

        [TestMethod]
        public void NashEquilibriumPlayer1Test()
        {
            var infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = (int)StartHandBucket.VeryGood,
                ActionHistory = new List<ActionBucket>()
            };

            var gameNode = trainer.GameNodes[infoSet.GetHashCode()];
            var averageStrategy = gameNode.calculateAverageStrategy();

        }
    }
}
