using HidoSport.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HidoSport.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin/Admin
        
        public AdminController() : base()
        {
            ViewBag.rsrc = ConfigurationManager.AppSettings["cdn_server_erp"];
            ViewBag.domain = ConfigurationManager.AppSettings["cdn_domain_erp"];
        }
        //protected override void OnActionExecuting(ActionExecutingContext filterContext)
        //{
        //    // If session exists
        //    if (filterContext.HttpContext.Session != null)
        //    {
        //        //if new session
        //        if (filterContext.HttpContext.Session.IsNewSession)
        //        {
        //            string cookie =
        //                filterContext.HttpContext.Request.Headers["Cookie"];
        //            //if cookie exists and sessionid index is greater than zero
        //            if ((cookie != null) &&
        //                (cookie.IndexOf("ASP.NET_SessionId") >= 0))
        //            {
        //                //redirect to desired session 
        //                //expiration action and controller
        //                filterContext.Result = new RedirectResult("~/administrator");
        //                return;
        //            }
        //        }
        //    }
        //    //otherwise continue with action
        //    base.OnActionExecuting(filterContext);
        //}
    }
}