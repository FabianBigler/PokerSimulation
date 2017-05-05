using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Model
{
    public class Hand
    {
        public List<Card> HoleCards { private set; get; }
        public HandRank Rank { get; set; }     
                
        public Hand(List<Card> holeCards)
        {
            HoleCards = holeCards;
        }
    }
}
