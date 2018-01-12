using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    /// <summary>
    /// Hand Strengths 
    /// </summary>
    public enum HandStrengthBucket : byte
    {
        /// <summary>
        /// Default value, not legal bucket
        /// </summary>
        None = 0,        
        /// <summary>
        /// Very low hand
        /// </summary>
        HighCardElse = 1,
        /// <summary>
        /// No card hit, but at least there is an Ace High Card
        /// </summary>
        HighCardAce = 2,
        /// <summary>
        /// Low pair hit
        /// </summary>
        LowPair = 3,
        /// <summary>
        /// Middle pair hit
        /// </summary>
        MiddlePair = 4,
        /// <summary>
        /// Top pair hit
        /// </summary>
        TopPair = 5,
        /// <summary>
        /// Top hands (anything better than one pair)
        /// </summary>
        TopHands = 6
    }
}
