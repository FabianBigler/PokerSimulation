using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.KuhnPoker
{
    public enum GameAction : byte
    {
        // Pass is either call, check or fold
        Pass,
        // Bet is always $1
        Bet
    }
}
