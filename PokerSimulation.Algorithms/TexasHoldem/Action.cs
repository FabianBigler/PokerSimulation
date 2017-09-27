using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem
{
    public enum Action : byte
    {
        // Pass, Call or Check is considered to be the same action
        Pass,

        // Bet has to be abstracted to buckets
        LowBet, 
        MediumBet, 
        HighBet  
    }
}
