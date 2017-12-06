using PokerSimulation.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Bots
{
    public class TightAggressiveBot : BasePlayerStyleBot
    {
        private const double tightRatio = Tight;
        private const double aggressiveRatio = Aggressive;              

        public TightAggressiveBot(PlayerEntity entity) : base(entity)
        {
            base.TightRatio = tightRatio;
            base.AggressiveRatio = aggressiveRatio;
        }
    }
}
