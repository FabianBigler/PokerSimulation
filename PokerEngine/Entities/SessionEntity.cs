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
        
        //these properties are loaded lazily
        public PlayerEntity Player1 { get; set; }        
        public PlayerEntity Player2 { get; set; }     
        public IEnumerable<PlayedHand> PlayedHands { get; set; }              
    }
}
