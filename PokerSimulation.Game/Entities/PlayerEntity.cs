using PokerSimulation.Game.Enumerations;
using System;

namespace PokerSimulation.Game.Entities
{ 
    public class PlayerEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PlayerType Type { get; set; }
    }
}
