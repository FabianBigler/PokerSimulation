﻿using PokerSimulation.Game.Enumerations;
using System;

namespace PokerSimulation.Game.Entities
{
    public class GameActionEntity
    {
        public Guid Id { get; set; }
        public Guid HandId { get; set; }
        public Guid PlayerId { get; set; }
        public int Amount { get; set; }
        public ActionType ActionType { get; set; }       
        public DateTime Timestamp { get; set; } 
    }
}
