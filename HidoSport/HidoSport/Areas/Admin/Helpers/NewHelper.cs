using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataAccess;
using System.Configuration;
using HidoSport.Areas.Admin.Models;
using System.IO;
using HidoSport.Helpers;

namespace HidoSport.Areas.Admin.Helpers
{
    public class NewHelper
    {
        public static PhanHomeEntities ctx = new PhanHomeEntities();
        public string rsrc = ConfigurationManager.AppSettings["cdn_server_erp"];
        public string domain = ConfigurationManager.AppSettings["cdn_domain_erp"];
        public static string ImageUploadPath = ConfigurationManager.AppSettings["UploadImagePath"];
        public static string ImageViewPath = ConfigurationManager.AppSettings["ImageViewPath"];

        public NewModel GetList(int page, string search)
        {
            string originLink = LinkHelper.GetBase(HttpContext.Current.Request.Url.AbsolutePath);
            int itemPage = 10;
            var list = (from i in ctx.News
                        where String.IsNullOrEmpty(i.flag) &&
                        i.NameCode.Contains(search)
                        orderby i.Status descending, i.Create_Day descending
                        select i).Skip((page - 1) * itemPage).Take(itemPage).ToList();
            var listCount = (from i in ctx.News where String.IsNullOrEmpty(i.flag) select i).ToList().Count();
            int totalpage = PaginationHelper.GetTotal(10, listCount);
            List<int> pagination = PaginationHelper.GetPage(page, totalpage, 4);
            var model = new NewModel()
            {
                LstItem = list,
                OriginLink = originLink,
                Page = page,
                Pagination = pagination,
                Search = search
            };
            return model;
        }

        public static int Save(int id, string name,int cate, int status, string fulldes, HttpPostedFileBase file)
        {
            // Id mơi
            int idNew = id;
            string typeImg = "";
            string nameCode = "";
            //Lấy ra tên của 
            New item = new New();
            if (file != null)
            {
                typeImg = Path.GetExtension(file.FileName);
                nameCode = Extension.RemoveUnicodeLower(name);
            }
            //Make Link
            //string catename = (from i in ctx.Cates where i.Id == cate select i.FullName).FirstOrDefault();
            string url = "/" + Extension.RemoveUnicodeLower(name);
            string imgSrc = nameCode + typeImg;
            string fileName = nameCode + typeImg;
            if (id == 0)
            {
                if (!String.IsNullOrEmpty(imgSrc))
                {
                    item.ImgSrc = "Areas/Admin/Assets/Temp/New/" + imgSrc;
                }
                item.FullDes = fulldes;
                item.CategoryId = cate;
                item.Name = name;
                item.NameCode = Extension.RemoveUnicodeLower(name);
                item.Status = status;
                item.Create_Day = DateTime.Now;
                item.Change_Day = DateTime.Now;
                ctx.News.Add(item);
                ctx.SaveChanges();
                idNew = item.Id;
            }
            else
            {
                item = (from i in ctx.News
                        where i.Id == id && string.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
                if (item != null)
                {
                    if (!String.IsNullOrEmpty(imgSrc))
                    {
                        item.ImgSrc = "Areas/Admin/Assets/Temp/New/" + imgSrc;
                    }
                    item.FullDes = fulldes;
                    item.CategoryId = cate;
                    item.Name = name;
                    item.NameCode = Extension.RemoveUnicodeLower(name);
                    item.Status = status;
                    item.Create_Day = DateTime.Now;
                    item.Change_Day = DateTime.Now;
                    ctx.SaveChanges();
                }
            }
            List<ImageDetail> lstimage = new List<ImageDetail>();
            //Lưu ảnh
            if (nameCode != "")
            {
                string upload = (ImageUploadPath) + "/New/"; 
                var savedFileName = Path.Combine(upload, Path.GetFileName(fileName));
                var rootUpload = ImageUploadPath;
                file.SaveAs(savedFileName);
            }
            return idNew;
        }
        public New GetDetail(int id)
        {
            var item = (from i in ctx.News
                        where i.Id == id
                        select i).FirstOrDefault();
            return item;
        }
        public New GetEdit(int id)
        {
            var item = (from i in ctx.News
                        where i.Id == id && String.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
            return item;
        }
        public bool Delete(int id)
        {
            var item = (from i in ctx.News
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