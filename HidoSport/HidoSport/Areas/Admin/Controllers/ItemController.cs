using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HidoSport.App_Start;
using System.Web.Mvc;
using DataAccess;
using HidoSport.Areas.Admin.Helpers;
using HidoSport.Areas.Admin.Models;
using System.IO;
using System.Diagnostics;
using System.Configuration;

namespace HidoSport.Areas.Admin.Controllers
{

    public class ItemController : AdminController
    {
        string ImageUploadPath = ConfigurationManager.AppSettings["UploadImagePath"];
        string ImageViewPath = ConfigurationManager.AppSettings["ImageViewPath"];
        string ImageUploadPathTemp = ConfigurationManager.AppSettings["UploadImagePathTemp"];
        string ImageViewPathTemp = ConfigurationManager.AppSettings["ImageViewPathTemp"];
        public ItemHelper hel = new ItemHelper();
        public List<string> LstImge = null;
        // GET: Admin/Item
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
            Item item = hel.GetDetail(id);
            return View(item);
        }
        [FilterConfig.SessionExpire]
        public ActionResult Edit(int id)
        {
            if (id != 0)
            {
                ItemEdit item = hel.GetEdit(id);
                return View(item);
            }
            return View("AddNew");
        }

        [ValidateInput(false)]
        [FilterConfig.SessionExpire]
        public ActionResult Save(FormCollection form, HttpPostedFileBase file, int id = 0)
        {
            //Khai báo các thông tin
            int status = 1;
            int statusHighlight = 1;
            int cate = 0;
            int cateChild = 0;
            string name = "";
            string des = "";
            double price = 0;
            int idSussces = 0;
            string fileLstImg = "";
            string checkBackspace = "";
            // Get value của các input
            string tmp = Request.Form["name"];
            if (!String.IsNullOrEmpty(tmp))
                name = tmp;
            tmp = form["fulldes"];
            if (!String.IsNullOrEmpty(tmp))
                des = tmp;
            tmp = Request.Form["status"];
            if (!String.IsNullOrEmpty(tmp))
                status = int.Parse(tmp);
            tmp = Request.Form["statusHighlight"];
            if (!String.IsNullOrEmpty(tmp))
                statusHighlight = int.Parse(tmp);
            tmp = Request.Form["cate"];
            if (!String.IsNullOrEmpty(tmp))
                cate = int.Parse(tmp);
            tmp = Request.Form["cateChild"];
            if (!String.IsNullOrEmpty(tmp))
                cateChild = int.Parse(tmp);
            tmp = Request.Form["id"];
            if (!String.IsNullOrEmpty(tmp))
                id = int.Parse(tmp);
            tmp = Request.Form["price"];
            if (!String.IsNullOrEmpty(tmp))
            {
                price = Convert.ToDouble(tmp);
            }
            tmp = Request.Form["checkBackspace"];
            if (!String.IsNullOrEmpty(tmp))
            {
                checkBackspace = tmp;
            }
            tmp = Request.Form["lstImagesNew"];
            if (!String.IsNullOrEmpty(tmp))
            {
                fileLstImg = tmp;
            }
            if (name != "" && id == 0)
            {
                idSussces = ItemHelper.Save(id, name, des, cate, cateChild, status, price, fileLstImg, file, null,statusHighlight);
                return RedirectToAction("Detail", new { id = idSussces });
            }
            else
            {
                idSussces = ItemHelper.Save(id, name,des, cate, cateChild, status, price, fileLstImg,file, checkBackspace, statusHighlight);
                return RedirectToAction("Detail", new { id = idSussces });
            }
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

        // GET: Admin/UploadLstImg
        public ActionResult upload()
        {
            string savedFileName = string.Empty;
            string imgUrl = string.Empty;
            var randomNumbers = new Random().Next(1, 1000);
            string guildId = Guid.NewGuid().ToString().Substring(0, 12);

            try
            {
                foreach (string file in Request.Files)
                {
                    var hpf = this.Request.Files[file];
                    if (hpf.ContentLength == 0)
                    {
                        continue;
                    }

                    if (hpf.ContentLength > 1024000)
                        return Json(new
                        {
                            imageUrl = imgUrl,
                            ImageName = hpf.FileName,
                            ImageId = guildId,
                            message = "maximum size upload",
                            isSucess = false
                        }, JsonRequestBehavior.AllowGet);

                    try
                    {
                        var filename = string.Format("{0}.jpg", guildId);
                        string url = ImageUploadPathTemp ;

                        var dirInfo = new DirectoryInfo(url);
                        if (!dirInfo.Exists)
                            dirInfo.Create();

                       
                        savedFileName = Path.Combine(url, Path.GetFileName(filename));
                        //if (ImageViewPath.IndexOf("https://") == -1)
                        //    ImageViewPath = "https://" + ImageViewPath;

                        //var rootUpload = ImageUploadPath + "/Temp/" + date;
                        //System.IO.Directory.CreateDirectory(rootUpload);
                        //var rootView = ImageViewPath + "/Temp/" + date + "/" + filename;
                        var rootUpload = ImageUploadPathTemp ;
                        System.IO.Directory.CreateDirectory(rootUpload);
                        var rootView = ImageViewPathTemp + filename;
                        imgUrl = string.Format(rootView);
                        hpf.SaveAs(savedFileName);
                        ImageOptimization(savedFileName);
                    }
                    catch (Exception ex)
                    {
                        return Json(new
                        {
                            imageUrl = imgUrl,
                            ImageName = string.Format("{0}.jpg", guildId),
                            ImageId = guildId,
                            message = "Error! " + ex.ToString(),
                            isSucess = true
                        }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception err)
            {
                int TimedOutExceptionCode = -2147467259;
                var http = err as HttpException;
                if (http != null && http.ErrorCode == TimedOutExceptionCode)
                {
                    return Json(new
                    {
                        imageUrl = imgUrl,
                        ImageName = "",
                        ImageId = guildId,
                        message = "upload failed max",
                        isSucess = false
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        imageUrl = imgUrl,
                        ImageName = "",
                        ImageId = guildId,
                        message = "upload failed",
                        isSucess = false
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new
            {
                imageUrl = imgUrl,
                ImageName = string.Format("{0}.jpg", guildId),
                ImageId = guildId,
                message = "upload sucessfully",
                isSucess = true
            }, JsonRequestBehavior.AllowGet);
        }
        public void ImageOptimization(string path)
        {
            try
            {
                //jpegtran.exe -optimize -progressive -copy none <file name> <dich>
                if (path.ToLower().EndsWith(".jpg"))
                {
                    var pi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = HttpRuntime.AppDomainAppPath + @"\ImageOptimization\jpegtran.exe",
                        Arguments = " -optimize -progressive -copy none \"" + path + "\" \"" + path + "\"",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    Process.Start(pi);
                }
                //optipng.exe -strip all -o7 <tên file>
                if (path.ToLower().EndsWith(".png"))
                {
                    var pi = new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = HttpRuntime.AppDomainAppPath + @"\ImageOptimization\optipng.exe",
                        Arguments = " -strip all -o7  \"" + path + "\" \"" + path + "\"",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    Process.Start(pi);
                }

            }
            catch { }
        }
        public ActionResult GetCateChild(int idCate = 0)
        {
            ViewBag.IdCate = idCate;
            var listCateChild = ItemHelper.GetCateChild(idCate);
            return PartialView(listCateChild);
        }
    }
}