using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public enum ActionBucket : byte
    {
        //Default value = None
        None = 0,
        // Pass, Check is considered to be the same action
        Pass = 1,

        // Bet has to be abstracted to buckets
        Call = 2,
        LowBet = 3, 
        MediumBet = 4, 
        HighBet = 5  
    }
}
