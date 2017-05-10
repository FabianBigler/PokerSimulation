using PokerEngine.Interfaces;
using System.Collections.Generic;

namespace PokerEngine.Model
{
    public class RandomDealer : ICanDeal
    {
        private CardDeck deck;

        public RandomDealer()
        {            
            deck = new CardDeck();       
        } 
        
        public void DealHoleCards(List<Player> players)
        {
            deck = new CardDeck();
            deck.Shuffle();
            foreach(var player in players)
            {
                player.DealHoleCards(deck.GetNextCard(), deck.GetNextCard());
            }            
        }            
        
        public void DealFlop(List<Card> board)
        {
            board.Add(deck.GetNextCard());
            board.Add(deck.GetNextCard());
            board.Add(deck.GetNextCard());
        }  

        public void DealTurn(List<Card> board)
        {
            board.Add(deck.GetNextCard());
        }

        public void DealRiver(List<Card> board)
        {
            board.Add(deck.GetNextCard());
        }         
    }
}
