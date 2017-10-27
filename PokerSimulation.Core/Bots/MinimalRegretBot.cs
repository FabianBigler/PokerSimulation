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

namespace PokerSimulation.Core.Bots
{
    public class MinimalRegretBot : Player
    {        
        private Dictionary<long, RegretGameNode<ActionBucket>> trainedTree;
        private byte handBucket;
        private List<ActionBucket> actionHistory;        
        

        public MinimalRegretBot(PlayerEntity entity) : base(entity)
        {
            var fs = new FileStream("cfr-tree.proto", FileMode.Open);
            trainedTree = Serializer.Deserialize<Dictionary<long, RegretGameNode<ActionBucket>>>(fs);                                 
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
            //if (this.Id == playerId) return;
            var actionBucket = ActionAbstracter.MapToBucket(action, amountToCall, currentGame.PotSize,amount);
            actionHistory.Add(actionBucket);         
        }

        private void Game_ChangedPhase(List<Card> board, GamePhase phase)
        {
            handBucket = (byte) HandStrengthAbstracter.MapToHandBucket(board, HoleCards);
        }     

        public override GameActionEntity GetAction(List<ActionType> possibleActions, int amountToCall)
        {               
            var infoSet = new InformationSet<ActionBucket>();
            infoSet.ActionHistory = actionHistory;
            infoSet.CardBucket = handBucket;

            RegretGameNode<ActionBucket> gameNode;
            trainedTree.TryGetValue(infoSet.GetLongHashCode(), out gameNode);
            if(gameNode == null)
            {
                throw new Exception("Information Set not found");                  
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

                    double newPercent = sumPercent + optimalStrategy[index];                    
                    if(randomValue >= sumPercent && randomValue <= optimalStrategy[index])
                    {
                        selectedActionBucket = action;
                        break;
                    }

                    sumPercent += optimalStrategy[index];
                    index++;
                }
                
                ActionType selectedAction= ActionAbstracter.MapToAction(selectedActionBucket, amountToCall);
                int betSize = 0;   
                if(selectedAction == ActionType.Bet || selectedAction == ActionType.Raise)
                {
                    betSize = ActionAbstracter.GetBetSize(selectedActionBucket, amountToCall, currentGame.PotSize);
                }

                if(!possibleActions.Contains(selectedAction))
                {
                    throw new Exception("Selected action is illegal!");
                }

                return new GameActionEntity
                {
                    ActionType = selectedAction,
                    Amount = betSize,
                    PlayerId = this.Id
                };
            }
        }         
    }
}
