using PokerSimulation.Core.Enumerations;
using System;

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
            return GetAbbreviation();
        }

        private string GetAbbreviation()
        {
            return string.Format("{0}{1}", GetAbbreviationValue(), GetAbbreviationSuit());
        }

        public static Card GetCard(string abbreviation)
        {
            if (abbreviation.Length != 2) throw new Exception("Abbrevation's length must be 2.");
            var shortValue = abbreviation[0];
            var shortSuit = abbreviation[1];
            var value = GetValue(shortValue);
            var suit = GetSuit(shortSuit);

            return new Card(suit, value);
        }

        private static CardSuit GetSuit(char shortSuit)
        {
            switch (shortSuit)
            {
                case 'c':
                    return CardSuit.Clubs;
                case 'd':
                    return CardSuit.Diamonds;
                case 'h':
                    return CardSuit.Hearts;
                case 's':
                    return CardSuit.Spades;
            }
            throw new NotImplementedException("CardSuit not implemented");
        }

        private static CardValue GetValue(char shortValue)
        {
            switch (shortValue)
            {
                case '2':
                    return CardValue.Two;                    
                case '3':
                    return CardValue.Three;
                case '4':
                    return CardValue.Four;
                case '5':
                    return CardValue.Five;
                case '6':
                    return CardValue.Six;
                case '7':
                    return CardValue.Seven;
                case '8':
                    return CardValue.Eight;                    
                case '9':
                    return CardValue.Nine;
                case 'T':
                    return CardValue.Ten;                    
                case 'J':
                    return CardValue.Jack;                    
                case 'Q':
                    return CardValue.Queen;
                case 'K':
                    return CardValue.King;
                case 'A':
                    return CardValue.Ace;
            }
            throw new NotImplementedException("ShortValue not implemented");
        }

        private char GetAbbreviationValue()
        {            
            switch (this.Value)
            {
                case CardValue.Two:
                    return '2';
                case CardValue.Three:
                    return  '3';
                case CardValue.Four:
                    return  '4';
                case CardValue.Five:
                    return '5';
                case CardValue.Six:
                    return '6';
                case CardValue.Seven:
                    return '7';
                case CardValue.Eight:
                    return '8';
                case CardValue.Nine:
                    return '9';
                case CardValue.Ten:
                    return 'T';
                case CardValue.Jack:
                    return 'J';
                case CardValue.Queen:
                    return 'Q';
                case CardValue.King:
                    return 'K';
                case CardValue.Ace:
                    return 'A';                    
            }

            throw new NotImplementedException("CardValue not implemented");
        }

        private char GetAbbreviationSuit()
        {            
            switch(this.Suit)
            {
                case CardSuit.Clubs:
                    return 'c';
                case CardSuit.Diamonds:
                    return 'd';
                case CardSuit.Hearts:
                    return 'h';
                case CardSuit.Spades:
                    return 's';
            }
            throw new NotImplementedException("CardSuit not implemented");
        }   
    }
}
