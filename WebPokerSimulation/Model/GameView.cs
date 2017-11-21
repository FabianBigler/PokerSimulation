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
        public HistoryView History { get; set; }
        public PlayerView Human { get; set; }
        public PlayerView Bot { get; set; }
        public PendingActionView PendingAction { get; set; }
    }

    public class PlayerView
    {
        public string Name { get; set; }
        public int ChipStack { get; set; }
        public string HoleCard1 { get; set; }
        public string HoleCard2 { get; set; }          
    }

    public class HistoryView
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
    }
}