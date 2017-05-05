using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebPokerSessionManager.Controllers
{
    [Authorize]
    public class PokerSessionController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNewSession(int numberOfHands)
        {
            return View();
        }
    }
}
