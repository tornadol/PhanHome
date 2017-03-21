using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HidoSport
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name:"Ajaxjj",
                url:"aj/{controller}/{action}/{*q}"
                );
            routes.MapRoute(
                name: "Request",
                url: "gio-hang",
                defaults: new { controller = "Cart", action = "Checkout" },
                namespaces: new[] { "HidoSport.Controllers" }
            );
            routes.MapRoute(
                name: "Pay",
                url: "thanh-toan",
                defaults: new { controller = "Cart", action = "Pay" },
                namespaces: new[] { "HidoSport.Controllers" }
            );
            routes.MapRoute(
              name:"blog",
              url:"tin-tuc",
              defaults:new { controller = "Blog", action = "Index" },
              namespaces:new[] { "HidoSport.Controllers" }
          );
            routes.MapRoute(
                name: "Introduction",
                url: "gioi-thieu",
                defaults: new { controller = "Default", action = "Intro" },
                namespaces: new[] { "HidoSport.Controllers" }
            );
            routes.MapRoute(
                name: "Dat hang",
                url: "dat-hang-thanh-cong",
                defaults: new { controller = "Cart", action = "PaySuccess" },
                namespaces: new[] { "HidoSport.Controllers" }
            );
            routes.MapRoute(
               "Detail",
               "san-pham/{productcode}",
                new { controller = "Detail", action = "Index" },
                new { productcode = @"^(\w|-|\d)+$" }
           );
           
            routes.MapRoute(
               "CateChild",
               "{catecode}/{catechildcode}",
                new { controller = "Cate", action = "Index" },
                new { catecode = @"^(\w|-|\d)+$", catechildcode = @"^(\w|-|\d)+$" },
                new[] { "HidoSport.Controllers" }
           );
            routes.MapRoute(
               "Cate",
               "{catecode}",
                new { controller = "Cate", action = "Index" },
                new { catecode = @"^(\w|-|\d)+$" }
           );
            
            routes.MapRoute(
               name: "Home",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "HidoSport.Controllers" }
           );
        }
    }
}
