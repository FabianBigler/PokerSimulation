using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Algorithms.TexasHoldem.OpponentModelling;
using System.Xml.Serialization;
using System.IO;
using PokerSimulation.Game;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;

namespace PokerSimulation.Core.Bots
{
    public class OpponentModellingBot : Player
    {
        private Opponent opponent;
        private RandomBot randomBot;

        private HandStrengthBucket handStrengthBucket;
        private StartHandBucket startHandBucket;
        private List<ActionBucket> actionHistory;
        private List<ActionHistoryPerPhase> actionHistoriesPerPhase;


        public OpponentModellingBot(PlayerEntity entity) : base(entity)
        {
            var serializer = new OpponentSerializer();
            this.opponent = serializer.Deserialize();

            randomBot = new RandomBot(entity);
        }

        public override Task<GameActionEntity> GetAction(List<ActionType> possibleActions, int amountToCall)
        {
            var possibleFeatureActions = new List<FeatureAction>();
            foreach (var possibleAction in possibleActions)
            {
                var featureAction = FeatureActionMapper.FromActionType(possibleAction);
                if (!possibleFeatureActions.Contains(featureAction))
                {
                    possibleFeatureActions.Add(featureAction);
                }
            }

            var currentPhasehistory = actionHistoriesPerPhase.FirstOrDefault(x => x.Phase == currentGame.Phase);
            Positioning opponentPositioning = Positioning.None;
            Aggression opponentAggression = Aggression.None;
            if (currentGame.Phase > GamePhase.PreFlop)
            {
                opponentPositioning = getOpponentPositioning(currentPhasehistory.ActionHistory.Count);
                opponentAggression = getOpponentAggression(opponentPositioning);
            }

            var counterAction = opponent.GetCounterAction(possibleFeatureActions, currentPhasehistory.ActionHistory, currentGame.Phase, opponentAggression, opponentPositioning);
            ActionBucket selectedActionBucket = ActionBucket.None;
            if (counterAction == null)
            {
                if (currentGame.Phase == GamePhase.PreFlop)
                {
                    switch (opponent.PlayStyle)
                    {
                        case PlayStyle.LooseAggressive:
                        case PlayStyle.LoosePassive:
                            //play TightAggressive
                            if (this.IsBigBlind)
                            {                                
                                if (startHandBucket >= StartHandBucket.VeryBad)
                                {
                                    selectedActionBucket = ActionBucket.LowBet;
                                }
                                else
                                {
                                    selectedActionBucket = ActionBucket.Pass;
                                }
                            } else
                            {                                
                                if (startHandBucket >= StartHandBucket.Bad)
                                {
                                    selectedActionBucket = ActionBucket.LowBet;
                                }
                                else
                                {
                                    selectedActionBucket = ActionBucket.Pass;
                                }
                            }
                      
                            break;                        
                        case PlayStyle.TightAggressive:
                        case PlayStyle.TightPassive:
                            if (this.IsBigBlind)
                            {
                                selectedActionBucket = ActionBucket.LowBet;
                            }
                            else
                            {
                                if (startHandBucket >= StartHandBucket.VeryBad)
                                {
                                    selectedActionBucket = ActionBucket.LowBet;
                                }
                                else
                                {
                                    selectedActionBucket = ActionBucket.Pass;
                                }
                            }                      
                            break;
                    }
                }
                else
                {
                    switch (opponent.PlayStyle)
                    {
                        case PlayStyle.LooseAggressive:
                            //play TightAggressive
                            if (handStrengthBucket >= HandStrengthBucket.LowPair)
                            {
                                selectedActionBucket = ActionBucket.LowBet;
                            }
                            else
                            {
                                selectedActionBucket = ActionBucket.Pass;
                            }
                            break;
                        case PlayStyle.LoosePassive:
                            //play TightAggressive
                            if (handStrengthBucket >= HandStrengthBucket.LowPair)
                            {
                                selectedActionBucket = ActionBucket.LowBet;
                            }
                            else
                            {
                                selectedActionBucket = ActionBucket.Pass;
                            }
                            break;
                        case PlayStyle.TightAggressive:
                            //play LooseAggressive
                            if (handStrengthBucket >= HandStrengthBucket.HighCardAce)
                            {
                                selectedActionBucket = ActionBucket.LowBet;
                            }
                            else
                            {
                                selectedActionBucket = ActionBucket.Pass;
                            }
                            break;
                        case PlayStyle.TightPassive:
                            //play LooseAggressive
                            if (handStrengthBucket >= HandStrengthBucket.HighCardAce)
                            {
                                selectedActionBucket = ActionBucket.LowBet;
                            }
                            else
                            {
                                selectedActionBucket = ActionBucket.Pass;
                            }
                            break;
                    }
                }
            }
            else
            {
                if (currentGame.Phase == GamePhase.PreFlop)
                {
                    switch (counterAction)
                    {
                        case FeatureAction.Bet:
                            if (startHandBucket > StartHandBucket.Worst)
                            {
                                selectedActionBucket = ActionBucket.LowBet;
                            }
                            break;
                        case FeatureAction.Pass:
                            if (startHandBucket < StartHandBucket.AverageGood)
                            {
                                selectedActionBucket = ActionBucket.Pass;
                            }
                            break;
                    }
                }
                else
                {
                    switch (counterAction)
                    {
                        case FeatureAction.Bet:                            
                            selectedActionBucket = ActionBucket.LowBet;
                            break;
                        case FeatureAction.Pass:
                            if (handStrengthBucket < HandStrengthBucket.LowPair)
                            {
                                selectedActionBucket = ActionBucket.Pass;
                            }
                            break;
                    }
                }
            }

            switch (selectedActionBucket)
            {
                case ActionBucket.None:
                    //no counter move has been found. Fall back to default strategy
                    var globalFeatures = opponent.Features.Where(x => x.IsGlobal);
                    randomBot.ChipStack = this.ChipStack;
                    return randomBot.GetAction(possibleActions, amountToCall);
                default:
                    ActionType selectedAction = ActionAbstracter.MapToAction(selectedActionBucket, amountToCall);
                    int betSize = 0;
                    switch (selectedAction)
                    {
                        case ActionType.Bet:
                        case ActionType.Raise:
                            betSize = ActionAbstracter.GetBetSize(selectedActionBucket, amountToCall, currentGame.PotSize);
                            break;
                        case ActionType.Call:
                            betSize = amountToCall;
                            break;
                    }


                    if (!possibleActions.Contains(selectedAction))
                    {
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

                    return Task.FromResult<GameActionEntity>(
                        new GameActionEntity
                        {
                            ActionType = selectedAction,
                            Amount = betSize,
                            PlayerId = this.Id
                        });
            }
        }

        public override void AssignCurrentGame(HeadsupGame game)
        {
            base.AssignCurrentGame(game);
            game.ChangedPhase += game_ChangedPhase;
            game.PlayerActed += game_PlayerActed;
            game.OnAfterShowdown += game_OnAfterShowdown;
        }

        public override void DealHoleCards(Card card1, Card card2)
        {
            base.DealHoleCards(card1, card2);
            actionHistory = new List<ActionBucket>();
            actionHistoriesPerPhase = new List<ActionHistoryPerPhase>();
            actionHistoriesPerPhase.Add(new ActionHistoryPerPhase()
            {
                Phase = GamePhase.PreFlop
            });

            startHandBucket = StartHandAbstracter.FromStartHand(card1, card2);
            handStrengthBucket = HandStrengthBucket.None;

            opponent.StartNewhand();
        }

        private void game_OnAfterShowdown(Guid winnerId)
        {
            bool? isOpponentWinner;
            if (winnerId == Guid.Empty)
            {
                isOpponentWinner = null;
            }
            else if (winnerId == this.Id)
            {
                isOpponentWinner = false;
            }
            else
            {
                isOpponentWinner = true;
            }

            opponent.UpdateFeaturesAfterShowdown(isOpponentWinner);
        }


        private void game_PlayerActed(Guid playerId, ActionType action, int amountToCall, int amount)
        {
            var actionBucket = ActionAbstracter.MapToBucket(action, amountToCall, currentGame.PotSize, amount);
            actionHistory.Add(actionBucket);

            FeatureAction featureAction = FeatureActionMapper.FromActionBucket(actionBucket);

            var currentPhasehistory = actionHistoriesPerPhase.FirstOrDefault(x => x.Phase == currentGame.Phase);
            currentPhasehistory.ActionHistory.Add(featureAction);

            if (this.Id != playerId)
            {
                Positioning opponentPositioning = Positioning.None;
                Aggression opponentAggression = Aggression.None;
                if (currentGame.Phase > GamePhase.PreFlop)
                {
                    opponentPositioning = getOpponentPositioning(currentPhasehistory.ActionHistory.Count);
                    opponentAggression = getOpponentAggression(opponentPositioning);
                }

                opponent.UpdateFeaturesAfterAction(currentPhasehistory.ActionHistory, currentGame.Phase, opponentAggression, opponentPositioning);
            }
        }


        private void game_ChangedPhase(List<Card> board, GamePhase phase)
        {
            handStrengthBucket = HandStrengthAbstracter.MapToHandBucket(board, HoleCards);
            actionHistoriesPerPhase.Add(new ActionHistoryPerPhase()
            {
                Phase = phase
            });
        }


        internal class ActionHistoryPerPhase
        {
            public GamePhase Phase { get; set; }
            public List<FeatureAction> ActionHistory { get; set; }

            internal ActionHistoryPerPhase()
            {
                ActionHistory = new List<FeatureAction>();
            }
        }

        private Positioning getOpponentPositioning(int countCurrentPhase)
        {
            if (countCurrentPhase % 2 == 0)
            {
                return Positioning.InPosition;
            }
            else
            {
                return Positioning.OutOfPosition;
            }
        }

        private Aggression getOpponentAggression(Positioning positioning)
        {
            GamePhase previousPhase = currentGame.Phase - 1;
            var previousHistory = actionHistoriesPerPhase.FirstOrDefault(x => x.Phase == previousPhase);
            var secondLastAction = previousHistory.ActionHistory[previousHistory.ActionHistory.Count - 2];
            // only betting is considered to be aggressive
            if (secondLastAction == FeatureAction.Bet)
            {
                if (previousHistory.ActionHistory.Count % 2 == 0)
                {
                    if (positioning == Positioning.InPosition)
                    {
                        return Aggression.IsNotLast;
                    }
                    else
                    {
                        return Aggression.IsLast;
                    }
                }
                else
                {
                    if (positioning == Positioning.InPosition)
                    {
                        return Aggression.IsLast;
                    }
                    else
                    {
                        return Aggression.IsNotLast;
                    }
                }
            }

            return Aggression.None;
        }
    }
}
