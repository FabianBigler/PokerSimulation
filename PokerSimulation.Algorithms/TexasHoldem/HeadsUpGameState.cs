using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem
{
    public class HeadsUpGameState
    {
        public int AmountToCall;
        public int PotSize;
        public GamePhase Phase;

        //does not change while playing a hand
        public List<Card> Board;
        public List<Card> Player1HoleCards;
        public List<Card> Player2HoleCards;

        public GamePhase NextPhase(GamePhase phase)
        {
            switch (phase)
            {
                case GamePhase.PreFlop:
                    return GamePhase.Flop;                    
                case GamePhase.Flop:
                    return GamePhase.Turn;                
                case GamePhase.Turn:
                    return GamePhase.River;
                case GamePhase.River:
                    return GamePhase.ShowDown;                                    
                default:
                    throw new NotImplementedException(string.Format("State {0} is not supported", Phase));
            }            
        }

        public HeadsUpGameState GetCopy()
        {
            var state = new HeadsUpGameState();
            state.AmountToCall = this.AmountToCall;
            state.Player1HoleCards = this.Player1HoleCards;
            state.Player2HoleCards = this.Player2HoleCards;
            state.Board = this.Board;
            state.Phase = this.Phase;
            state.PotSize = this.PotSize;
            return state;            
        }
    }
}
