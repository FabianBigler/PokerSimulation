using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public enum StartHandBucket : byte
    {
        /// <summary>
        /// Hands with two unpaired cards below an {8-}
        /// </summary>
        Worst = 1,
        /// <summary>
        /// Non-connected hands that are { J-}-, {10-}- or {9-}-high
        /// </summary>
        VeryBad = 2,
        /// <summary>
        /// Connected or suited middle cards
        /// </summary>
        Bad = 3,
        /// <summary>
        /// Weak hands with a Broadway card and {2-}{2-}
        /// </summary>
        AverageBad = 4,
        /// <summary>
        /// Remaining Broadway hands, and hands like {10-}{9-}-suited
        /// </summary>
        AverageGood = 5,
        /// <summary>
        /// Weak aces, weak kings, and small pairs
        /// </summary>
        Good = 6,
        /// <summary>
        /// Big aces, big kings, suited Broadways, and medium pairs
        /// </summary>
        VeryGood = 7,
        /// <summary>
        /// Big pairs
        /// </summary>
        Best = 8
    }
}
