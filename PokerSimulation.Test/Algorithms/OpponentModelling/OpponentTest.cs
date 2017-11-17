using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms.TexasHoldem.OpponentModelling;
using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Test.Algorithms.OpponentModelling
{
    [TestClass]
    public class OpponentTest
    {
        private Opponent opponent;
        [TestInitialize]
        public void Initialize()
        {
            var serializer = new OpponentSerializer();
            opponent = serializer.Deserialize();
        }

        [TestMethod]
        public void UpdateFeaturesAfterActionTest()
        {
            // cbet 2 of 3 times
            var lastActions = new List<FeatureAction>() { FeatureAction.Bet };
            opponent.StartNewhand();
            opponent.UpdateFeaturesAfterAction(lastActions, GamePhase.Flop, 
                Aggression.IsLast, Positioning.InPosition);
            opponent.StartNewhand();
            opponent.UpdateFeaturesAfterAction(lastActions, GamePhase.Flop,
                Aggression.IsLast, Positioning.InPosition);
            lastActions = new List<FeatureAction>() { FeatureAction.Pass };
            opponent.StartNewhand();
            opponent.UpdateFeaturesAfterAction(lastActions, GamePhase.Flop,
              Aggression.IsLast, Positioning.InPosition);

            var featureToCheck = opponent.Features.FirstOrDefault(x => x.Name == FeatureNames.ContinuationBetIp);
            double expected = (double) 2 / 3;
            Assert.AreEqual(expected, featureToCheck.Value);
        }

        [TestMethod]
        public void UpdateShowdownAfterActionTest()
        {
            //6 hands played
            //4 went to showdown
            //won showdown 2 of 3. 1 split pot (ignored)
            opponent.StartNewhand();
            opponent.StartNewhand();            
            opponent.StartNewhand();
            opponent.UpdateFeaturesAfterShowdown(null);
            opponent.StartNewhand();
            opponent.UpdateFeaturesAfterShowdown(true);
            opponent.StartNewhand();
            opponent.UpdateFeaturesAfterShowdown(true);
            opponent.StartNewhand();
            opponent.UpdateFeaturesAfterShowdown(false);

            var featureToCheck = opponent.Features.First(x => x.Name == FeatureNames.WonShowdown);
            double expected = (double)2 / 3;
            Assert.AreEqual(expected, featureToCheck.Value);

            featureToCheck = opponent.Features.First(x => x.Name == FeatureNames.WentToShowdown);
            expected = (double)4 / 6;
            Assert.AreEqual(expected, featureToCheck.Value);
        }
    }
}
