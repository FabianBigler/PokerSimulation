using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public enum StartHandBucket : byte
    {
        // Hands with two unpaired cards below an {8-}
        Lowest = 1,
        // Non-connected hands that are { J-}-, {10-}- or {9-}-high
        VeryLow = 2,
        // Connected or suited middle cards
        Low = 3,    
        // Weak hands with a Broadway card and {2-}{2-}
        MediumLow = 4,
        // Remaining Broadway hands, and hands like {10-}{9-}-suited
        MediumGood = 5,
        // Weak aces, weak kings, and small pairs
        Good = 6,
        // Big aces, big kings, suited Broadways, and medium pairs
        VeryGood = 7,
        // Big pairs
        Top = 8
    }
}
