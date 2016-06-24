using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using log4net;

namespace EasyBot.Attributes
{
    public class CustomHandleErrorAttribute : HandleErrorAttribute
    {
        ILog _log = LogManager.GetLogger(typeof(CustomHandleErrorAttribute));

        public override void OnException(ExceptionContext filterContext)
        {
            _log.Error(filterContext.Exception.Message, filterContext.Exception);
            filterContext.ExceptionHandled = true;

            ViewDataDictionary viewData = new ViewDataDictionary();
#if DEBUG
            viewData.Add(new KeyValuePair<string, object>("errorMessage", filterContext.Exception.Message));
#endif

            filterContext.Result = new ViewResult
            {
                ViewData = viewData,
                ViewName = MVC.Shared.Views.Error
            };
            //base.OnException(filterContext);
        }
    }
}