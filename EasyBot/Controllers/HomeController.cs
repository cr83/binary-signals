using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EasyBot.Controllers
{
    public partial class HomeController : BaseController
    {
        [AllowAnonymous]
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}