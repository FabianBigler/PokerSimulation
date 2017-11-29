using PokerSimulation.Game.Enumerations;
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
                return this.TotalBigBlindsWon / ((decimal)this.PlayedHandsCount / 100);
            }
        }

        public List<SessionStatisticsDetail> StatisticsDetails { get; set; }

        public SessionStatistics()
        {
            StatisticsDetails = new List<SessionStatisticsDetail>();
        }
    }

    public class SessionStatisticsDetail
    {
        public string Winner { get; set; }
        public GamePhase Phase { get; set; }
        public string PhaseName { get { return Phase.ToString();  } }
        public decimal AmountWon { get; set; }
        public decimal BigBlindsWon { get; set; }        
        public int PlayedHandsCount { get; set; }
        public int TotalHandsCount { get; set; }

        public decimal BigBlindsPer100HandsWon
        {
            get
            {
                return this.BigBlindsWon / ((decimal)this.TotalHandsCount / 100);
            }
        }
    }
}