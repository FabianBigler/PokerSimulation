using PokerEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Interfaces
{
    public interface ICanDeal
    {
        void DealHoleCards(List<Player> players);
        void DealFlop(List<Card> board);
        void DealTurn(List<Card> board);
        void DealRiver(List<Card> board);
    }
}
