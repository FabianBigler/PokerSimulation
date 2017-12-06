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

namespace PokerSimulation.Core.Bots
{
    public class BasePlayerStyleBot : Player
    {
        public const double Tight = 0.8;
        public const double Loose = 0.2;
        public const double Aggressive = 0.8;
        public const double Passive = 0.2;

        private HandStrengthBucket handStrengthBucket;
        private StartHandBucket startHandBucket;

        public BasePlayerStyleBot(PlayerEntity entity) : base(entity)
        {
        }

        /// <summary>
        /// 1 is very tight and 0 is very loose
        /// </summary>
        public double TightRatio { set; get; }
        /// <summary>
        /// 1 is very aggressive and 0 is very passive
        /// </summary>
        public double AggressiveRatio { set; get; }     

        public override Task<GameActionEntity> GetAction(List<ActionType> possibleActions, int amountToCall)
        {
            ActionBucket selectedActionBucket;
            switch (currentGame.Phase)
            {
                case GamePhase.PreFlop:
                    double startHandStrengthRatio = (double)startHandBucket / (double)StartHandBucket.Best;
                    if (TightRatio <= startHandStrengthRatio)
                    {
                        startHandStrengthRatio = 1 - startHandStrengthRatio;
                        if (AggressiveRatio >= startHandStrengthRatio)
                        {
                            selectedActionBucket = ActionBucket.LowBet;
                        } else
                        {                            
                            selectedActionBucket = ActionBucket.Call;
                        }                        
                    }
                    else
                    {
                        selectedActionBucket = ActionBucket.Pass;
                    }                    
                    break;
                default:
                    double handStrengthRatio = 1 -  (double)handStrengthBucket / (double)HandStrengthBucket.TopHands;
                    if (AggressiveRatio >= handStrengthRatio)
                    {
                        selectedActionBucket = ActionBucket.LowBet;
                    }
                    else
                    {
                        selectedActionBucket = ActionBucket.Pass;
                    }

                    break;
            }

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
                   case ActionType.Call:
                        if (possibleActions.Contains(ActionType.Check))
                        {
                            selectedAction = ActionType.Check;
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

        public override void AssignCurrentGame(HeadsupGame game)
        {
            base.AssignCurrentGame(game);
            game.ChangedPhase += game_ChangedPhase;
        }

        public override void DealHoleCards(Card card1, Card card2)
        {
            base.DealHoleCards(card1, card2);
            startHandBucket = StartHandAbstracter.FromStartHand(card1, card2);
            handStrengthBucket = HandStrengthBucket.None;
        }

        private void game_ChangedPhase(List<Card> board, GamePhase phase)
        {
            handStrengthBucket = HandStrengthAbstracter.MapToHandBucket(board, HoleCards);
        }
    }
}
