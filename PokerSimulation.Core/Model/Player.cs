using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Interfaces;
using System;
using System.Collections.Generic;

namespace PokerSimulation.Core.Model
{
    public class Player : ICanPlay
    {        
        public PlayerEntity Entity { get; set; }
        public int ChipStack { get; set; }    
        public bool IsSmallBlind { get; set; }    
        public bool IsBigBlind { get; set; }
        public int AmountAlreadyInPotThisRound { get; set; }

        public Guid Id { get
            {
                return Entity.Id;
            }
        }

        public string Name
        {
            get
            {
                return Entity.Name;
            }
        }

        public Player(PlayerEntity entity)
        {
            this.Entity = entity;
        }

        public List<Card> HoleCards {get;set;}
        
        public virtual void DealHoleCards(Card card1, Card card2)
        {
            HoleCards = new List<Card>(2);
            HoleCards.Add(card1);
            HoleCards.Add(card2);
        }  

        public int GetBlind()
        {            
            if (IsSmallBlind)
            {
                this.ChipStack -= HeadsupGame.SmallBlindSize;
                this.AmountAlreadyInPotThisRound = HeadsupGame.SmallBlindSize;
                return HeadsupGame.SmallBlindSize;                
            }
                                         
            if(IsBigBlind)
            {                                       
                this.ChipStack -= HeadsupGame.BigBlindSize;
                this.AmountAlreadyInPotThisRound = HeadsupGame.BigBlindSize;
                return HeadsupGame.BigBlindSize;
            }

            return 0;
        }

        public virtual GameActionEntity GetAction(List<ActionType> possibleActions, HeadsupGame context, int amountToCall)
        {
            throw new NotImplementedException("GetAction not implemented!");
        }
    }
}
