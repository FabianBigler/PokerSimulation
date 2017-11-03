using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Helpers;
using PokerSimulation.Game.Interfaces;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem
{
    public class TexasHoldemTrainer : ITrainer
    {
        //Dictionary of long to store the hash codes and access them in O(1).
        public Dictionary<long, RegretGameNode<ActionBucket>> GameNodes { get; set; }
        private Player dummyPlayer1;
        private Player dummyPlayer2;
        private List<Player> players;
        private ICanDeal dealer;

        public TexasHoldemTrainer(ICanDeal dealer)
        {
            this.dealer = dealer;
            GameNodes = new Dictionary<long, RegretGameNode<ActionBucket>>();

            //dealer needs two players to deal cards to            
            dummyPlayer1 = new Player();
            dummyPlayer2 = new Player();
            players = new List<Player>() { dummyPlayer1, dummyPlayer2 };
        }

        public void Train(int numberOfHands)
        {
            for (int i = 0; i < numberOfHands; i++)
            {
                dealer.DealHoleCards(players);
                // deal board at the beginning of each hand already for training      
                var board = new List<Card>();
                dealer.DealFlop(board);
                dealer.DealTurn(board);
                dealer.DealRiver(board);

                StartHandBucket startHandBucket1 = StartHandAbstracter.FromStartHand(dummyPlayer1.HoleCards[0], dummyPlayer1.HoleCards[1]);
                StartHandBucket startHandBucket2 = StartHandAbstracter.FromStartHand(dummyPlayer2.HoleCards[0], dummyPlayer2.HoleCards[1]);
                var handBuckets = new List<byte>() { (byte)startHandBucket1, (byte)startHandBucket2 }.ToArray();
                var initialState = new HeadsUpGameState()
                {
                    Phase = GamePhase.PreFlop,
                    Board = board,
                    Player1HoleCards = dummyPlayer1.HoleCards,
                    Player2HoleCards = dummyPlayer2.HoleCards,
                    PotSize = HeadsupGame.BigBlindSize + HeadsupGame.SmallBlindSize,
                    AmountToCall = HeadsupGame.SmallBlindSize
                };

                CalculateCounterFactualRegret(initialState, handBuckets, new List<ActionBucket>(), 1, 1, 0);
            }
        }

        /// <summary>
        /// Traverses the sub tree of the passed hand buckets recursively. Calculates and then backpropagates the counter factual regret for each node.        
        /// </summary>
        /// <param name="gameState"></param>
        /// <param name="handBuckets"></param>
        /// <param name="actions"></param>
        /// <param name="probabilityPlayer1"></param>
        /// <param name="probabilityPlayer2"></param>
        /// <returns></returns>
        private float CalculateCounterFactualRegret(HeadsUpGameState gameState, byte[] handBuckets, List<ActionBucket> actions, float probabilityPlayer1, float probabilityPlayer2, int depth)
        {
            int plays = actions.Count;
            int playerIndex = plays % 2;
            var newState = gameState.GetCopy();
            ActionBucket lastAction = ActionBucket.None;
            if (actions.Count > 0) lastAction = actions[plays - 1];
            ActionBucket secondLastAction = ActionBucket.None;
            if (actions.Count > 1) secondLastAction = actions[plays - 2];
            depth++;            

            bool nextActionCallEnabled = true;
            bool phaseChanged = false;
            switch (lastAction)
            {
                case ActionBucket.Pass:
                    if (secondLastAction == ActionBucket.LowBet ||
                        secondLastAction == ActionBucket.HighBet ||
                        secondLastAction == ActionBucket.MediumBet)
                    {
                        int payoff = (newState.PotSize - newState.AmountToCall) / 2;
                        return (playerIndex == 0) ? payoff : -payoff;
                    }

                    switch (secondLastAction)
                    {
                        case ActionBucket.None:
                            return (playerIndex == 0) ? HeadsupGame.SmallBlindSize : -HeadsupGame.SmallBlindSize;
                        case ActionBucket.Call:
                            newState.SetNextPhase(newState.Phase);
                            phaseChanged = true;
                            break;
                        case ActionBucket.Pass:
                            //check if last pass count is dividable by 2 (then it's a new phase)
                            int lastActionPassCount = 0;                            
                            for (int i = actions.Count - 1; i >= 0; i--)
                            {
                                if (actions[i] == ActionBucket.Pass)
                                {
                                    lastActionPassCount++;
                                }
                                else
                                {
                                    //special case: if it's first round, call pass results in ending the round
                                    if(actions[i] == ActionBucket.Call && i == 0)
                                    {                                        
                                        lastActionPassCount--;                                        
                                    }
                                    break;
                                }
                            }

                            if (lastActionPassCount % 2 == 0)
                            {
                                newState.SetNextPhase(newState.Phase);
                                phaseChanged = true;
                            }
                            break;
                    }
                    nextActionCallEnabled = false;
                    break;
                case ActionBucket.Call:
                    newState.PotSize += newState.AmountToCall;
                    newState.AmountToCall = 0;
                    //special case for first round: big blind needs to check or bet
                    if (actions.Count == 1)
                    {
                        nextActionCallEnabled = false;
                    }
                    else
                    {
                        newState.SetNextPhase(newState.Phase);                        
                        phaseChanged = true;
                    }
                    break;
                case ActionBucket.HighBet:
                case ActionBucket.MediumBet:
                case ActionBucket.LowBet:
                    int betSize = 0;
                    if (newState.AmountToCall > 0 && newState.AmountToCall == HeadsupGame.SmallBlindSize)
                    {
                        //exception: first round
                        newState.PotSize += HeadsupGame.SmallBlindSize;
                    }

                    int lastActionLowBetCount = 0;
                    for (int i = actions.Count - 1; i >= 0; i--)
                    {
                        if (actions[i] == ActionBucket.LowBet)
                        {
                            lastActionLowBetCount++;
                        }
                        else
                        {                            
                            break;
                        }
                    }

                    betSize = ActionAbstracter.GetBetSize(lastAction, newState.AmountToCall, newState.PotSize);
                    if (betSize > 0)
                    {                                   
                        newState.AmountToCall = betSize;                        
                        newState.PotSize += betSize;
                        if(newState.PotSize >= HeadsupGame.StackSize * 2)
                        {
                            phaseChanged = true;
                            newState.Phase = GamePhase.ShowDown;
                        }
                    }
                    else
                    {
                        phaseChanged = true;
                        newState.Phase = GamePhase.ShowDown;
                    }
                    break;
            }

            if (phaseChanged)
            {
                if (newState.Phase == GamePhase.ShowDown)
                {
                    int payoff = newState.PotSize / 2;
                    var handComparison = HandComparer.Compare(newState.Player1HoleCards, newState.Player2HoleCards, newState.Board);
                    switch (handComparison)
                    {
                        case HandComparison.None:
                            return 0;
                        case HandComparison.Player1Won:
                            return (playerIndex == 0) ? payoff : -payoff;
                        case HandComparison.Player2Won:
                            return (playerIndex == 0) ? -payoff : payoff;
                    }
                }
                else
                {
                    nextActionCallEnabled = false;

                    List<Card> currentBoard = null;
                    switch (newState.Phase)
                    {
                        case GamePhase.Flop:
                            currentBoard = gameState.Board.Take(3).ToList();
                            break;
                        case GamePhase.Turn:
                            currentBoard = gameState.Board.Take(4).ToList();
                            break;
                        case GamePhase.River:
                            currentBoard = gameState.Board;
                            break;
                    }
                    
                    byte bucket1 = (byte)HandStrengthAbstracter.MapToHandBucket(currentBoard, newState.Player1HoleCards);
                    byte bucket2 = (byte)HandStrengthAbstracter.MapToHandBucket(currentBoard, newState.Player2HoleCards);                    
                    handBuckets = new byte[] { bucket1, bucket2};
                }
            }

            var infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = handBuckets[playerIndex],
                ActionHistory = actions
            };

            int numberOfActions = Settings.NumberOfActions;
            if (!nextActionCallEnabled)
            {
                numberOfActions = Settings.NumberOfActions - 1;
            }

            RegretGameNode<ActionBucket> node = null;
            long hash = infoSet.GetLongHashCode();
            if (!GameNodes.TryGetValue(hash, out node))
            {
                node = new RegretGameNode<ActionBucket>(numberOfActions);
                node.InfoSet = infoSet;
                GameNodes.Add(hash, node);
            }

            var strategy = node.calculateStrategy(playerIndex == 0 ? probabilityPlayer1 : probabilityPlayer2);

            // initialise list with zeros
            var utilities = new List<float>(numberOfActions);
            for (int i = 0; i < numberOfActions; i++)
            {
                utilities.Add(0);
            }

            float nodeUtility = 0;
            int index = 0;
            foreach (ActionBucket nextAction in Enum.GetValues(typeof(ActionBucket)))
            {
                //skip illegal actions
                if (nextAction == ActionBucket.None) continue;
                if (nextAction == ActionBucket.Call && !nextActionCallEnabled) continue;

                if(depth == 2)
                {
                    Debug.WriteLine(nextAction);
                    if (nextAction == ActionBucket.LowBet)
                    {

                    }
                }
                var nextHistory = new List<ActionBucket>();
                nextHistory.AddRange(actions.ToArray());
                nextHistory.Add(nextAction);                                

                utilities[index] = playerIndex == 0
                    ? -CalculateCounterFactualRegret(newState, handBuckets, nextHistory, probabilityPlayer1 * strategy[index], probabilityPlayer2, depth)
                   : -CalculateCounterFactualRegret(newState, handBuckets, nextHistory, probabilityPlayer1, probabilityPlayer2 * strategy[index], depth);

                nodeUtility += strategy[index] * utilities[index];
                index++;
            }

            for (int i = 0; i < numberOfActions; i++)
            {
                float regret = utilities[i] - nodeUtility;
                node.RegretSum[i] += (playerIndex == 0 ? probabilityPlayer2 : probabilityPlayer1) * regret;
            }

            return nodeUtility;
        }
    }
}
