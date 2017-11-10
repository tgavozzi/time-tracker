using Project.Web.DAL;
using Project.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project.Web.Controllers
{
    public class HomeController : Controller
    {
        private ITimeClockDAL dal;

        public HomeController(ITimeClockDAL dal)
        {
            this.dal = dal;
        }

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult ClockInOrClockOut()
        {
            return View("ClockInOrClockOut");
        }

        [HttpPost]
        public ActionResult ClockInOrClockOut(string username)
        {
           if(dal.CanClockIn(username))
            {
                TimeCard card = new TimeCard();
                card.Username = username;
                dal.ClockIn(card);
                return RedirectToAction("TimeCardHistoryResult", new { username = username });
            }
            else
            {
               
                return RedirectToAction("ClockOut", new { username = username });
            }
            
        }

        public ActionResult ClockOut(string username)
        {
            TimeCard finishProject = new TimeCard();
            finishProject.Username = username;
            return View("ClockOut", finishProject);
        }

        [HttpPost]
        public ActionResult ClockOut(TimeCard card)
        {
            dal.ClockOut(card);

            return RedirectToAction("Index");
        }

        public ActionResult TimeCardHistory()
        {
            return View("TimeCardHistory");
        }


        public ActionResult TimeCardHistoryResult(string username)
        {
            var timeCardRecords = dal.GetTimeCardHistory(username);

            return View("TimeCardHistoryResult", timeCardRecords);
        }

    }
}