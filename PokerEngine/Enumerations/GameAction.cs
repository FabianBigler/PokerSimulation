using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Enumerations
{
    public enum ActionType
    {
        Illegal = -1,
        Fold = 0,
        Check = 1,
        Call = 2,
        Raise = 3,
        Bet = 4
    }
}
