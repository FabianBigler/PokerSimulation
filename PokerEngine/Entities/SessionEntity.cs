using PokerEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Entities
{
    public class SessionEntity
    {
        public Guid Id { get; set; }
        public Guid Player1Id { get; set; }
        public Guid Player2Id { get; set; }      
        public SessionState State { get; set; }
        public int TotalHandsCount { get; set; }
        public int PlayedHandsCount { get;  set; }        
                
        public PlayerEntity PlayerEntity1 { get; set; }        
        public PlayerEntity PlayerEntity2 { get; set; }     
        public IEnumerable<PlayedHandEntity> PlayedHandEntities { get; set; }              
    }
}
