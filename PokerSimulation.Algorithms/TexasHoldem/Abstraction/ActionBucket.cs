using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public enum ActionBucket : byte
    {
        // Pass, Check is considered to be the same action
        Pass,

        // Bet has to be abstracted to buckets
        Call,
        LowBet, 
        MediumBet, 
        HighBet  
    }
}
