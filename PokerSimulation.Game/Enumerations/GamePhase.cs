using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Game.Enumerations
{
    public enum GamePhase : byte
    {
        PreFlop,
        Flop,
        Turn,
        River,
        Showdown                    
    }
}
