using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.OpponentModelling
{
    public class FeatureNames
    {
        public const string Vpip = "VPIP";
        public const string Pfr = "PFR";
        public const string ThreeBet = "3 Bet";
        public const string ThreeBetCall = "3 Bet Call";
        public const string ThreeBetFold = "3 Bet Fold";
        public const string FourBet = "4 Bet";
        public const string FourBetCall = "4 Bet Call";
        public const string FourBetFold = "4 Bet Fold";
        public const string ContinuationBetIp = "Continuation Bet IP";
        public const string ContinuationBetOop = "Continuation Bet OOP";
        public const string ContinuationBetReraise = "Continuation Bet Reraise";
        public const string ContinuationBetRaise = "Continuation Bet Raise";
        public const string ContinuationBetCall = "Continuation Bet Raise";
        public const string ContinuationBetFold = "Continuation Bet Fold";
        public const string DonkBetIp = "Donk Bet IP";
        public const string DonkBetOop = "Donk Bet OOP";
        public const string DonkBetReraise = "Donk Bet Reraise";
        public const string DonkBetCall = "Donk Bet Call";
        public const string DonkBetFold = "Donk Bet Fold";
        public const string WentToShowdown = "Went to Showdown";
        public const string WonShowdown = "Won Showdown";        
    }
}
