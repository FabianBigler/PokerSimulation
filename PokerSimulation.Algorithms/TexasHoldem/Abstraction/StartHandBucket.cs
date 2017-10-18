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
        Worst = 1,
        // Non-connected hands that are { J-}-, {10-}- or {9-}-high
        VeryBad = 2,
        // Connected or suited middle cards
        Bad = 3,    
        // Weak hands with a Broadway card and {2-}{2-}
        AverageBad = 4,
        // Remaining Broadway hands, and hands like {10-}{9-}-suited
        AverageGood = 5,
        // Weak aces, weak kings, and small pairs
        Good = 6,
        // Big aces, big kings, suited Broadways, and medium pairs
        VeryGood = 7,
        // Big pairs
        Best = 8
    }
}
