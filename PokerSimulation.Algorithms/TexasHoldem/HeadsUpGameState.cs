using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem
{
    public struct HeadsUpGameState
    {
        public int AmountToCall;
        public int PotSize;
        public GamePhase Phase;

        public byte[] Board;
    }
}
