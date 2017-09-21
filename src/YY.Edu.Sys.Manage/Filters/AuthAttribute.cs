﻿using System.Web;
using System.Web.Mvc;

namespace YY.Edu.Sys.Manage.Filters
{
    public class AuthAttribute : System.Web.Mvc.ActionFilterAttribute
    {

        /// <summary> 
        /// 角色名称 
        /// </summary> 
        public string Code { get; set; }
        /// <summary> 
        /// 验证权限（action执行前会先执行这里） 
        /// </summary> 
        /// <param name="filterContext"></param> 
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["accessToken"] == null)
            {
                //filterContext.Result = new RedirectToRouteResult("Login", new RouteValueDictionary { { "from", filterContext.RequestContext.HttpContext.Request.Url.ToString() } });
                filterContext.Result = new RedirectResult("~/Account/Login");
            }
            else
            {
                //filterContext.Controller.ViewBag.Msg = "Hello";
            }

            base.OnActionExecuting(filterContext);

            //如果存在身份信息 
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //ContentResult Content = new ContentResult();
                //Content.Content = string.Format("<script type='text/javascript'>alert('请先登录！');window.location.href='{0}';</script>", FormsAuthentication.LoginUrl);
                //filterContext.Result = Content;
            }
            else
            {
                //string[] Role = CheckLogin.Instance.GetUser().Roles.Split(',');//获取所有角色 
                //if (!Role.Contains(Code))//验证权限 
                //{
                //    //验证不通过 
                //    ContentResult Content = new ContentResult();
                //    Content.Content = "<script type='text/javascript'>alert('权限验证不通过！');history.go(-1);</script>";
                //    filterContext.Result = Content;
                //}
            }

        }
    }
}