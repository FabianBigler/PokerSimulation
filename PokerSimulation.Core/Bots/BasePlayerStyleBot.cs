﻿using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
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
                    double handTightRatio = (double)startHandBucket / (double)StartHandBucket.Best;
                    if (TightRatio <= handTightRatio)
                    {                        
                        selectedActionBucket = ActionBucket.LowBet;
                    }
                    else
                    {
                        selectedActionBucket = ActionBucket.Pass;
                    }
                    break;
                default:
                    double handAggressiveRatio = (double)handStrengthBucket / (double)HandStrengthBucket.TopHands;
                    if (AggressiveRatio <= handAggressiveRatio)
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
