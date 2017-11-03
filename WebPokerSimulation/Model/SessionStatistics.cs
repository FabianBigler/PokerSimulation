using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebPokerSimulation.Model
{
    public class SessionStatistics
    {
        public string Winner { get; set; }
        public int TotalAmountWon { get; set; }
        public decimal TotalBigBlindsWon { get; set; }
        public int PlayedHandsCount { get; set; }
        public decimal BigBlindsPer100HandsWon
        {
            get
            {
                return this.TotalBigBlindsWon / (this.PlayedHandsCount / 100);
            }
        }
    }
}