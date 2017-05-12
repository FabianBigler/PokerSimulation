using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Exceptions;
using PokerSimulation.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSimulation.Core.Helpers
{
    public class HandEvaluator
    {
        public const int MinCardsCount = 5;

        private List<Card> cardsOrderedBySuit;
        private List<Card> cardsOrderedByValue;

        public HandEvaluator(List<Card> holeCards, List<Card> board)
        {
            cardsOrderedBySuit = holeCards.Union(board).OrderByDescending(x => x.Suit).ThenByDescending(x => x.Value).ToList();
            cardsOrderedByValue = cardsOrderedBySuit.OrderByDescending(x => x.Value).ThenByDescending(x => x.Suit).ToList();
        }              

        public HandRank GetHandRank()
        {           
            if (cardsOrderedBySuit.Count < MinCardsCount) throw new InvalidCardCountException(String.Format("Invalid Count of Cards passed. Expected: {0}, Actual: {1}", MinCardsCount, cardsOrderedBySuit.Count));            
            if (isRoyalFlush()) return HandRank.RoyalFlush;
            if (isStraightFlush()) return HandRank.StraightFlush;
            if (isFourOfAKind()) return HandRank.FourOfAKind;
            if (isFullHouse()) return HandRank.FullHouse;
            if (isFlush()) return HandRank.Flush;
            if (isStraight()) return HandRank.Straight;
            if (isThreeOfAKind()) return HandRank.ThreeOfAKind;
            if (isTwoPairs()) return HandRank.TwoPairs;
            if (isPair()) return HandRank.Pair;            
            return HandRank.HighCard;            
        }    
        
        /// <summary>
        /// This method is used if both players have the same handrank. 
        /// In Texas Hold'em only the best five cards are measured to determine the winner.
        /// </summary>
        /// <returns></returns>
        public List<Card> GetBestFiveCards(HandRank rank)
        {
            switch(rank)
            {
                case HandRank.RoyalFlush:
                    return getRoyalFlushCards();                    
                case HandRank.StraightFlush:
                    return getStraightFlushCards();
                case HandRank.FourOfAKind:
                    return getBestFiveCardsOfSameValue(4);
                case HandRank.FullHouse:
                    return getFullHouseCards();
                case HandRank.Flush:
                    return getFlushCards();
                case HandRank.Straight:
                    return getStraightCards();
                case HandRank.ThreeOfAKind:
                    return getBestFiveCardsOfSameValue(3);
                case HandRank.TwoPairs:
                    return getTwoPairCards();
                case HandRank.Pair:
                    return getBestFiveCardsOfSameValue(2);
                default:
                    return cardsOrderedByValue.Take(5).ToList();
            }         
        }
 

        /// <summary>
        /// Deteremines if there are 5 cards with the same suit
        /// </summary>        
        /// <returns></returns>
        private bool isFlush()
        {
            var minCardsForFlush = 5;
            //checks if there is any group of suits consisting of 5 or more cards
            var query = cardsOrderedBySuit.GroupBy(x => x.Suit,
                (key, values) => new { Suit = key, Count = values.Count() })
                .Where(x => x.Count >= minCardsForFlush);
            return (query.Count() > 0);
        }

        private List<Card> getFlushCards()
        {
            var minCardsForFlush = 5;
            //checks if there is any group of suits consisting of 5 or more cards
            var query = cardsOrderedBySuit.GroupBy(x => x.Suit,
                (key, values) => new { Suit = key, Count = values.Count() })
                .First(x => x.Count >= minCardsForFlush);            
            return cardsOrderedBySuit.Where(x => x.Suit == query.Suit).Take(5).ToList();                        
        }


        /// <summary>
        /// Determines if there is any sequence of 5 consecutive cards
        /// The special case 'A 2 3 4 5' is also treated.
        /// </summary>        
        /// <returns></returns>
        private bool isStraight()
        {
            var minCardsForStraight = 5;
            int currentSequence = 1;
            for (int i = 0; i < cardsOrderedByValue.Count - 1; i++)
            {               
                if (cardsOrderedByValue[i + 1].Value == (cardsOrderedByValue[i].Value - 1) ||
                    //treatment of special case: A 2 3 4 5 is also a legitimate straight!
                    cardsOrderedByValue[i].Value == CardValue.Two && cardsOrderedByValue[0].Value == CardValue.Ace)
                {
                    currentSequence++;
                    if (currentSequence == minCardsForStraight) return true;
                } else
                {
                    currentSequence = 1;
                }
            }
            return false;
        }

        private List<Card> getStraightCards()
        {
            var minCardsForStraight = 5;
            int currentSequence = 1;
            var cardsToReturn = new List<Card>();

            for (int i = 0; i < cardsOrderedByValue.Count - 1; i++)
            {
                if (cardsOrderedByValue[i + 1].Value == (cardsOrderedByValue[i].Value - 1) ||                    
                    cardsOrderedByValue[i].Value == CardValue.Two && cardsOrderedByValue[0].Value == CardValue.Ace)
                {
                    cardsToReturn.Add(cardsOrderedByValue[i]);
                    currentSequence++;
                    if (currentSequence == minCardsForStraight)
                    {
                        cardsToReturn.Add(cardsOrderedByValue[i + 1]);
                        return cardsToReturn;
                    }
                }
                else
                {
                    cardsToReturn = new List<Card>();
                    cardsToReturn.Add(cardsOrderedByValue[i]);
                    currentSequence = 1;
                }
            }
            return null;
        }

        private bool isStraightFlush()
        {
            var minCardsForStraightFlush = 5;
            int currentSequence = 1;
            for (int i = 0; i < cardsOrderedBySuit.Count - 1; i++)
            {
                if (cardsOrderedBySuit[i + 1].Suit == cardsOrderedBySuit[i].Suit && 
                    (cardsOrderedBySuit[i + 1].Value == (cardsOrderedBySuit[i].Value - 1) ||
                    //treatment of special case: A 2 3 4 5 is also a legitimate straight!
                    cardsOrderedBySuit[i].Value == CardValue.Two && cardsOrderedBySuit[0].Value == CardValue.Ace))
                {
                    currentSequence++;
                    if (currentSequence == minCardsForStraightFlush) return true;
                }
                else
                {
                    currentSequence = 1;
                }
            }
            return false;
        }

        private List<Card> getStraightFlushCards()
        {
            var minCardsForStraightFlush = 5;
            int currentSequence = 1;
            var cards = new List<Card>();

            for (int i = 0; i < cardsOrderedBySuit.Count - 1; i++)
            {
                if (cardsOrderedBySuit[i + 1].Suit == cardsOrderedBySuit[i].Suit &&
                    (cardsOrderedBySuit[i + 1].Value == (cardsOrderedBySuit[i].Value - 1) ||                    
                    cardsOrderedBySuit[i].Value == CardValue.Two && cardsOrderedBySuit[0].Value == CardValue.Ace))
                {
                    currentSequence++;
                    cards.Add(cardsOrderedBySuit[i]); 
                    if (currentSequence == minCardsForStraightFlush) return cards;
                }
                else
                {
                    currentSequence = 1;
                    cards = new List<Card>();
                    cards.Add(cardsOrderedBySuit[i]);
                }
            }

            return null;
        }

        private bool isRoyalFlush()
        {        
            var minCardsForRoyalFlush = 5;           
            var flushGroup = cardsOrderedBySuit.GroupBy(n => n.Suit,
                (key, values) => new { Suit = key, Count = values.Count() })
                .FirstOrDefault(x => x.Count >= minCardsForRoyalFlush);
            if (flushGroup == null) return false;

            return cardsOrderedBySuit.Where(x => x.Suit == flushGroup.Suit && x.Value <= CardValue.Ace && x.Value >= CardValue.Ten).Count() == minCardsForRoyalFlush;                               
        }        

        private List<Card> getRoyalFlushCards()
        {
            var minCardsForRoyalFlush = 5;
            var flushGroup = cardsOrderedBySuit.GroupBy(n => n.Suit,
                (key, values) => new { Suit = key, Count = values.Count() })
                .FirstOrDefault(x => x.Count >= minCardsForRoyalFlush);            
            return cardsOrderedBySuit.Where(x => x.Suit == flushGroup.Suit && x.Value <= CardValue.Ace && x.Value >= CardValue.Ten).ToList();
        }

        private bool isFourOfAKind()
        {
            //checks if there is any group of suits consisting of 5 or more cards      
            return checkForSameValue(4);
        }          

        private List<Card> getBestFiveCardsOfSameValue(int count)
        {
            var valueGroup = cardsOrderedByValue.GroupBy(x => x.Value,
               (key, values) => new { Value = key, Count = values.Count() })
               .First(x => x.Count == count);

            var tempCards = new List<Card>(cardsOrderedByValue);
            var cardsToReturn = new List<Card>();
            foreach (var card in cardsOrderedByValue)
            {
                if(card.Value == valueGroup.Value)
                {
                    tempCards.Remove(card);
                    cardsToReturn.Add(card);
                }
            }

            cardsToReturn.AddRange(tempCards.Take(5 - count));
            return cardsToReturn;
        }

        private bool checkForSameValue(int count)
        {
            var valueGroup = cardsOrderedByValue.GroupBy(x => x.Value,
             (key, values) => new { Value = key, Count = values.Count() })
             .Where(x => x.Count == count);                        
            return (valueGroup.Count() > 0);
        }

        private bool isThreeOfAKind()
        {
            return checkForSameValue(3);
        }

        private bool isPair()
        {
            return checkForSameValue(2);
        }

        private bool isTwoPairs()
        {
            var query = cardsOrderedByValue.GroupBy(x => x.Value,
             (key, values) => new { Value = key, Count = values.Count() })
             .Where(x => x.Count == 2);            
            return (query.Count() == 2);
        }

        private bool isFullHouse()
        {
            var query = cardsOrderedByValue.GroupBy(x => x.Value,
            (key, values) => new { Value = key, Count = values.Count() });           

            var hasPair = false;
            var hasThreeOfAKind = false;
            foreach(var group in query.ToList())
            {
                if(group.Count == 2)
                {
                    hasPair = true;
                }
                if (group.Count == 3)
                {
                    hasThreeOfAKind = true;
                }
            }

            return (hasPair && hasThreeOfAKind);
        }

        private List<Card> getFullHouseCards()
        {        
            var query = cardsOrderedByValue.GroupBy(x => x.Value,
           (key, values) => new { Value = key, Count = values.Count() });
            var cardsToReturn = new List<Card>();
            var threeOfAKindValue = query.First(x => x.Count == 3).Value;
            var pairValue = query.First(x => x.Count == 2).Value;

            cardsToReturn.AddRange(cardsOrderedByValue.Where(x => x.Value == threeOfAKindValue));
            cardsToReturn.AddRange(cardsOrderedByValue.Where(x => x.Value == pairValue));
            return cardsToReturn;
        }

        private List<Card> getTwoPairCards()
        {
            var query = cardsOrderedByValue.GroupBy(x => x.Value,
             (key, values) => new { Value = key, Count = values.Count() })
             .Where(x => x.Count == 2);

            var tempCards = new List<Card>(cardsOrderedByValue);
            var cardsToReturn = new List<Card>();
            foreach(var group in query)
            {
                var cardsToAdd = cardsOrderedByValue.Where(x => x.Value == group.Value);
                foreach (var card in cardsToAdd)
                {
                    tempCards.Remove(card);
                }
                cardsToReturn.AddRange(cardsToAdd);                
            }

            cardsToReturn.Add(tempCards.First());
            return cardsToReturn;
        }
    }
}
