using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPokerSimulation.Model
{
    public class GameView
    {
        public int PotSize { get; set; }       
        public string Phase {get;set;}
        public List<string> Board { get; set; }       
        public StatisticsView Statistics { get; set; }
        public PlayerView Human { get; set; }
        public PlayerView Bot { get; set; }
        public PendingActionView PendingAction { get; set; }

        public int HashCode { get { return this.GetHashCode();  } }

        object locking = new object();

        public override int GetHashCode()
        {
            lock(locking)
            {
                unchecked
                {
                    int hash = 17;
                    hash = hash * 23 + PotSize;
                    hash = hash * 23 + Phase.GetHashCode();                    
                    foreach (string card in Board)
                    {
                        hash *= 23; // multiply by a prime number
                        hash += card.GetHashCode(); // add next hash code                    
                    }

                    if (PendingAction != null)
                    {
                        hash = hash * 23 + PendingAction.GetHashCode();
                    }

                    return hash;
                }
            }            
        }
    }

    public class PlayerView
    {
        public string Name { get; set; }
        public int ChipStack { get; set; }
        public string HoleCard1 { get; set; }
        public string HoleCard2 { get; set; }          
    }

    public class StatisticsView
    {
        public int PlayedHandsCount { get; set; }
        public int TotalHandsCount { get; set; }
        public int AmountWon { get; set; }
    }

    public class PendingActionView
    {
        public int AmountToCall { get; set; }
        public List<ActionType> PossibleActions { get; set; }
        public int MinAmount { get; set; }
        public int MinAmountToRaise { get; set; }
        public int MaxAmount { get; set; }        

        public PendingActionView()
        {
            PossibleActions = new List<ActionType>();
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + AmountToCall;                
                hash = hash * 23 + MinAmount;
                hash = hash * 23 + MaxAmount;
                hash = hash * 23 + MinAmountToRaise;
                foreach (var action in PossibleActions)
                {
                    int actionHash = (int) action + 1;
                    hash *= 23; // multiply by a prime number
                    hash += actionHash; // add next hash code                    
                }                                
                return hash;
            }
        }
    }
}