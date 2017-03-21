using System.Web.Mvc;

namespace HidoSport.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
           
            context.MapRoute(
                name: "AdminLogin",
                url: "administrator",
                defaults: new { controller = "Home", action = "Index" }
            );
            context.MapRoute(
             "Ajax",
             "Item/upload",
             new { controller = "Item", action = "upload"}
            );
            context.MapRoute(
             "New",
             "New/Save",
             new { controller = "New", action = "Save" }
            );
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "HidoSport.Areas.Admin.Controllers" }
            );
        }
    }
}