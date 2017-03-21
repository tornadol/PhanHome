using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HidoSport.Areas.Admin.Helpers
{
    public class LinkHelper
    {
        private static string UrlDuoSlash = "//";
        public static string GetBase()
        {
            //HttpContext.Current.Request.Url.Scheme + Uri.SchemeDelimiter
            return UrlDuoSlash + HttpContext.Current.Request.Url.Host +
                   (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port);
        }
        public static string GetBase(string slug)
        {
            try
            {
                if (slug.Contains(GetBase()))
                {
                    return slug.ToLower();
                }
                var url = (HttpContext.Current.Request.Url.IsDefaultPort ? "" : ":" + HttpContext.Current.Request.Url.Port) + "/" + slug;
                url = url.Replace("//", "/");
                url = UrlDuoSlash + HttpContext.Current.Request.Url.Host + url;
                return url.ToLower();
            }
            catch (NullReferenceException e)
            {
                Console.Write(e);
                return GetBase();
            }
        }
        public static string ImplodeString(List<string> source, string glue)
        {
            if (source.Count == 0)
                return "";
            return string.Join(glue, source.ToArray()); ;
        }
    }
}