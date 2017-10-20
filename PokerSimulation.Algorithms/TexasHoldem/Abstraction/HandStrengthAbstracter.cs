﻿using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Helpers;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public class HandStrengthAbstracter
    {
        public static HandStrengthBucket MapToHandBucket(List<Card> board, List<Card> holeCards)
        {
            var evaluator = new HandEvaluator(holeCards, board);
            var rank = evaluator.GetHandRank();
            if (rank >= HandRank.TwoPairs) return HandStrengthBucket.TopHands;

            switch (rank)
            {
                case HandRank.HighCard:
                case HandRank.Pair:
                    var topCards = evaluator.GetTopFiveCards(rank);
                    if (rank == HandRank.Pair)
                    {
                        var pairValue = topCards.First().Value;
                        var boardOrderedByValue = board.OrderBy(x => x.Value);
                        var topValue = boardOrderedByValue.First().Value;
                        var lowestValue = boardOrderedByValue.Last().Value;

                        if (pairValue >= topValue)
                        {
                            // either a overpair (e.g. Pocket Aces) or top pair
                            return HandStrengthBucket.TopPair;
                        }
                        else if (pairValue < topValue && pairValue > lowestValue)
                        {
                            return HandStrengthBucket.MiddlePair;
                        }
                        else
                        {
                            return HandStrengthBucket.LowPair;
                        }
                    }
                    else
                    {
                        //highcard
                        bool hasAce = topCards.Any(x => x.Value == CardValue.Ace);
                        if (hasAce)
                        {
                            return HandStrengthBucket.HighCardAce;
                        }
                        else
                        {
                            return HandStrengthBucket.HighCardElse;
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException("Handrank is not supported");
            }
        }
    }
}