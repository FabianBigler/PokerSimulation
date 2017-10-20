using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Interfaces;
using System;
using System.Collections.Generic;

namespace PokerSimulation.Game.Model
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

        protected HeadsupGame currentGame { get; set; }

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

        /// <summary>
        /// Assign Game like this to allow subscriptions in derived classes
        /// </summary>
        /// <param name="game"></param>
        public virtual void AssignCurrentGame(HeadsupGame game)
        {
            this.currentGame = game;
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
