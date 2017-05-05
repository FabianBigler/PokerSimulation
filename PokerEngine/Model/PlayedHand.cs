using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Model
{
    public class PlayedHand 
    {
        public Guid Id { get; set; }
        public Guid SessionID;
        public List<GameAction> Actions { get; set; }
        public Player Winner;
        public int PotSize;
        public DateTime Timestamp;

        public PlayedHand()
        {
            Actions = new List<GameAction>();                        
        }
    }
}
