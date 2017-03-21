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
    public class ItemHelper
    {
        public static PhanHomeEntities ctx = new PhanHomeEntities();
        public string rsrc = ConfigurationManager.AppSettings["cdn_server_erp"];
        public string domain = ConfigurationManager.AppSettings["cdn_domain_erp"];
        public static string ImageUploadPath = ConfigurationManager.AppSettings["UploadImagePath"];
        public static string ImageViewPath = ConfigurationManager.AppSettings["ImageViewPath"];
        public static string ImageViewPathTemp = ConfigurationManager.AppSettings["ImageViewPathTemp"];

        public ItemModel GetList(int page, string search)
        {
            string originLink = LinkHelper.GetBase(HttpContext.Current.Request.Url.AbsolutePath);
            int itemPage = 10;
            var list = (from i in ctx.Items
                        where String.IsNullOrEmpty(i.flag) &&
                        i.NameCode.Contains(search)
                        orderby i.Status descending, i.Create_Day descending
                        select i).Skip((page - 1) * itemPage).Take(itemPage).ToList();
            var listCount = (from i in ctx.Items where String.IsNullOrEmpty(i.flag) select i).ToList().Count();
            int totalpage = PaginationHelper.GetTotal(10, listCount);
            List<int> pagination = PaginationHelper.GetPage(page, totalpage, 4);
            var model = new ItemModel()
            {
                LstItem = list,
                OriginLink = originLink,
                Page = page,
                Pagination = pagination,
                Search = search
            };
            return model;
        }
        public Item GetDetail(int id)
        {
            var item = (from i in ctx.Items
                        where i.Id == id
                        select i).FirstOrDefault();
            return item;
        }
        public ItemEdit GetEdit(int id)
        {
            var item = (from i in ctx.Items
                        where i.Id == id && String.IsNullOrEmpty(i.flag)
                        select i).FirstOrDefault();
            var model = new ItemEdit()
            {
                Item = item
            };
            return model;
        }
        public static int Save(int id, string name, string des, int cate, int cateChild, int status, double price,string fileLstImg, HttpPostedFileBase file,string checkBackspace,int highlight)
        {
            // Id mơi
            int idNew = id;
            string typeImg = "";
            string nameCode = "";
            //Lấy ra tên của 
            Item item = new Item();
            if (file != null)
            {
                typeImg = Path.GetExtension(file.FileName);
                nameCode = Extension.RemoveUnicodeLower(name);
            }
            //Make Link
            //string catename = (from i in ctx.Cates where i.Id == cate select i.FullName).FirstOrDefault();
            string url = "/"+ Extension.RemoveUnicodeLower(name);
            string imgSrc = nameCode + typeImg;
            string fileName = nameCode + typeImg;
            if (imgSrc != "" && id == 0)
            {
                
                if (imgSrc != null)
                {
                    item.ImgSrc = "/Areas/Admin/Assets/Temp/" + imgSrc;
                }
                item.Description = des;
                item.CateId = cate;
                item.Name = name;
                item.NameCode = Extension.RemoveUnicodeLower(name);
                item.Price = price;
                item.Status = status;
                item.CateChildId = cateChild;
                item.Create_Day = DateTime.Now;
                item.Change_Day = DateTime.Now;
                item.Link = url;
                item.highlight = highlight;
                ctx.Items.Add(item);
                ctx.SaveChanges();
                idNew = item.Id;
            }
            else
            {
                item = (from i in ctx.Items
                            where i.Id == id && string.IsNullOrEmpty(i.flag)
                            select i).FirstOrDefault();
                if (item != null)
                {
                    if (!String.IsNullOrEmpty(imgSrc))
                    {
                        item.ImgSrc = "/Areas/Admin/Assets/Temp/"  + imgSrc;
                    }
                    item.Description = des;
                    item.CateId = cate;
                    item.Name = name;
                    item.NameCode = Extension.RemoveUnicodeLower(name);
                    item.Price = price;
                    item.Status = status;
                    item.CateChildId = cateChild;
                    item.Create_Day = DateTime.Now;
                    item.Change_Day = DateTime.Now;
                    item.Link = url;
                    item.highlight = highlight;
                    ctx.SaveChanges();
                }
            }
            List<ImageDetail> lstimage = new List<ImageDetail>();
            if (id != 0)
            {
                //Nếu có chính sửa
                if(checkBackspace == "yes")
                {
                    //Xoá lst ảnh nếu đó là edit và cập nhập lại
                    var lstImageOld = (from i in ctx.ImageDetails where i.ItemId == id && String.IsNullOrEmpty(i.flag)  select i).ToList();
                    foreach (var image in lstImageOld)
                    {
                        image.flag = "D";
                        image.Change_Day = DateTime.Now;
                        ctx.SaveChanges();
                    }
                    //Lưu danh sách hình ảnh
                    string test = fileLstImg;
                    var lstfileLstImg = fileLstImg.Split(',');
                    foreach (var i in lstfileLstImg)
                    {
                        var imageDetail = new ImageDetail()
                        {
                            Change_Day = DateTime.Now,
                            Create_Day = DateTime.Now,
                            ItemId = idNew,
                            Name = fileName,
                            Link = i
                        };
                        lstimage.Add(imageDetail);
                    }
                    foreach (var y in lstimage)
                    {
                        ctx.ImageDetails.Add(y);
                    }
                    ctx.SaveChanges();
                }
            }
            else
            {
                //Lưu danh sách hình ảnh
                string test = fileLstImg;
                var lstfileLstImg = fileLstImg.Split(',');
                foreach (var i in lstfileLstImg)
                {
                    var imageDetail = new ImageDetail()
                    {
                        Change_Day = DateTime.Now,
                        Create_Day = DateTime.Now,
                        ItemId = idNew,
                        Name = fileName,
                        Link = i
                    };
                    lstimage.Add(imageDetail);
                }
                foreach (var y in lstimage)
                {
                    ctx.ImageDetails.Add(y);
                }
                ctx.SaveChanges();
            }
            //Lưu ảnh
            if (nameCode != "")
            {
                string upload = (ImageUploadPath);
                var savedFileName = Path.Combine(upload, Path.GetFileName(fileName));
                var rootUpload = ImageUploadPath;
                file.SaveAs(savedFileName);
            }
            return idNew;
        }
        public bool Delete(int id)
        {
            var item = (from i in ctx.Items
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
        public static string ConvertCategory(int? cateId = 0)
        {
            string name = (from i in ctx.Cates where i.Id == cateId && String.IsNullOrEmpty(i.flag) && i.Status == 1 select i.Name).FirstOrDefault();
            return name;
        }
        public static string ConvertCategoryChild(int? cateId = 0)
        {
            string name = (from i in ctx.CateChilds where i.Id == cateId && String.IsNullOrEmpty(i.flag) && i.Status == 1 select i.Name).FirstOrDefault();
            return name;
        }
        public static List<DataAccess.Cate> GetCate()
        {
            var lstCate = (from i in ctx.Cates where String.IsNullOrEmpty(i.flag) && i.Status == 1 select i).ToList();
            return lstCate;
        }
        public static List<DataAccess.CateChild> GetCateChild(int? cateId)
        {
            var lstCateChild = (from i in ctx.CateChilds where String.IsNullOrEmpty(i.flag) && i.CateId == cateId select i).ToList();
            return lstCateChild;
        }
        public static List<ImageDetail> GetLstImg(int id)
        {
            var lstItem = (from i in ctx.ImageDetails where i.ItemId == id && String.IsNullOrEmpty(i.flag) select i).ToList();
            return lstItem;
        }
    }
}