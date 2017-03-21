using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess;
using StackExchange.Profiling;
using System.Configuration;
using HidoSport.Helpers;
using System.IO;
using System.Web.UI;
using System.Text;

namespace HidoSport.Controllers
{
    public class BaseController : Controller
    {
        public PhanHomeEntities ctx = new PhanHomeEntities();
        public MiniProfiler Profiler { get; set; }
        // GET: Base
        public BaseController() : base()
        {
            ViewBag.domain = ConfigurationManager.AppSettings["cdn_domain"];
            Profiler = MiniProfiler.Current;
        }
        /// <summary>
        /// Render a web control to string
        /// </summary>
        /// <param name="control">Control name</param>
        /// <returns></returns>
        public string RenderControl(Control control)
        {
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            HtmlTextWriter writer = new HtmlTextWriter(sw);

            control.RenderControl(writer);
            return sb.ToString();
        }

        /// <summary>
        /// Render a partial view to string
        /// </summary>
        /// <param name="viewName">View name</param>
        /// <param name="model"></param>
        /// <param name="viewData"></param>
        /// <returns></returns>
        public string RenderPartialViewToString(string viewName, object model = null, ViewDataDictionary viewData = null)
        {
            try
            {
                // first find the ViewEngine for this view
                var viewEngineResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);

                if (viewEngineResult == null)
                    throw new FileNotFoundException("View cannot be found.");

                // get the view and attach the model to view data
                var view = viewEngineResult.View;
                ViewData.Model = model;
                if (viewData != null)
                {
                    ViewData.Clear();
                    foreach (var vd in viewData)
                    {
                        ViewData.Add(vd.Key, vd.Value);
                    }
                }

                string result;

                using (var sw = new StringWriter())
                {
                    var ctx = new ViewContext(ControllerContext, view, ViewData, TempData, sw);
                    view.Render(ctx, sw);
                    result = sw.ToString();
                }

                return result;
            }
            catch (Exception ex)
            {
#if DEBUG
                throw ex;
#else
                return null;
#endif
            }
        }
    }
}