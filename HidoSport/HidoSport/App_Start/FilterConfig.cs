using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HidoSport.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public class SessionExpireAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuting(ActionExecutingContext filterContext)
            {
                // check  sessions here
                if (HttpContext.Current.Session["keyValue"] == null)
                {
                    filterContext.Result = new RedirectResult("~/administrator");
                    return;
                }
                base.OnActionExecuting(filterContext);
            }
        }
    }
}