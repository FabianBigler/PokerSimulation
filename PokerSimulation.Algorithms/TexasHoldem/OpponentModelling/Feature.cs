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
        /// <summary>
        /// the phase in which this feature is active
        /// </summary>
        public GamePhase Phase { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// the precondition which has to be met that the feature becomes active.
        /// </summary>
        public Precondition PreCondition { get; set; }                
        public double BetMinThreshold { get; set; }
        public double BetMaxThreshold { get; set; }
        public double PassMinThreshold { get; set; }
        public double PassMaxThreshold { get; set; }

        /// <summary>
        /// determines which actions (usually only one) are measured
        /// </summary>
        public List<FeatureAction> ActionsToMeasure { get; set; }

        /// <summary>
        /// global features' CountTotal matches the played hands' count 
        /// some features (e.g. went to showdown) have to be counted
        /// even if they have not occured
        /// </summary>
        public bool IsGlobal { get; set; }

        /// <summary>
        /// every feature has to be incremented only once per hand
        /// but may occur multiple times
        /// </summary>
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

        /// <summary>
        /// Used to check if the opponent's actions are in bet range.
        /// </summary>
        public bool IsInBetRange
        {
            get
            {
                return Value >= BetMinThreshold &&
                       Value <= BetMaxThreshold;
            }           
        }

        /// <summary>
        /// Used to check if the opponent's actions are in pass range.
        /// </summary
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
