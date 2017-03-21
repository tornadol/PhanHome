using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HidoSport.Areas.Admin.Helpers;
using HidoSport.App_Start;
using DataAccess;

namespace HidoSport.Areas.Admin.Controllers
{
    public class CategoryChildController : AdminController
    {
        // GET: Admin/CategoryChild
        public CategoryChildHelper hel = new CategoryChildHelper();
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
        public ActionResult Detail(int id)
        {
            CateChild item = hel.GetDetail(id);
            return View(item);
        }
        [FilterConfig.SessionExpire]
        public ActionResult Edit(int id)
        {
            if (id != 0)
            {
                CateChild item = hel.GetEdit(id);
                return View(item);
            }
            return View("AddNew");
        }
        [FilterConfig.SessionExpire]
        public ActionResult Save(FormCollection form, HttpPostedFileBase file, int id = 0)
        {
            //Khai báo các thông tin
            int status = 1;
            string name = "";
            int cate = 0;
            int idSussces = 0;
            int sort = 0;
            // Get value của các input
            string tmp = Request.Form["name"];
            if (!String.IsNullOrEmpty(tmp))
                name = tmp;
            tmp = Request.Form["status"];
            if (!String.IsNullOrEmpty(tmp))
                status = int.Parse(tmp);
            tmp = Request.Form["id"];
            if (!String.IsNullOrEmpty(tmp))
                id = int.Parse(tmp);
            tmp = Request.Form["cate"];
            if (!String.IsNullOrEmpty(tmp))
                cate = int.Parse(tmp);
            tmp = Request.Form["sort"];
            if (!String.IsNullOrEmpty(tmp))
                sort = int.Parse(tmp);
            if (name != "" && id == 0)
            {
                idSussces = hel.Save(id, name, status,cate, sort);
            }
            else
            {
                idSussces = hel.Save(id, name, status,cate, sort);
            }
            return RedirectToAction("Detail", new { id = idSussces });
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