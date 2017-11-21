using System.Collections.Generic;
using PokerSimulation.Core.Entities;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Algorithms;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Core.Helpers;
using System;
using System.Linq;
using PokerSimulation.Game.Model;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game;
using ProtoBuf;
using System.IO;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Bots
{
    public class MinimalRegretBot : Player
    {        
        private Dictionary<long, RegretGameNode<ActionBucket>> trainedTree;
        private byte handBucket;
        private List<ActionBucket> actionHistory;
        Object thisLock = new Object();


        public MinimalRegretBot(PlayerEntity entity) : base(entity)
        {
            lock(thisLock)
            {
                var appDomain = System.AppDomain.CurrentDomain;
                var basePath = appDomain.RelativeSearchPath ?? appDomain.BaseDirectory;
                var filePath = Path.Combine(basePath, "CFR", "cfr-tree.proto");
                using (var fs = new FileStream(filePath, FileMode.Open))
                {
                    trainedTree = Serializer.Deserialize<Dictionary<long, RegretGameNode<ActionBucket>>>(fs);
                }                    
            }           
        }

        public override void DealHoleCards(Card card1, Card card2)
        {
            base.DealHoleCards(card1, card2);
            actionHistory = new List<ActionBucket>();
            handBucket = (byte) StartHandAbstracter.FromStartHand(card1, card2);
        }

        public override void AssignCurrentGame(HeadsupGame game)
        {
            base.AssignCurrentGame(game);           
            game.ChangedPhase += Game_ChangedPhase;
            game.PlayerActed += Game_PlayerActed;
        }


        private void Game_PlayerActed(Guid playerId, ActionType action, int amountToCall, int amount)
        {            
            var actionBucket = ActionAbstracter.MapToBucket(action, amountToCall, currentGame.PotSize,amount);
            actionHistory.Add(actionBucket);         
        }

        private void Game_ChangedPhase(List<Card> board, GamePhase phase)
        {
            handBucket = (byte) HandStrengthAbstracter.MapToHandBucket(board, HoleCards);
        }     

        public override Task<GameActionEntity> GetAction(List<ActionType> possibleActions, int amountToCall)
        {               
            var infoSet = new InformationSet<ActionBucket>();
            infoSet.ActionHistory = actionHistory;
            infoSet.CardBucket = handBucket;

            RegretGameNode<ActionBucket> gameNode;
            trainedTree.TryGetValue(infoSet.GetLongHashCode(), out gameNode);
            if(gameNode == null)
            {
                //throw new Exception("Information Set not found"); 
                return Task.FromResult<GameActionEntity>(new GameActionEntity
                {
                    ActionType = possibleActions[0],
                    Amount = 0,
                    PlayerId = this.Id
                });
            } else
            {
                var optimalStrategy = gameNode.calculateAverageStrategy();
                var rand = new Random();
                double randomValue = rand.NextDouble();                
                bool isCallActionEnabled = (optimalStrategy.Count == 5);

                int index = 0;
                double sumPercent = 0;
                ActionBucket selectedActionBucket = ActionBucket.None;
                foreach (ActionBucket action in Enum.GetValues(typeof(ActionBucket)))
                {
                    if (action == ActionBucket.None) continue;
                    if (action == ActionBucket.Call && !isCallActionEnabled) continue;

                    double newSumPercent = sumPercent + optimalStrategy[index];                    
                    if(randomValue >= sumPercent && randomValue <= newSumPercent)
                    {
                        selectedActionBucket = action;
                        break;
                    }

                    sumPercent = newSumPercent;
                    index++;
                }
                
                ActionType selectedAction= ActionAbstracter.MapToAction(selectedActionBucket, amountToCall);
                int betSize = 0;   
                switch(selectedAction)
                {
                    case ActionType.Bet:
                    case ActionType.Raise:
                        betSize = ActionAbstracter.GetBetSize(selectedActionBucket, amountToCall, currentGame.PotSize);
                        break;
                    case ActionType.Call:
                        betSize = amountToCall;
                        break;
                }
          

                if(!possibleActions.Contains(selectedAction))
                {
                    // in all-in scenarios actions from bot may differ from possible actions
                    switch (selectedAction)
                    {
                        case ActionType.Bet:
                        case ActionType.Raise:
                        case ActionType.Check:
                            if (possibleActions.Contains(ActionType.Call))
                            {
                                selectedAction = ActionType.Call;
                            }
                            break;
                        default:
                            throw new Exception("Selected action is illegal!");
                    }                               
                }

                if (ChipStack < betSize) betSize = this.ChipStack;

                return Task.FromResult<GameActionEntity>(new GameActionEntity
                {
                    ActionType = selectedAction,
                    Amount = betSize,
                    PlayerId = this.Id
                });
            }
        }         
    }
}
