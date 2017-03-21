using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;
using HidoSport.Areas.Admin.Models;

namespace HidoSport.Areas.Admin.Helpers
{
    public class RequestHelper
    {
        public static PhanHomeEntities ctx = new PhanHomeEntities();
        private static RequestHelper _instance;
        public static RequestHelper Instance
        {
            get { return _instance ?? (_instance = new RequestHelper()); }
        }
        public RequestModel GetList(int page, string search)
        {
            string originLink = LinkHelper.GetBase(HttpContext.Current.Request.Url.AbsolutePath);
            int itemPage = 10;
            var list = (from i in ctx.Requests
                        where String.IsNullOrEmpty(i.flag) &&
                        i.Email.Contains(search)
                        orderby i.Status descending, i.Create_Day descending
                        select i).Skip((page - 1) * itemPage).Take(itemPage).ToList();
            var listCount = (from i in ctx.Customers where String.IsNullOrEmpty(i.flag) select i).ToList().Count();
            int totalpage = PaginationHelper.GetTotal(10, listCount);
            List<int> pagination = PaginationHelper.GetPage(page, totalpage, 4);
            var model = new RequestModel()
            {
                LstItem = list,
                OriginLink = originLink,
                Page = page,
                Pagination = pagination,
                Search = search
            };
            return model;
        }
        public Request GetDetail(int id)
        {
            var req = (from i in ctx.Requests
                       where i.Id == id && String.IsNullOrEmpty(i.flag)
                       select i).FirstOrDefault();
            return req;
        }
        public Request GetEdit(int id)
        {
            var req = (from i in ctx.Requests
                        where i.Id == id && String.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
            return req;
        }
        public int Save(int id, int status)
        {
            // Id mơi
            int idNew = id;
            Request item = new Request();

            item = (from i in ctx.Requests
                    where i.Id == id && string.IsNullOrEmpty(i.flag)
                    select i).FirstOrDefault();
            if (item != null)
            {
                item.Status = status;
                item.Create_Day = DateTime.Now;
                item.Change_Day = DateTime.Now;
                ctx.SaveChanges();
            }
            return idNew;
        }
    }
}