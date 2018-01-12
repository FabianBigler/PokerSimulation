using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    /// <summary>
    /// ActionBucket has 5 legal actions: Pass, Call, LowBet, MediumBet, HighBet which may be extended to more actions.
    /// Whereas LowBet, MediumBet and HighBet are highly abstracted.
    /// </summary>
    public enum ActionBucket : byte
    {        
        /// <summary>
        /// Default value = None
        /// </summary>
        None = 0,
        /// <summary>
        /// Pass, Check is considered to be the same action
        /// </summary>
        Pass = 1,                
        Call = 2,
        LowBet = 3,        
        MediumBet = 4, 
        HighBet = 5  
    }
}
