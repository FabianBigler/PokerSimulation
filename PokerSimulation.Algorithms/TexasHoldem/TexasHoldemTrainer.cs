using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem
{
    public class TexasHoldemTrainer : ITrainer
    {
        public Dictionary<int, RegretGameNode<ActionBucket>> GameNodes { get; private set; }

        public void Train(int numberOfHands)
        {
            GameNodes = new Dictionary<int, RegretGameNode<ActionBucket>>();

            var playerEntity1 = new PlayerEntity();
            var player1 = new Player(playerEntity1);
            var player2 = new Player(playerEntity1);
            var players = new List<Player>();
            players.Add(player1);
            players.Add(player2);

            var dealer = new RandomDealer();            
            var game = new HeadsupGame(players, dealer);

            for (int i = 0; i < numberOfHands; i++)
            {
                var playedHand = new PlayedHandEntity();
                game.StartNewRound(playedHand);

                byte startHandBucket1 = (byte)StartHandAbstracter.FromStartHand(player1.HoleCards[0], player1.HoleCards[1]);
                byte startHandBucket2 = (byte)StartHandAbstracter.FromStartHand(player2.HoleCards[0], player2.HoleCards[1]);

                var handBuckets = new List<byte>() { startHandBucket1, startHandBucket1 }.ToArray();
                var state = new HeadsUpGameState();
                state.Phase = GamePhase.PreFlop;
                CalculateCounterFactualRegret(state, handBuckets, new List<ActionBucket>(), 1, 1);
            }            

            //int[] cards = { (int)CardValue.Jack, (int)CardValue.Queen, (int)CardValue.King };
            //float util = 0;
            //var rand = new Random();

            //for (int i = 0; i < numberOfhands; i++)
            //{
            //    var shuffled = cards.OrderBy(x => rand.Next()).ToArray();
            //    util += CalculateCounterFactualRegret(shuffled, new List<GameAction>(), 1, 1);
            //}

            throw new NotImplementedException();
        }

        private float CalculateCounterFactualRegret(HeadsUpGameState gameState, byte[] handBuckets, List<ActionBucket> actions, float probabilityPlayer1, float probabilityPlayer2)
        {
            int plays = actions.Count;
            int playerIndex = plays % 2;            
            

            //flop, turn, river
            //amount
            if(actions.Count > 1)
            {
                var lastAction = actions.LastOrDefault();
                var secondLastAction = actions[actions.Count - 2];

                switch (lastAction)
                {
                    case ActionBucket.Pass:
                        if (secondLastAction == ActionBucket.LowBet ||
                                            secondLastAction == ActionBucket.HighBet ||
                                            secondLastAction == ActionBucket.MediumBet)
                        {
                            return gameState.PotSize / 2;
                        }

                        if(secondLastAction == ActionBucket.Pass)
                        {
                            //next betting round or showdown                                                                                                                                                                                                                                                                                        
                        }
                        
                                                

                        break;
                    case ActionBucket.Call:
                        
                        break;
                    case ActionBucket.HighBet:
                    case ActionBucket.MediumBet:
                    case ActionBucket.LowBet:
                        //ActionAbstracter.ToAction(lastAction, game.PotSize);

                        break;
                }                                     
            }

            //if (plays > 1)
            //{
            //    bool isLastActionPass = (actions.Last() == GameAction.Pass);
            //    bool isSecondLastActionPass = (actions[actions.Count - 2] == GameAction.Pass);
            //    bool isPlayerCardHigher = cards[player] > cards[opponent];
            //    bool isDoubleBet = !isLastActionPass && !isSecondLastActionPass;
            //    bool isDoublePass = isLastActionPass && isSecondLastActionPass;

            //    if (isLastActionPass)
            //    {
            //        if (isDoublePass)
            //        {
            //            return isPlayerCardHigher ? 1 : -1;
            //        }
            //        else
            //        {
            //            return 1;
            //        }
            //    }
            //    else if (isDoubleBet)
            //    {
            //        return isPlayerCardHigher ? 2 : -2;
            //    }
            //}

            var infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = handBuckets[playerIndex],
                Actions = actions
            };

            RegretGameNode<ActionBucket> node = null;
            var hash = infoSet.GetHashCode();
            if (!GameNodes.TryGetValue(hash, out node))
            {
                node = new RegretGameNode<ActionBucket>(Settings.NumberOfActions);
                node.InfoSet = infoSet;
                GameNodes.Add(hash, node);
            }

            var strategy = node.calculateStrategy(playerIndex == 0 ? probabilityPlayer1 : probabilityPlayer2);
            var utilities = new List<float>(Settings.NumberOfActions) { 0, 0 };
            float nodeUtility = 0;

            int index = 0;
            foreach (ActionBucket nextAction in Enum.GetValues(typeof(ActionBucket)))
            {
                var nextHistory = new List<ActionBucket>();
                nextHistory.Add(nextAction);
                
                utilities[index] = playerIndex == 0
                    ? -CalculateCounterFactualRegret(gameState, handBuckets, nextHistory, probabilityPlayer1 * strategy[index], probabilityPlayer2)
                   : -CalculateCounterFactualRegret(gameState, handBuckets, nextHistory, probabilityPlayer1, probabilityPlayer2 * strategy[index]);

                nodeUtility += strategy[index] * utilities[index];            
                index++;
            }

            for (int i = 0; i < Settings.NumberOfActions; i++)
            {
                float regret = utilities[i] - nodeUtility;
                node.RegretSum[i] += (playerIndex == 0 ? probabilityPlayer2 : probabilityPlayer1) * regret;
            }

            return nodeUtility;
        }

        private void Game_ChangedPhase(List<Game.Model.Card> board, Game.Enumerations.GamePhase phase)
        {
            throw new NotImplementedException();
        }
    }
}
