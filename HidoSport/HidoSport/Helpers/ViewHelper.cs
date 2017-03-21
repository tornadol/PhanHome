using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Configuration;

namespace HidoSport.Helpers
{
    public class ViewHelper
    {

        public static string GetCss(string fileName, bool isMobile = false)
        {
            if (isMobile)
                fileName += ".mobile";

            if (!HttpContext.Current.IsDebuggingEnabled && !fileName.Contains(".min"))
                fileName += ".min";

            var cacheKey = Cacher.CreateCacheKey("css", fileName);
            string content = null;

            if (!HttpContext.Current.IsDebuggingEnabled)
            {
                content = Cacher.Get<string>(cacheKey);
                if (content != null)
                {
                    return content;
                }
            }

            fileName += ".css";

            string path = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/" + fileName);
            Debug.Assert(path != null, "path != null");

            if (!File.Exists(path))
            {
                path = path.Replace(".min", "");
                if (!File.Exists(path))
                {
                    return null;
                }
            }
            content = File.ReadAllText(path);

            //var cdnImgUrl = ConfigurationManager.AppSettings[]; + "/Content/images/";
            //content = Regex.Replace(content, @"url\('\/Content\/images\/", "url('" + cdnImgUrl);
            //content = Regex.Replace(content, @"url\('images\/", "url('" + cdnImgUrl);
            //content = Regex.Replace(content, @"url\(images\/", "url(" + cdnImgUrl);
            //if (ConfigHelper.Cdn.Contains("cdn"))
            //{
            //    var version = ConfigHelper.GetStaticContentVersion();

            //    content = content.Replace("desktop.png", "desktop.v" + version + ".png");
            //    content = content.Replace("mobiledmx@2x.png", "mobiledmx@2x.v" + version + ".png");
            //    content = content.Replace("{cdn}", ConfigHelper.Cdn);
            //    content = content.Replace("{v}", ".v" + version);
            //}
            //else
            //{
            //    content = content.Replace("{cdn}", "");
            //    content = content.Replace("{v}", "");
            //}

            if (!HttpContext.Current.IsDebuggingEnabled)
            {
                Cacher.Add(cacheKey, content, DateTime.Now.AddMinutes(60));
            }

            return content;
        }
    }
}