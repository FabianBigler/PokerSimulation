using PokerSimulation.Game.Model;
using System.Collections.Generic;

namespace PokerSimulation.Game.Interfaces
{
    public interface ICanDeal
    {
        void DealHoleCards(List<Player> players);
        void DealFlop(List<Card> board);
        void DealTurn(List<Card> board);
        void DealRiver(List<Card> board);
    }
}
