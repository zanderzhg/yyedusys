﻿using Senparc.Weixin;
using Senparc.Weixin.HttpUtility;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using Senparc.Weixin.MP.CommonAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YY.Edu.Sys.Web.Controllers
{
    public class MediaController : Controller
    {
        // GET: Media
        public ActionResult Index()
        {

            string accessTokenOrAppId = "oWWNy6_-s49Er9_UtT_kmAakPTaZzZewNcp2PY-Tc2YChTuM56jxl1wWr5ZNmxhr5TAOo_lmJSGnQ-GNuMm7m2S-FtliCnq6Lsh7DrMTijpaQYMUfrzlLN0Xl0MFbKS3QJNeACADWA";
            MediaList_NewsResult media = ApiHandlerWapper.TryCommonApi(accessToken =>
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/material/batchget_material?access_token={0}", accessToken.AsUrlData());

                var date = new
                {
                    type = "news",
                    offset = 0,
                    count = 15
                };

                return CommonJsonSend.Send<MediaList_NewsResult>(null, url, date, CommonJsonSendType.POST, 10000);

            }, accessTokenOrAppId);

            string jsonStr = Comm.Helper.JsonHelper.ToJsonStringByNewtonsoft(media);

            ViewBag.media = jsonStr;
            return View();
        }
    }
}