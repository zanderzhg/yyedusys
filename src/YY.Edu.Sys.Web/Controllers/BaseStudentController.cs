﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace YY.Edu.Sys.Web.Controllers
{
    public class BaseStudentController : BaseController
    {
        // GET: BaseStudent

        private string studentDomain = System.Configuration.ConfigurationManager.AppSettings["StudentDomain"];


        public async Task<Sys.Models.Student> Me()
        {
            await GetMe();
            if (Session["Binding"] != null && Session["Binding"].ToString() != "0")
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Sys.Models.Student>(Session[studentDomain].ToString());
            }
            return null;
        }

        private async Task GetMe()
        {
            await base.LoginSuccess(studentDomain, base.OpenId, "");

            if (Session["Binding"] != null && Session["Binding"].ToString() != "0")
            {
                await base.GetMineInfo(studentDomain, base.AccessToken, base.OpenId);
            }
        }

    }
}