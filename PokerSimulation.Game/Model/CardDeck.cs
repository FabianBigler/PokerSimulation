using PokerSimulation.Game.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerSimulation.Game.Model
{
    public class CardDeck
    {        
        private Stack<Card> deck;

        public CardDeck()
        {         
            deck = new Stack<Card>(52);         
                        
            int index = 0;
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    deck.Push(new Card(suit, value));                    
                    index++;
                }
            }            
        }     

        public void Shuffle()
        {
            var r = new Random();
            var shuffled = deck.OrderBy(x => r.Next());
            deck = new Stack<Card>(shuffled);
        }
       
        public Card GetNextCard()
        {
            return deck.Pop();
        }
    }
}
