using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using StackExchange.Profiling;
using System.Diagnostics;
using System.IO.Compression;
using System.Globalization;

namespace HidoSport
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Settings.MaxJsonResponseSize = int.MaxValue;
                MiniProfiler.Start();
            }
            //HttpApplication app = (HttpApplication)sender;
            //string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            //System.IO.Stream prevUncompressedStream = app.Response.Filter;

            //if (acceptEncoding == null || acceptEncoding.Length == 0)
            //    return;

            //acceptEncoding = acceptEncoding.ToLower();

            //if (acceptEncoding.Contains("gzip"))
            //{
            //    // gzip
            //    app.Response.Filter = new System.IO.Compression.GZipStream(prevUncompressedStream,
            //        System.IO.Compression.CompressionMode.Compress);
            //    app.Response.AppendHeader("Content-Encoding",
            //        "gzip");
            //}
            //else if (acceptEncoding.Contains("deflate"))
            //{
            //    // defalte
            //    app.Response.Filter = new System.IO.Compression.DeflateStream(prevUncompressedStream,
            //        System.IO.Compression.CompressionMode.Compress);
            //    app.Response.AppendHeader("Content-Encoding",
            //        "deflate");
            //}
        }
        protected void Application_EndRequest()
        {
            if (Request.IsLocal)
            {
                MiniProfiler.Stop();
            }
        }
        protected void Application_PreSendRequestHeaders(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;

            if (app?.Context != null)
            {
                HttpResponse response = app.Response;
                response.Headers.Add("Server-Name", HttpContext.Current.Server.MachineName);
                response.Headers.Add("Server-PID", Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture));

                if (response.Filter is GZipStream && response.Headers["Content-Encoding"] != "gzip")
                {
                    response.Headers.Remove("Content-Encoding");
                    response.AppendHeader("Content-Encoding", "gzip");
                }
                else if (response.Filter is DeflateStream && response.Headers["Content-Encoding"] != "deflate")
                {
                    response.Headers.Remove("Content-Encoding");
                    response.AppendHeader("Content-Encoding", "deflate");
                }
            }
        }
    }
}
