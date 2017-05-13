using PokerSimulation.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Extensions
{
    public static class CardArrayExtension
    {
        public static string ToAbbreviations(this List<Card> cards)
        {
            var sb = new StringBuilder();
            foreach(var card in cards)
            {
                sb.Append(card.ToString() + " ");
            }
            if (sb.Length > 0) sb.Length -= 1;
            return sb.ToString();
        }
    }    
}
