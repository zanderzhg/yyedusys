﻿using System.Web.Mvc;

namespace YY.Edu.Sys.Admin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Session["venue"] = new YY.Edu.Sys.Models.Venue() { VenueID = 1 };
            return View();
        }

        public ActionResult DashboardV1()
        {
            return View();
        }
        public ActionResult DashboardV2()
        {
            return View();
        }
    }
}