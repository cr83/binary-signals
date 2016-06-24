using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Data.Entity;

using EasyBot.Streaming;

using log4net;

namespace EasyBot
{
    public class MvcApplication : System.Web.HttpApplication
    {
        ILog _log = LogManager.GetLogger(typeof(MvcApplication));

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Session["init"] = 0; // To set static sessionId
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            Streamer.Remove();
        }

        protected void Application_End()
        {
            Streamer.RemoveAll();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            _log.Error(exception.Message, exception);
            //Server.ClearError();
        }
    }
}
