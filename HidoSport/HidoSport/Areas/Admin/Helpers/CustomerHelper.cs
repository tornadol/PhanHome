using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;
using HidoSport.Areas.Admin.Models;

namespace HidoSport.Areas.Admin.Helpers
{

    public class CustomerHelper
    {
        public static PhanHomeEntities ctx = new PhanHomeEntities();
        public CustomerModel GetList(int page, string search)
        {
            string originLink = LinkHelper.GetBase(HttpContext.Current.Request.Url.AbsolutePath);
            int itemPage = 10;
            var list = (from i in ctx.Customers
                        where String.IsNullOrEmpty(i.flag) &&
                        i.Name.Contains(search)
                        orderby i.Status descending, i.Create_Day descending
                        select i).Skip((page - 1) * itemPage).Take(itemPage).ToList();
            var listCount = (from i in ctx.Customers where String.IsNullOrEmpty(i.flag) select i).ToList().Count();
            int totalpage = PaginationHelper.GetTotal(10, listCount);
            List<int> pagination = PaginationHelper.GetPage(page, totalpage, 4);
            var model = new CustomerModel()
            {
                LstItem = list,
                OriginLink = originLink,
                Page = page,
                Pagination = pagination,
                Search = search
            };
            return model;
        }
        public DetailCustomer GetDetail(int id)
        {
            var cus = (from i in ctx.Customers
                        where i.Id == id && String.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
            var lstCusBuy = (from b in ctx.CustomerBuys
                             where b.IdCustomer == cus.Id
                             && String.IsNullOrEmpty(b.flag)
                             select b).ToList();
            var model = new DetailCustomer
            {
                Cus = cus,
                Cusbuy = lstCusBuy
            };
            return model;
        }
        public Customer GetEdit(int id)
        {
            var item = (from i in ctx.Customers
                        where i.Id == id && String.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
            return item;
        }
        public int Save(int id, int status)
        {
            // Id mơi
            int idNew = id;
            Customer item = new Customer();
            
            item = (from i in ctx.Customers
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
        public bool Delete(int id)
        {
            var item = (from i in ctx.Customers
                        where i.Id == id && String.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
            if (item == null)
            {
                return false;
            }
            else
            {
                item.flag = "D";
                ctx.SaveChanges();
                return true;
            }
        }
    }
}