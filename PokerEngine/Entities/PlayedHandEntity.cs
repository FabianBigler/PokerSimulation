using PokerEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Entities
{
    public class PlayedHandEntity
    {
        public Guid Id { get; set; }
        public Guid WinnerId { get; set; }        
        public int PotSize { get; set; }
        public DateTime Timestamp { get; set; }
        public Guid SessionId { get; set; }        

        public List<GameActionEntity> Actions { get; set; }
        public Player Winner;

        public PlayedHandEntity()
        {
            Actions = new List<GameActionEntity>();
        }
    }
}
