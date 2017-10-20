using PokerSimulation.Core.Interfaces;
using PokerSimulation.Core.Model;
using PokerSimulation.Game.Interfaces;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Test.Core
{
    /// <summary>
    /// This class is solely used for unit tests (injecting cards)
    /// </summary>
    public class DeterministicDealer : ICanDeal
    {        
        public List<Card> HoleCardsPlayer1 { get; set; }
        public List<Card> HoleCardsPlayer2 { get; set; }
        public List<Card> BoardCards { get; set; }

        public void DealFlop(List<Card> board)
        {            
            board.Add(BoardCards[0]);
            board.Add(BoardCards[1]);
            board.Add(BoardCards[2]);
        }

        public void DealHoleCards(List<Player> players)
        {            
            players[0].DealHoleCards(HoleCardsPlayer1[0], HoleCardsPlayer1[1]);
            players[1].DealHoleCards(HoleCardsPlayer2[0], HoleCardsPlayer2[1]);            
        }

        public void DealTurn(List<Card> board)
        {
            board.Add(BoardCards[3]);
        }

        public void DealRiver(List<Card> board)
        {
            board.Add(BoardCards[4]);
        }
    }
}
