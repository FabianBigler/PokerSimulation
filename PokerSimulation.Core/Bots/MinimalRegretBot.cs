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

namespace PokerSimulation.Core.Bots
{
    public class MinimalRegretBot : Player
    {        
        private Dictionary<int, RegretGameNode<ActionBucket>> gameTree;
        private byte handBucket;

        public MinimalRegretBot(PlayerEntity entity) : base(entity)
        {                                                
            //load game tree from memory                                    
        }

        public override void DealHoleCards(Card card1, Card card2)
        {
            base.DealHoleCards(card1, card2);
            handBucket = (byte) StartHandAbstracter.FromStartHand(card1, card2);
        }

        public override void AssignCurrentGame(HeadsupGame game)
        {
            base.AssignCurrentGame(game);           
            game.ChangedPhase += Game_ChangedPhase;
        }

        private void Game_ChangedPhase(List<Card> board, GamePhase phase)
        {
            handBucket = (byte) HandStrengthAbstracter.MapToHandBucket(board, HoleCards);
        }     

        public override GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {
            
            // map actions to buckets
            //ActionBucket.Call                
                        
               
            //var infoSet = new InformationSet<ActionBucket>();
           // infoSet.CardBucket           

            return null;
        }         
    }
}
