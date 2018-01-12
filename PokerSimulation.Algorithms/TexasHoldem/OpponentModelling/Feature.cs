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
        public double BetMinThreshold { get; set; }
        public double BetMaxThreshold { get; set; }
        public double PassMinThreshold { get; set; }
        public double PassMaxThreshold { get; set; }
        public List<FeatureAction> ActionsToMeasure { get; set; }

        // global features' CountTotal matches the played hands' count 
        // some features (e.g. went to showdown) have to be counted
        // even if they have not occured
        public bool IsGlobal { get; set; }

        // every feature has to be incremented only once per hand
        // but may occur multiple times
        public bool AlreadyIncremented { get; set; }

        // the following values are evaluated at runtime        
        public int CountTotal { get; set; }
        public int Count { get; set; }
        public double Value
        {
            get
            {
                return (double)Count / CountTotal;
            }
        }

        public bool IsInBetRange
        {
            get
            {
                return Value >= BetMinThreshold &&
                       Value <= BetMaxThreshold;
            }           
        }
        public bool IsInPassRange
        {
            get
            {
                return Value >= PassMinThreshold &&
                       Value <= PassMaxThreshold;
            }
        }

        public Feature()
        {
            ActionsToMeasure = new List<FeatureAction>();
        }

        public override string ToString()
        {
            return string.Format("{0}: Value: {1} (Count:{2})", Name, Value, Count);
        }
    }
}
