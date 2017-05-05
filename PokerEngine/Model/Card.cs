using PokerEngine.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerEngine.Model
{
    public class Card
    {
        public CardSuit Suit { get; set; }
        public CardValue Value { get; set; }        

        public Card(CardSuit suit, CardValue value)
        {
            Suit = suit;
            Value = value;
        }        

        public override string ToString()
        {
            return string.Format("{0} of {1}", Value, Suit);
        }
    }
}
