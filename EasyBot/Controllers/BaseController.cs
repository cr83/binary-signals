using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EasyBot.Attributes;

namespace EasyBot.Controllers
{
    [CustomHandleError]
    public partial class BaseController : Controller
    {
    }
}