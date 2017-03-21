using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HidoSport.Areas.Admin.Helpers;
using HidoSport.App_Start;
using HeyRed.MarkdownSharp;
using DataAccess;

namespace HidoSport.Areas.Admin.Controllers
{
    public class NewController : AdminController
    {
        // GET: Admin/New
        public  NewHelper hel = new NewHelper();
        // GET: Admin/Category
        [FilterConfig.SessionExpire]
        public ActionResult Index()
        {
            int page = 1;
            string search = "";
            string tmp = Request.QueryString["p"];
            if (!String.IsNullOrEmpty(tmp))
                page = int.Parse(tmp);
            tmp = Request.QueryString["s"];
            if (!String.IsNullOrEmpty(tmp))
                search = tmp;
            var lstItem = hel.GetList(page, search);
            return View(lstItem);
        }
        [FilterConfig.SessionExpire]
        public ActionResult Edit(int id)
        {
            if (id != 0)
            {
               New item = hel.GetEdit(id);
               return View(item);
            }
            return View("AddNew");
        }
        [ValidateInput(false)]
        [FilterConfig.SessionExpire]
        public ActionResult Save(FormCollection formdata, string data, HttpPostedFileBase file)
        { 
            int id = formdata["id"] == null ? 0 : Convert.ToInt32(formdata["id"]);
            var name = formdata["name"];
            int cate = formdata["cate"] == null ? 0: Convert.ToInt32(formdata["cate"]);
            var fulldes = formdata["fulldes"];
            int status = formdata["status"] == null ? 0 : Convert.ToInt32(formdata["status"]);
           
            int idSussces = 0;
            if (id == 0)
            {
                idSussces = NewHelper.Save(id, name.Trim(), cate, status, fulldes,file);
                return RedirectToAction("Detail", new { id = idSussces });
            }
            else
            {
                idSussces = NewHelper.Save(id, name.Trim(), cate, status, fulldes, file);
                return RedirectToAction("Detail", new { id = idSussces });
            }
        }
        [FilterConfig.SessionExpire]
        public ActionResult Detail(int id)
        {
            New item = hel.GetDetail(id);
            return View(item);
        }
        [FilterConfig.SessionExpire]
        public ActionResult Delete(int id)
        {
            if (id != 0)
            {
                bool item = hel.Delete(id);
                if (item)
                {
                    return Json(new
                    {
                        ResultCode = 1
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        ResultCode = 0
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new
                {
                    ResultCode = 0,
                }, JsonRequestBehavior.AllowGet);
            }
        }

    }
    
}