using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerEngine.Enumerations;
using PokerEngine.Interfaces;
using PokerEngine.Entities;
using PokerEngine.Core;

namespace PokerEngine.Model
{
    public class Player : ICanPlay
    {        
        //public Guid Id { get; set; }
        //public string Name { get; set; }        
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

        public Card[] HoleCards {get;set;}
        
        public void DealHoleCards(Card card1, Card card2)
        {
            HoleCards = new Card[2];
            HoleCards[0] = card1;
            HoleCards[1] = card2;
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
