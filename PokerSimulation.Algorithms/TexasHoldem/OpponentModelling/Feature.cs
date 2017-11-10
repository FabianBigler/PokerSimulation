using PokerSimulation.Algorithms.TexasHoldem.Abstraction;
using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.OpponentModelling
{
    public class Feature
    {
        public GamePhase Phase { get; set; }        
        public string Name { get; set; }
        public Precondition PreCondition { get; set; }
        public double ExpertValue { get; set; }

        // the value is evaluated at runtime
        public double Value { get; set; }                
    }
}
