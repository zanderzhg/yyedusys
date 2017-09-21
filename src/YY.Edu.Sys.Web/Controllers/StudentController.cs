﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc;

namespace YY.Edu.Sys.Web.Controllers
{
    
    public class StudentController : Controller
    {
        public System.Web.Mvc.ActionResult Index()
        {

            return View();
        }


        public System.Web.Mvc.ActionResult MyCurriculum()
        {
            //var obj =  base.Me();
            // Sys.Models.Student s = obj.Result;           
            // ViewBag.VenueID = s.VenueID;
            // ViewBag.StudentID = s.StudentID;

            ViewBag.VenueID = 1;
            ViewBag.StudentID =1;
            return View();
        }

        public ActionResult MyEvaluate(int pkid,int sid,int cid)
        {
            ViewBag.PKID = pkid;
            ViewBag.SID = sid;
            ViewBag.CID = cid;
            return View();
        }

        /// <summary>
        /// 购买课时
        /// </summary>
        /// <param name="VenueID"></param>
        /// <param name="StudentID"></param>
        /// <returns></returns>
        public ActionResult BuyHourClass()
        {
            //var obj = base.Me();
            //Sys.Models.Student s = obj.Result;
            //ViewBag.VenueID = s.VenueID;
            //ViewBag.StudentID = s.StudentID;
            ViewBag.VenueID = 1;
            ViewBag.StudentID = 1;
            return View();
        }

        public ActionResult SubscribeCurriculum()
        {
            //var obj = base.Me();
            //Sys.Models.Student s = obj.Result;
            //ViewBag.VenueID = s.VenueID;
            //ViewBag.StudentID = s.StudentID;
            ViewBag.VenueID = 1;
            ViewBag.StudentID = 1;
            ViewBag.SName = "小赵子";
            return View();
        }


        public ActionResult StudentGrowth()
        {
            //var obj = base.Me();
            //Sys.Models.Student s = obj.Result;
            //ViewBag.VenueID = s.VenueID;
            //ViewBag.StudentID = s.StudentID;
            ViewBag.VenueID = 1;
            ViewBag.StudentID = 1;
            return View();
        }


        public ActionResult MyMessage()
        {
            //var obj = base.Me();
            //Sys.Models.Student s = obj.Result;
            //ViewBag.VenueID = s.VenueID;
            //ViewBag.StudentID = s.StudentID;
            ViewBag.VenueID = 1;
            ViewBag.StudentID = 1;
            return View();
        }


        public ActionResult MyClassHoursDetailed()
        {
            //var obj = base.Me();
            //Sys.Models.Student s = obj.Result;
            //ViewBag.VenueID = s.VenueID;
            //ViewBag.StudentID = s.StudentID;
            ViewBag.VenueID = 1;
            ViewBag.StudentID = 1;
            return View();
        }


        public ActionResult BindingUser()
        {
            return View();
        }


        public ActionResult CurriculumDetail(int cid,int pkid)
        {
            ViewBag.CID = cid;
            ViewBag.StudentID = 1;
            ViewBag.PKID = pkid;
            return View();
        }
    }
}
