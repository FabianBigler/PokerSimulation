using PokerSimulation.Core.Model;
using PokerSimulation.Game;
using PokerSimulation.Game.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerSimulation.Core.Interfaces
{
    public interface IHumanGameService
    {
        Session GetHumanSession();
        HeadsupGame CurrentGame { get;}
        Player CurrentHumanPlayer { get; }
        Player CurrentOpponent { get; }
        Session CurrentSession { get; }
        int GetAmountWon();

        PendingAction GetPendingAction();
        void SetAction(ActionType type, int amount);        
    }
}
