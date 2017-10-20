using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.Abstraction
{
    public class GameAction
    {
        public ActionType Action { get; set; }
        public int Amount { get; set; }
    }
}
