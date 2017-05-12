using System.Collections.Generic;

namespace PokerSimulation.Core.Model
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
