using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Algorithms.TexasHoldem.CounterFactualRegret
{
    /// <summary>
    /// This class holds the current state of the heads up game   
    /// </summary>
    public class HeadsUpGameState
    {
        public int AmountToCall;
        public int PotSize;
        public GamePhase Phase;
        
        public List<Card> Board;
        public List<Card> Player1HoleCards;
        public List<Card> Player2HoleCards;

        /// <summary>
        /// Sets the next phase of the game
        /// </summary>
        /// <param name="phase">current phase of the game</param>
        public void SetNextPhase(GamePhase phase)
        {
            switch (phase)
            {
                case GamePhase.PreFlop:
                    this.Phase =  GamePhase.Flop;
                    break;           
                case GamePhase.Flop:
                    this.Phase = GamePhase.Turn;
                    break;
                case GamePhase.Turn:
                    this.Phase = GamePhase.River;
                    break;
                case GamePhase.River:
                    this.Phase = GamePhase.Showdown;
                    break;
                default:
                    throw new NotImplementedException(string.Format("State {0} is not supported", Phase));
            }            
        }

        /// <summary>
        /// Retrieves a new copy of the current game state
        /// </summary>
        /// <returns></returns>
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
