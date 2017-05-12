using PokerSimulation.Core.Enumerations;
using System;

namespace PokerSimulation.Core.Entities
{ 
    public class PlayerEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public PlayerType Type { get; set; }
    }
}
