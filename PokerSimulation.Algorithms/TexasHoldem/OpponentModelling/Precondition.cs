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
        public Positioning Positioning { get; set; }
        public Aggression Aggression { get; set; }
        public List<FeatureAction> LastActions { get; set; }

        public Precondition()
        {
            LastActions = new List<FeatureAction>();
        }
    }
}
