using System.Collections.Generic;
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Algorithms;

namespace PokerSimulation.Core.Model.Bots
{
    public class CounterFactualRegretBot : Player
    {        
        private Dictionary<int, RegretGameNode<ActionBucket>> gameTree;
        private byte handBucket;

        public CounterFactualRegretBot(PlayerEntity entity) : base(entity)
        {                                    
            //load game tree from memory
        }

        public override void DealHoleCards(Card card1, Card card2)
        {
            base.DealHoleCards(card1, card2);
            handBucket = (byte) StartHandAbstracter.FromStartHand(card1, card2);
        }

        public override GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {

            // map actions to buckets
            // map hands to buckets
            //             
            //var infoSet = new InformationSet<ActionBucket>();
           // infoSet.CardBucket
           

            return null;
        }         
    }
}
