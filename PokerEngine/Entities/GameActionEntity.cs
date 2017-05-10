using PokerEngine.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Model
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
