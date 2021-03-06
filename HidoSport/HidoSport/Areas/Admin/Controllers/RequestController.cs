﻿using HidoSport.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HidoSport.Areas.Admin.Helpers;
using DataAccess;


namespace HidoSport.Areas.Admin.Controllers
{
    public class RequestController : AdminController
    {
        [FilterConfig.SessionExpire]
        // GET: Admin/Customer
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
            var lstItem = RequestHelper.Instance.GetList(page, search);
            return View(lstItem);
        }
        [FilterConfig.SessionExpire]
        public ActionResult Detail(int id)
        {
            var req = RequestHelper.Instance.GetDetail(id);
            return View(req);
        }
        [FilterConfig.SessionExpire]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return null;
            }
            Request req = RequestHelper.Instance.GetEdit(id);
            return View(req);
        }
        [FilterConfig.SessionExpire]
        public ActionResult Save(FormCollection form, HttpPostedFileBase file, int id = 0)
        {
            //Khai báo các thông tin
            int status = 0;
            int idSussces = 0;
            // Get value của các input
            string tmp = Request.Form["status"];
            if (!String.IsNullOrEmpty(tmp))
                status = int.Parse(tmp);
            tmp = Request.Form["id"];
            if (!String.IsNullOrEmpty(tmp))
                id = int.Parse(tmp);
            if (id == 0)
            {
                idSussces = RequestHelper.Instance.Save(id, status);
            }
            else
            {
                idSussces = RequestHelper.Instance.Save(id, status);
            }
            return RedirectToAction("Detail", new { id = idSussces });
        }
    }
}