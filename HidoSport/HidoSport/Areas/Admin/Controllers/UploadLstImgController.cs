using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HidoSport.Areas.Admin.Controllers
{
    public class UploadLstImgController : AdminController
    {
        string ImageUploadPath = ConfigurationManager.AppSettings["UploadImagePath"]; 
        string ImageViewPath = ConfigurationManager.AppSettings["ImageViewPath"];
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
                        string url = (ImageUploadPath + "Temp\\");

                        var dirInfo = new DirectoryInfo(url);
                        if (!dirInfo.Exists)
                            dirInfo.Create();

                        savedFileName = Path.Combine(url, Path.GetFileName(filename));
                        //if (ImageViewPath.IndexOf("https://") == -1)
                        //    ImageViewPath = "https://" + ImageViewPath;

                        imgUrl = string.Format(ImageViewPath + "/Temp/{0}", filename);
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
    }
}