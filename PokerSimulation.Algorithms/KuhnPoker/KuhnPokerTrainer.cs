﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.KuhnPoker
{
    public class KuhnPokerTrainer : ITrainer
    {       
        public Dictionary<int, RegretGameNode<GameAction>> GameNodes { get; private set; }

        /// <summary>
        /// Trains the exposed GameNodes property for the passed number of hands.
        /// After each played hand the deck will be shuffled, the cards dealt randomly.
        /// </summary>
        /// <param name="numberOfhands">The number of hands the trainer has to train.</param>
        public void Train(int numberOfhands)
        {
            GameNodes = new Dictionary<int, RegretGameNode<GameAction>>();

            int[] cards = { (int) CardValue.Jack, (int) CardValue.Queen, (int) CardValue.King };
            float util = 0;
            var rand = new Random();

            for (int i = 0; i < numberOfhands; i++)
            {                
                var shuffled = cards.OrderBy(x => rand.Next()).ToArray();
                util += CalculateCounterFactualRegret(shuffled, new List<GameAction>(), 1, 1);
            }
        }

        /// <summary>
        /// Recursively implements the Counterfactual Regret Minimization algorithm
        /// 
        /// </summary>
        /// <param name="cards">Hand cards of player 1 and 2</param>
        /// <param name="actions">Action History</param>
        /// <param name="probability0">Accumulated action probability of player 1</param>
        /// <param name="probability1">Accumulated action probability of player 2</param>
        /// <returns></returns>
        private float CalculateCounterFactualRegret(int[] cards, List<GameAction> actions, float probability0, float probability1)
        {
            int plays = actions.Count;
            int player = plays % 2;
            int opponent = 1 - player;

            if (plays > 1)
            {
                bool isLastActionPass = (actions.Last() == GameAction.Pass);
                bool isSecondLastActionPass = (actions[actions.Count - 2] == GameAction.Pass);
                bool isPlayerCardHigher = cards[player] > cards[opponent];
                bool isDoubleBet = !isLastActionPass && !isSecondLastActionPass;
                bool isDoublePass = isLastActionPass && isSecondLastActionPass;
              
                if (isLastActionPass)
                {
                    if (isDoublePass)
                    {
                        return isPlayerCardHigher ? 1 : -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                else if (isDoubleBet)
                {
                    return isPlayerCardHigher ? 2 : -2;               
                }
            }

            var infoSet = new InformationSet<GameAction>()
            {
                CardBucket = cards[player],
                ActionHistory = actions
            };

            RegretGameNode<GameAction> node = null;
            var hash = infoSet.GetHashCode();            
            if (!GameNodes.TryGetValue(hash, out node))
            {
                node = new RegretGameNode<GameAction>(Settings.NumberOfActions);
                node.InfoSet = infoSet;
                GameNodes.Add(hash, node);
            }

            var strategy = node.calculateStrategy(player == 0 ? probability0 : probability1);
            var utilities = new List<float>(Settings.NumberOfActions) { 0, 0 };
            float nodeUtility = 0;

            for (int i = 0; i < Settings.NumberOfActions; i++)
            {                
                var nextAction = (i == 0) ? GameAction.Pass : GameAction.Bet;                
                var nextHistory = new List<GameAction>(actions);
                nextHistory.Add(nextAction);
                
                utilities[i] = player == 0
                    ? -CalculateCounterFactualRegret(cards, nextHistory, probability0 * strategy[i], probability1)
                   : -CalculateCounterFactualRegret(cards, nextHistory, probability0, probability1 * strategy[i]);

                nodeUtility += strategy[i] * utilities[i];
            }

            for (int i = 0; i < Settings.NumberOfActions; i++)
            {
                float regret = utilities[i] - nodeUtility;                
                node.RegretSum[i] += (player == 0 ? probability1 : probability0) * regret;
            }

            return nodeUtility;
        }        
    }
}