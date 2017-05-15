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

        [HttpGet]
        public ActionResult GetById(Guid sessionId)
        {
            var session = sessionRepository.GetById(sessionId);
            return Json(session, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Detail(Guid sessionID)
        {            
            var session = sessionRepository.GetById(sessionID);
            return View(session);
        }

        [HttpPost]
        public ActionResult Pause(Guid sessionId)
        {
            sessionScheduler.PauseSession(sessionId);
            return View(sessionRepository.GetAll());
        }

        [HttpPost]
        public ActionResult Resume(Guid sessionId)
        {
            sessionScheduler.ResumeSession(sessionId);
            return View(sessionRepository.GetAll());
        }

        [HttpPost]
        public ActionResult Delete(Guid sessionId)
        {
            var session = sessionRepository.GetById(sessionId);
            sessionScheduler.PauseSession(sessionId);
            sessionRepository.Delete(session);
            return Json(new { success = true });            
        }

        public ActionResult GetAvailablePlayers()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        // public ActionResult LineBasic()
        //{
        //    List<double> tokyoValues = new List<double> { 7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6 };
        //    List<double> nyValues = new List<double> { -0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5 };
        //    List<double> berlinValues = new List<double> { -0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0 };
        //    List<double> londonValues = new List<double> { 3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8 };
        //    List<LineSeriesData> tokyoData = new List<LineSeriesData>();
        //    List<LineSeriesData> nyData = new List<LineSeriesData>();
        //    List<LineSeriesData> berlinData = new List<LineSeriesData>();
        //    List<LineSeriesData> londonData = new List<LineSeriesData>();

        //    tokyoValues.ForEach(p => tokyoData.Add(new LineSeriesData { Y = p }));
        //    nyValues.ForEach(p => nyData.Add(new LineSeriesData { Y = p }));
        //    berlinValues.ForEach(p => berlinData.Add(new LineSeriesData { Y = p }));
        //    londonValues.ForEach(p => londonData.Add(new LineSeriesData { Y = p }));


        //    ViewData["tokyoData"] = tokyoData;
        //    ViewData["nyData"] = nyData;
        //    ViewData["berlinData"] = berlinData;
        //    ViewData["londonData"] = londonData;

        //    return View();
        //}
    }
}