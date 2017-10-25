using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Game.Helpers
{
    public class HandComparer
    {
        public static HandComparison Compare(List<Card> holeCards1, List<Card> holeCards2, List<Card> board)
        {
            var player1Evaluator = new HandEvaluator(holeCards1, board);
            var player1Rank = player1Evaluator.GetHandRank();
            var player2Evaluator = new HandEvaluator(holeCards2, board);
            var player2Rank = player2Evaluator.GetHandRank();

            if (player1Rank > player2Rank)
            {
                return HandComparison.Player1Won;
            }
            if (player2Rank > player1Rank)
            {
                return HandComparison.Player2Won;
            }

            var player1TopCards = player1Evaluator.GetTopFiveCards(player1Rank);
            var player2TopCards = player2Evaluator.GetTopFiveCards(player2Rank);
            for (int i = 0; i < 5; i++)
            {
                if (player1TopCards[i].Value > player2TopCards[i].Value)
                {
                    return HandComparison.Player1Won;
                }
                else if (player2TopCards[i].Value > player1TopCards[i].Value)
                {
                    return HandComparison.Player2Won;
                }
            }
            return HandComparison.None;
        }     
    }

    public enum HandComparison
    {
        None = 0,
        Player1Won = 1,
        Player2Won = -1
    }
}
