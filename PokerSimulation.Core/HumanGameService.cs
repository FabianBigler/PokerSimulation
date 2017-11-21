using PokerSimulation.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokerSimulation.Core.Model;
using PokerSimulation.Game;
using PokerSimulation.Core.Repositories;
using PokerSimulation.Core.Entities;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Game.Model;

namespace PokerSimulation.Core
{
    public class HumanGameService : IHumanGameService
    {
        private IRepository<SessionEntity> sessionRepository;
        private ISessionScheduler sessionScheduler;

        private HeadsupGame currentGame;
        private Session currentSession;
        private Player currentHumanPlayer;
        private Player currentOpponentPlayer;
        private PendingAction currentPendingAction;
        private GameActionEntity currentPlayerAction;
          
        public Player CurrentHumanPlayer
        {
            get
            {
                return this.currentHumanPlayer;
            }
        }

        public HeadsupGame CurrentGame
        {
            get
            {
                return this.currentGame;
            }
        }

        public Session CurrentSession
        {
            get
            {
                return this.currentSession;
            }
        }

        public Player CurrentOpponent
        {
            get
            {
                return this.currentOpponentPlayer;
            }
        }

        public HumanGameService(IRepository<SessionEntity> sessionRepository, ISessionScheduler sessionScheduler)
        {
            this.sessionRepository = sessionRepository;
            this.sessionScheduler = sessionScheduler;
        }     


        private GameActionEntity Player_ActionRequired(HeadsupGame game, List<ActionType> possibleActions, int amountToCall)
        {
            currentPendingAction = new PendingAction()
            {
                PossibleActions = possibleActions,
                AmountToCall = amountToCall
            };

            while (currentPlayerAction == null)
            {
                Task.Delay(3000);
            }

            return currentPlayerAction;
        }

        public Session GetHumanSession()
        {
            if (currentSession == null)
            {
                var humanSessionEntity = sessionRepository.GetAll().FirstOrDefault(x => (x.State == SessionState.Running || x.State == SessionState.Ready) &&
                                                                            (x.PlayerEntity1.Type == PlayerType.Human ||
                                                                             x.PlayerEntity2.Type == PlayerType.Human));
                if (humanSessionEntity != null)
                {
                    var session = sessionScheduler.GetSession(humanSessionEntity.Id);
                    currentGame = session.Game;
                    if (session != null)
                    {
                        currentSession = session;
                        if (humanSessionEntity.PlayerEntity1.Type == PlayerType.Human)
                        {
                            currentHumanPlayer = session.Player1;
                            currentOpponentPlayer = session.Player2;
                            session.Player1.ActionRequired -= Player_ActionRequired;
                            session.Player1.ActionRequired += Player_ActionRequired;
                        }
                        else
                        {
                            currentHumanPlayer = session.Player2;
                            currentOpponentPlayer = session.Player1;
                            session.Player1.ActionRequired -= Player_ActionRequired;
                            session.Player2.ActionRequired += Player_ActionRequired;
                        }
                        return session;
                    }
                }
            }
            else
            {
                return currentSession;
            }

            return null;
        }

        public PendingAction GetPendingAction()
        {
            return currentPendingAction;
        }


        public void SetAction(ActionType type, int amount)
        {
            currentPendingAction = null;
            var gameAction = new GameActionEntity()
            {
                ActionType = type,
                Amount = amount,
                PlayerId = currentHumanPlayer.Id
            };

            currentPlayerAction = gameAction;            
        }
    }

    public class PendingAction
    {
        public List<ActionType> PossibleActions { get; set; }
        public int AmountToCall { get; set; }
    }
}
