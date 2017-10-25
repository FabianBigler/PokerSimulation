using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Helpers;
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

            for (int i = 0; i < numberOfHands; i++)
            {
                var playedHand = new PlayedHandEntity();
                dealer.DealHoleCards(players);

                //deal board already
                var board = new List<Card>();
                dealer.DealFlop(board);
                dealer.DealTurn(board);
                dealer.DealRiver(board);

                byte startHandBucket1 = (byte)StartHandAbstracter.FromStartHand(player1.HoleCards[0], player1.HoleCards[1]);
                byte startHandBucket2 = (byte)StartHandAbstracter.FromStartHand(player2.HoleCards[0], player2.HoleCards[1]);

                var handBuckets = new List<byte>() { startHandBucket1, startHandBucket1 }.ToArray();
                var state = new HeadsUpGameState();
                state.Phase = GamePhase.PreFlop;
                state.Board = board;
                state.Player1HoleCards = player1.HoleCards;
                state.Player2HoleCards = player2.HoleCards;
                state.PotSize = HeadsupGame.BigBlindSize + HeadsupGame.SmallBlindSize;
                state.AmountToCall = 1;

                CalculateCounterFactualRegret(state, handBuckets, new List<ActionBucket>(), 1, 1);
            }       

            throw new NotImplementedException();
        }

        private float CalculateCounterFactualRegret(HeadsUpGameState gameState, byte[] handBuckets, List<ActionBucket> actions, float probabilityPlayer1, float probabilityPlayer2)
        {
            int plays = actions.Count;
            int playerIndex = plays % 2;
            var newState = gameState.GetCopy();
            bool nextActionCallEnabled = true;   

            ActionBucket lastAction = actions.LastOrDefault();
            ActionBucket secondLastAction = ActionBucket.None;
            if(actions.Count > 1) secondLastAction = actions[actions.Count - 2];           

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

                    if (secondLastAction == ActionBucket.Pass)
                    {
                        newState.NextPhase(newState.Phase);
                        phaseChanged = true;                                            
                    }

                    nextActionCallEnabled = false;
                    break;
                case ActionBucket.Call:
                    newState.PotSize += newState.AmountToCall;
                    newState.NextPhase(newState.Phase);
                    newState.AmountToCall = 0;
                    phaseChanged = true;
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

                    betSize = ActionAbstracter.GetBetSize(lastAction, newState.AmountToCall, newState.PotSize);
                    if (betSize > 0)
                    {
                        newState.AmountToCall = betSize;
                        newState.PotSize += betSize;
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

                    handBuckets[0] = (byte)HandStrengthAbstracter.MapToHandBucket(currentBoard, newState.Player1HoleCards);
                    handBuckets[1] = (byte)HandStrengthAbstracter.MapToHandBucket(currentBoard, newState.Player2HoleCards);
                }
            }

            var infoSet = new InformationSet<ActionBucket>()
            {
                CardBucket = handBuckets[playerIndex],
                Actions = actions
            };

            int numberOfActions = Settings.NumberOfActions;
            if (!nextActionCallEnabled)
            {
                numberOfActions = Settings.NumberOfActions - 1;
            }

            RegretGameNode<ActionBucket> node = null;
            var hash = infoSet.GetHashCode();
            if (!GameNodes.TryGetValue(hash, out node))
            {              
                node = new RegretGameNode<ActionBucket>(numberOfActions);
                node.InfoSet = infoSet;
                GameNodes.Add(hash, node);
            }

            var strategy = node.calculateStrategy(playerIndex == 0 ? probabilityPlayer1 : probabilityPlayer2);
            var utilities = new List<float>(Settings.NumberOfActions) { 0, 0 };
            float nodeUtility = 0;

            int index = 0;

            foreach (ActionBucket nextAction in Enum.GetValues(typeof(ActionBucket)))
            {
                if (nextAction == ActionBucket.None) continue;
                if (nextAction == ActionBucket.Call && !nextActionCallEnabled) continue;                

                var nextHistory = new List<ActionBucket>();
                nextHistory.Add(nextAction);
                
                utilities[index] = playerIndex == 0
                    ? -CalculateCounterFactualRegret(newState, handBuckets, nextHistory, probabilityPlayer1 * strategy[index], probabilityPlayer2)
                   : -CalculateCounterFactualRegret(newState, handBuckets, nextHistory, probabilityPlayer1, probabilityPlayer2 * strategy[index]);

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
