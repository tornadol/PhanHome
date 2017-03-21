using HidoSport.App_Start;
using HidoSport.Areas.Admin.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HidoSport.Areas.Admin.Controllers
{
    public class HomeController : AdminController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            ViewBag.Error = TempData["Error"] == null ? "" : TempData["Error"].ToString();
            return PartialView();
        }
        [HttpPost]

        public ActionResult CheckLogin(FormCollection req)
        {
            string user = Request["Username"];
            string pass = Request["Password"];
            LoginHelper helper = new LoginHelper();
            if (!String.IsNullOrEmpty(user) && !String.IsNullOrEmpty(pass))
            {
                int keyValue = helper.CheckUser(user, pass);
                if (keyValue > 0)
                {
                    Session["keyValue"] = keyValue;
                }
                else
                {
                    TempData["Error"] = "Mật khẩu hoặc tên đăng nhập không đúng";
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("LoadMenu");
        }
        [FilterConfig.SessionExpire]
        public ActionResult CheckLogin()
        {
            return View();
        }
        [FilterConfig.SessionExpire]
        public ActionResult LoadMenu()
        {
            LoginHelper helper = new LoginHelper();
            return View();
        }
        [FilterConfig.SessionExpire]
        public ActionResult MenuDetail()
        {
            return PartialView();
        }
    }
}