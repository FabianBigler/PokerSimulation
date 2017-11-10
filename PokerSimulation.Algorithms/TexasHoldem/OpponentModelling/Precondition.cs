using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.OpponentModelling
{
    public class Precondition
    {        
        public bool IsInPosition { get; set; }
        public List<ActionBucket> LastActions { get; set; }
    }
}
