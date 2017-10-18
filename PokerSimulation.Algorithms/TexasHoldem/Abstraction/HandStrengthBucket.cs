using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public enum HandStrengthBucket : byte
    {
        // Very low hand
        HighCardElse = 1,
        // No card hit, but at least there is an Ace High Card
        HighCardAce = 2,
        // Low pair hit
        LowPair = 3,
        // Middle pair hit
        MiddlePair = 4,
        // Top pair hit
        TopPair = 5,
        // Top hands (anything better than one pair)
        TopHands = 6
    }
}
