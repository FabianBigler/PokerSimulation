using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Model;
using PokerSimulation.Game.Entities;
using System;
using System.Collections.Generic;

namespace PokerSimulation.Core.Entities
{
    public class SessionEntity
    {
        public Guid Id { get; set; }
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }      
        public SessionState State { get; set; }
        public int TotalHandsCount { get; set; }
        public int PlayedHandsCount { get;  set; }     
        public DateTime Created { get; set; }   
                
        public PlayerEntity PlayerEntity1 { get; set; }        
        public PlayerEntity PlayerEntity2 { get; set; }     
        public IEnumerable<PlayedHandEntity> PlayedHandEntities { get; set; }              
    }
}
