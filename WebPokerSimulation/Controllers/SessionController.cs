using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Interfaces;
using PokerSimulation.Core.Repositories;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebPokerSimulation.Controllers
{
    public class SessionController : Controller
    {
        private IRepository<SessionEntity> sessionRepository;
        private IRepository<PlayerEntity> playerRepository;
        private ISessionScheduler sessionScheduler;


        public SessionController(IRepository<SessionEntity> sessionRepository, 
                                 IRepository<PlayerEntity> playerRepository,
                                 ISessionScheduler sessionScheduler)
        {
            this.sessionRepository = sessionRepository;
            this.playerRepository = playerRepository;
            this.sessionScheduler = sessionScheduler;
        }

        public ActionResult Index()
        {            
            return View(sessionRepository.GetAll());
        }

        public ActionResult New()
        {
            return View(playerRepository.GetAll());
        }

        [HttpPost]
        public ActionResult New(int totalHandsToPlay, Guid player1Id, Guid player2Id)
        {
            if(player1Id == player2Id)
            {
                ViewBag.ErrorMessage = "Please select different players!";
                return View(playerRepository.GetAll());
            }         
            var session = new SessionEntity { Player1Id = player1Id, Player2Id = player2Id, TotalHandsCount = totalHandsToPlay };
            sessionRepository.Insert(session);
            
            sessionScheduler.StartNewSession(session);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Pause(Guid SessionId)
        {
            sessionScheduler.PauseSession(SessionId);
            return View();
        }

        public ActionResult GetAvailablePlayers()
        {
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}