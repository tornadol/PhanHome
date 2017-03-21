using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HidoSport.Areas.Admin.Models;
using DataAccess;
using HidoSport.Helpers;

namespace HidoSport.Areas.Admin.Helpers
{
    public class CategoryHelper
    {
        public static PhanHomeEntities ctx = new PhanHomeEntities();
        public CategoryModel GetList(int page, string search)
        {
            string originLink = LinkHelper.GetBase(HttpContext.Current.Request.Url.AbsolutePath);
            int itemPage = 10;
            var list = (from i in ctx.Cates
                        where String.IsNullOrEmpty(i.flag) &&
                        i.NameCode.Contains(search)
                        orderby i.Status descending, i.Sort,i.Create_Day descending
                        select i).Skip((page - 1) * itemPage).Take(itemPage).ToList();
            var listCount = (from i in ctx.Items where String.IsNullOrEmpty(i.flag) select i).ToList().Count();
            int totalpage = PaginationHelper.GetTotal(10, listCount);
            List<int> pagination = PaginationHelper.GetPage(page, totalpage, 4);
            var model = new CategoryModel()
            {
                LstItem = list,
                OriginLink = originLink,
                Page = page,
                Pagination = pagination,
                Search = search
            };
            return model;
        }
        public Cate GetDetail(int id)
        {
            var item = (from i in ctx.Cates
                        where i.Id == id && String.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
            return item;
        }
        public Cate GetEdit(int id)
        {
            var item = (from i in ctx.Cates
                        where i.Id == id && String.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
            return item;
        }
        public bool Delete(int id)
        {
            var item = (from i in ctx.Cates
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
        public int Save(int id, string name, int status,int sort)
        {
            // Id mơi
            int idNew = id;
            Cate item = new Cate();
            if (id == 0)
            {
                item.Name = name;
                item.NameCode = Extension.RemoveUnicodeLower(name);
                item.Status = status;
                item.Sort = sort;
                item.Create_Day = DateTime.Now;
                item.Change_Day = DateTime.Now;
                ctx.Cates.Add(item);
                ctx.SaveChanges();
                idNew = item.Id;
            }
            else
            {
                item = (from i in ctx.Cates
                        where i.Id == id && string.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
                if (item != null)
                {
                    item.Name = name;
                    item.NameCode = Extension.RemoveUnicodeLower(name);
                    item.Status = status;
                    item.Sort = sort;
                    item.Create_Day = DateTime.Now;
                    item.Change_Day = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
            return idNew;
        }
    }
}