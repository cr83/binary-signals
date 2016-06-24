using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyBot.Controllers
{
    public partial class ChartController : BaseController
    {
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}