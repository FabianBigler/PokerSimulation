using PokerSimulation.Game.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Bots
{
    public class TightPassiveBot : BasePlayerStyleBot
    {
        private const double tightRatio = 0.9;
        private const double aggressiveRatio = 0.1;

        public TightPassiveBot(PlayerEntity entity) : base(entity)
        {
            base.TightRatio = tightRatio;
            base.AggressiveRatio = aggressiveRatio;
        }
    }
}
