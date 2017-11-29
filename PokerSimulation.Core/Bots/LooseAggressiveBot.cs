using PokerSimulation.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Bots
{
    public class LooseAggressiveBot : BasePlayerStyleBot
    {
        private const double tightRatio = 0.1;
        private const double aggressiveRatio = 0.9;

        public LooseAggressiveBot(PlayerEntity entity) : base(entity)
        {
            base.TightRatio = tightRatio;
            base.AggressiveRatio = aggressiveRatio;
        }
    }
}
