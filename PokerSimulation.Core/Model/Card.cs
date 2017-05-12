using PokerSimulation.Core.Enumerations;

namespace PokerSimulation.Core.Model
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
