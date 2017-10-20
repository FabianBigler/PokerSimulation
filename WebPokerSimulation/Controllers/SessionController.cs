using Highsoft.Web.Mvc.Charts;
using PokerSimulation.Core.Entities;
using PokerSimulation.Core.Interfaces;
using PokerSimulation.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using WebPokerSimulation.Game;
using PokerSimulation.Game.Entities;

namespace WebPokerSimulation.Controllers
{
    public class SessionController : Controller
    {
        private IRepository<SessionEntity> sessionRepository;
        private IRepository<PlayerEntity> playerRepository;
        private ISessionScheduler sessionScheduler;
        private IPlayedHandRepository playedHandRepository;

        public SessionController(IRepository<SessionEntity> sessionRepository, 
                                 IRepository<PlayerEntity> playerRepository,
                                 ISessionScheduler sessionScheduler,
                                 IPlayedHandRepository playedHandRepository)
        {
            this.sessionRepository = sessionRepository;
            this.playerRepository = playerRepository;
            this.sessionScheduler = sessionScheduler;
            this.playedHandRepository = playedHandRepository;
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

        public ActionResult LineChart(Guid sessionId)
        {
            var session = sessionRepository.GetById(sessionId);
            if (session == null)
            {
                ViewBag.NotFoundMessage = "This session is not available anymore.";
                return PartialView();
            }

            var playedHands = playedHandRepository.GetAllBySessionId(sessionId);            
            var countPoints = 20;
            var categories = new List<string>();

            var handsPerPoint = playedHands.Count() / countPoints;
            List<LineSeriesData> player1Data = new List<LineSeriesData>();
            List<LineSeriesData> player2Data = new List<LineSeriesData>();
            int prevPlayer1Won = 0;
            int prevPlayer2Won = 0;

            for (int i = 0; i < countPoints; i++)
            {
                var handsToSkip = i * handsPerPoint;
                var hands = playedHands.Skip(handsToSkip).Take(handsPerPoint);
                categories.Add(handsToSkip.ToString());                
                var player1Sum = hands.Where(x => x.WinnerId == session.Player1Id).Sum(x => x.AmountWon);
                var player2Sum = hands.Where(x => x.WinnerId == session.Player2Id).Sum(x => x.AmountWon);
                var player1Won = player1Sum + prevPlayer1Won - player2Sum;
                var player2Won = player2Sum + prevPlayer2Won - player1Sum;
                player1Data.Add(new LineSeriesData { Y = player1Won });                
                player2Data.Add(new LineSeriesData { Y = player2Won });
                prevPlayer1Won = player1Won;
                prevPlayer2Won = player2Won;
            }

            //List<double> tokyoValues = new List<double> { 7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6 };
            //List<double> nyValues = new List<double> { -0.2, 0.8, 5.7, 11.3, 17.0, 22.0, 24.8, 24.1, 20.1, 14.1, 8.6, 2.5 };
            //List<double> berlinValues = new List<double> { -0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0 };
            //List<double> londonValues = new List<double> { 3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8 };
            //List<LineSeriesData> tokyoData = new List<LineSeriesData>();
            //List<LineSeriesData> nyData = new List<LineSeriesData>();
            //List<LineSeriesData> berlinData = new List<LineSeriesData>();
            //List<LineSeriesData> londonData = new List<LineSeriesData>();

            //tokyoValues.ForEach(p => tokyoData.Add(new LineSeriesData { Y = p }));
            //nyValues.ForEach(p => nyData.Add(new LineSeriesData { Y = p }));
            //berlinValues.ForEach(p => berlinData.Add(new LineSeriesData { Y = p }));
            //londonValues.ForEach(p => londonData.Add(new LineSeriesData { Y = p }));

            //ViewData["tokyoData"] = tokyoData;
            //ViewData["nyData"] = nyData;
            //ViewData["berlinData"] = berlinData;
            //ViewData["londonData"] = londonData;

            //ViewData[""]
            //Categories = new List<string> { "Jan", "Feb", "Mar", "Apr", "May", "Jun",
            //                    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" },

            var sessionLineChart = new SessionLineChart();
            var series = new List<Series>
            {
                new LineSeries
                {
                    Name = session.PlayerEntity1.Name,
                    Data = player1Data
                },
                new LineSeries
                {
                    Name = session.PlayerEntity2.Name,
                    Data = player2Data
                }           
            };

            sessionLineChart.XAxis = new XAxis
            {
                Title = new XAxisTitle
                {
                    Text = "Count hands"
                },
                Categories = categories,
            };

            sessionLineChart.Series = series;
            return PartialView(sessionLineChart);
        }
    }
}