using Microsoft.VisualStudio.TestTools.UnitTesting;
using PokerSimulation.Algorithms;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Test.Algorithms
{
    [TestClass]
    public class TexasHoldemCfrTreeTest
    {
        private static Dictionary<long, RegretGameNode<ActionBucket>> trainedTree;
        private float tolerance = 0.1f;
        
        /// <summary>
        /// Run LoadTree only once to save time
        /// </summary>
        /// <param name="context"></param>
        [ClassInitialize]
        public static void LoadTree(TestContext context)
        {
            var appDomain = System.AppDomain.CurrentDomain;
            var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
            var filePath = Path.Combine(basePath, "CFR", "cfr-tree.proto");
            var fs = new FileStream(filePath, FileMode.Open);
            trainedTree = Serializer.Deserialize<Dictionary<long, RegretGameNode<ActionBucket>>>(fs);
        }

        [TestMethod]
        public void Player1BestStartHandBucketTest()
        {
            var optimalStrategy = getOptimalStrategy((byte)StartHandBucket.Best, new List<ActionBucket>());
            float passProbability = getProbability(ActionBucket.Pass, optimalStrategy);
            float callProbability = getProbability(ActionBucket.Call, optimalStrategy);
            float betProbability = getProbability(ActionBucket.LowBet, optimalStrategy) +
                        getProbability(ActionBucket.MediumBet, optimalStrategy) +
                        getProbability(ActionBucket.HighBet, optimalStrategy);
            float totalProbability = passProbability + callProbability + betProbability;

            // given the best hand the bet probability should be higher than either call or pass probability           
            Assert.IsTrue(betProbability > passProbability);
            Assert.IsTrue(betProbability > callProbability);
            // call probability must be higher than pass probability
            Assert.IsTrue(callProbability > passProbability);            
        }

        [TestMethod]
        public void Player1WorstStartHandBucketTest()
        {
            var optimalStrategy = getOptimalStrategy((byte)StartHandBucket.Worst, new List<ActionBucket>());
            float passProbability = getProbability(ActionBucket.Pass, optimalStrategy);
            float callProbability = getProbability(ActionBucket.Call, optimalStrategy);
            float betProbability = getProbability(ActionBucket.LowBet, optimalStrategy) +
                                    getProbability(ActionBucket.MediumBet, optimalStrategy) +
                                    getProbability(ActionBucket.HighBet, optimalStrategy);
            float totalProbability = passProbability + callProbability + betProbability;

            //pass probability should be highest with worst hands
            Assert.IsTrue(passProbability > betProbability);
            Assert.IsTrue(passProbability > callProbability);
        }

        [TestMethod]
        public void Player2BestStartHandBucketAfterCallTest()
        {
            var optimalStrategy = getOptimalStrategy((byte)StartHandBucket.Best, new List<ActionBucket>() { ActionBucket.Call });        

            float passProbability = getProbability(ActionBucket.Pass, optimalStrategy);
            float callProbability = getProbability(ActionBucket.Call, optimalStrategy);
            float betProbability = getProbability(ActionBucket.LowBet, optimalStrategy) +
                        getProbability(ActionBucket.MediumBet, optimalStrategy) +
                        getProbability(ActionBucket.HighBet, optimalStrategy);
            float totalProbability = passProbability + callProbability + betProbability;

            // calling is not possible
            Assert.IsTrue(callProbability == 0);     
            // bet probability must be much higher than passing  
            Assert.IsTrue(betProbability > passProbability);            
        }

        [TestMethod]
        public void Player2WorstStartHandBucketAfterCallTest()
        {
            var optimalStrategy = getOptimalStrategy((byte)StartHandBucket.Worst, new List<ActionBucket>() { ActionBucket.Call });
            float passProbability = getProbability(ActionBucket.Pass, optimalStrategy);
            float callProbability = getProbability(ActionBucket.Call, optimalStrategy);
            float betProbability = getProbability(ActionBucket.LowBet, optimalStrategy) +
                                    getProbability(ActionBucket.MediumBet, optimalStrategy) +
                                    getProbability(ActionBucket.HighBet, optimalStrategy);
            float totalProbability = passProbability + callProbability + betProbability;

            //pass probability should be highest with worst hands
            Assert.IsTrue(passProbability > betProbability);
            Assert.IsTrue(passProbability > callProbability);
        }

        [TestMethod]
        public void WeakHandOnFlopTest()
        {
            // small bind bets, big blind calls, small blind bets again on the flop
            // big blind hits nothing (i.e. HighCardAce) and has to decide which action to do next
            var optimalStrategy = getOptimalStrategy((byte)HandStrengthBucket.HighCardAce, new List<ActionBucket>() { ActionBucket.MediumBet, ActionBucket.Call, ActionBucket.MediumBet });            
            float passProbability = getProbability(ActionBucket.Pass, optimalStrategy);
            float callProbability = getProbability(ActionBucket.Call, optimalStrategy);
            float betProbability = getProbability(ActionBucket.LowBet, optimalStrategy) +
                                    getProbability(ActionBucket.MediumBet, optimalStrategy) +
                                    getProbability(ActionBucket.HighBet, optimalStrategy);
            float totalProbability = passProbability + callProbability + betProbability;

            //pass probability should be highest with worst hands
            Assert.IsTrue(passProbability > betProbability);
            Assert.IsTrue(passProbability > callProbability);
        }

        [TestMethod]
        public void StrongHandOnFlopTest()
        {
            // small bind bets, big blind calls, small blind bets again on the flop
            // big blind hits nothing (i.e. HighCardAce) and has to decide which action to do next
            var optimalStrategy = getOptimalStrategy((byte)HandStrengthBucket.TopHands, new List<ActionBucket>() { ActionBucket.MediumBet, ActionBucket.Call, ActionBucket.MediumBet });
            float passProbability = getProbability(ActionBucket.Pass, optimalStrategy);
            float callProbability = getProbability(ActionBucket.Call, optimalStrategy);
            float betProbability = getProbability(ActionBucket.LowBet, optimalStrategy) +
                                    getProbability(ActionBucket.MediumBet, optimalStrategy) +
                                    getProbability(ActionBucket.HighBet, optimalStrategy);
            float totalProbability = passProbability + callProbability + betProbability;

            //call probability should be higher than pass probability        
            Assert.IsTrue(callProbability > passProbability);
        }
        

        [TestMethod]
        public void TotalProbabilityTest()
        {
            // take any handbucket and see if total probability is approx. 1
            var optimalStrategy = getOptimalStrategy((byte)StartHandBucket.Worst, new List<ActionBucket>());
            float passProbability = getProbability(ActionBucket.Pass, optimalStrategy);
            float callProbability = getProbability(ActionBucket.Call, optimalStrategy);
            float betProbability = getProbability(ActionBucket.LowBet, optimalStrategy) +
                                    getProbability(ActionBucket.MediumBet, optimalStrategy) +
                                    getProbability(ActionBucket.HighBet, optimalStrategy);
            float totalProbability = passProbability + callProbability + betProbability;
                        
            Assert.IsTrue(totalProbability < (1 + tolerance));
            Assert.IsTrue(totalProbability > (1 - tolerance));
        }

        [TestMethod]
        public void NumberOfStatesTest()
        {
            int expectedCount = 95716;
            Assert.IsTrue(trainedTree.Count == expectedCount);            
        }

        private List<float> getOptimalStrategy(byte handBucket, List<ActionBucket> actions)
        {
            var infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = handBucket,
                ActionHistory = actions
            };

            var gameNode = trainedTree[infoSet.GetHashCode()];
            Assert.IsNotNull(gameNode);
            return gameNode.calculateAverageStrategy();
        }

        private float getProbability(ActionBucket bucket, List<float> optimalStrategy)
        {
            bool isCallActionEnabled = (optimalStrategy.Count == 5);
            int passIndex = 0;
            int callIndex = 1;
            int lowBetIndex = 2;
            int mediumBetIndex = 3;
            int highBetIndex = 4;

            if (!isCallActionEnabled)
            {
                lowBetIndex = 1;
                mediumBetIndex = 2;
                highBetIndex = 3;
            }

            float passProbability = optimalStrategy[passIndex];
            float callProbability = optimalStrategy[callIndex];
            float betProbability = optimalStrategy[lowBetIndex] + optimalStrategy[mediumBetIndex] + optimalStrategy[highBetIndex];
            switch (bucket)
            {
                case ActionBucket.Pass:
                    return optimalStrategy[passIndex];
                case ActionBucket.Call:
                    if (isCallActionEnabled)
                    {
                        return optimalStrategy[callIndex];
                    }
                    else
                    {
                        return 0;
                    }
                case ActionBucket.HighBet:
                    return optimalStrategy[highBetIndex];
                case ActionBucket.LowBet:
                    return optimalStrategy[lowBetIndex];
                case ActionBucket.MediumBet:
                    return optimalStrategy[mediumBetIndex];
                default:
                    throw new NotImplementedException("ActionBucket not implemented!");
            }
        }
    }
}
