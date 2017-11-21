using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Enumerations;
using PokerSimulation.Core.Interfaces;
using PokerSimulation.Core.Repositories;
using PokerSimulation.Game;
using PokerSimulation.Game.Enumerations;
using PokerSimulation.Game.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebPokerSimulation.Model;

namespace WebPokerSimulation.Controllers
{
    public class GameController : Controller
    {
        private IHumanGameService humanGameService;

        public GameController(IHumanGameService humanGameService)
        {
            this.humanGameService = humanGameService;
        }


        public ActionResult Index()
        {
            var session = humanGameService.GetHumanSession();
            if(session == null)
            {
                ViewBag.Message = "Please create a new session with a human player!";
            }

            return View();        
        }

        [HttpGet]
        public ActionResult GetGame()
        {
            var currentGame = humanGameService.CurrentGame;
            var currentHumanPlayer = humanGameService.CurrentHumanPlayer;
            var currentOpponent = humanGameService.CurrentOpponent;
            var currentSession = humanGameService.CurrentSession;
            var pendingAction = humanGameService.GetPendingAction();                                   

            var gameView = new GameView()
            {
                PotSize = currentGame.PotSize,
                Phase = currentGame.Phase.ToString(),
                Board = currentGame.Board.ToAbbreviations().Split(' ').ToList(),
                Bot = new PlayerView()
                {
                    Name = currentOpponent.Name,
                    ChipStack = currentOpponent.ChipStack,
                    HoleCard1 = currentOpponent.HoleCards[0].ToString(),
                    HoleCard2 = currentOpponent.HoleCards[1].ToString()
                },
                Human = new PlayerView()
                {
                    ChipStack = currentHumanPlayer.ChipStack,
                    HoleCard1 = currentHumanPlayer.HoleCards[0].ToString(),
                    HoleCard2 = currentHumanPlayer.HoleCards[0].ToString(),
                    Name = "You",                                        
                },
                History = new HistoryView()
                {
                    PlayedHandsCount = currentSession.PlayedHandsCount,
                    TotalHandsCount = currentSession.TotalHandsCount
                }              
            };

            if (pendingAction != null)
            {
                int minAmount = 0;
                int minAmountToRaise = 0;
                if (pendingAction.AmountToCall == 0)
                {
                    minAmount = HeadsupGame.BigBlindSize;
                    minAmountToRaise = HeadsupGame.BigBlindSize;
                }
                else
                {
                    minAmount = pendingAction.AmountToCall;
                    minAmountToRaise = pendingAction.AmountToCall * 2;
                }                                

                if(minAmount > currentHumanPlayer.ChipStack)
                {
                    minAmount = currentHumanPlayer.ChipStack;
                }

                gameView.PendingAction = new PendingActionView()
                {
                    AmountToCall = pendingAction.AmountToCall,
                    PossibleActions = pendingAction.PossibleActions,
                    MinAmount = minAmount,
                    MaxAmount = currentHumanPlayer.ChipStack
                };
            }
         

            return Json(gameView, JsonRequestBehavior.AllowGet);
        }    


        [HttpPost]
        public ActionResult SetAction(ActionType actionType, int amount)
        {
            humanGameService.SetAction(actionType, amount);
            return View();
        }
    }
}