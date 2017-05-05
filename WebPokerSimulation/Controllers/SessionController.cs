using PokerEngine.Entities;
using PokerEngine.Model;
using PokerEngine.Repositories;
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

        public SessionController(IRepository<SessionEntity> sessionRepository, IRepository<PlayerEntity> playerRepository)
        {
            this.sessionRepository = sessionRepository;
            this.playerRepository = playerRepository;
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
                                               
            return RedirectToAction("Index");
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