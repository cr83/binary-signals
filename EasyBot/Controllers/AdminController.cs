using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity.Owin;

namespace EasyBot.Controllers
{
    public partial class AdminController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //[Authorize(Roles = Common.Constants.Security.Roles.Admin)]
        public virtual ActionResult Index()
        {
            return View();
        }

        //[Authorize(Roles = Common.Constants.Security.Roles.Admin)]
        public virtual JsonResult GetUsers(int rows, int page)
        {
            //_search=false&nd=1451918706892&rows=10&page=1&sidx=&sord=asc
            return new JsonResult()
            {
                Data = UserManager.Users
                    .Skip(rows*(page - 1))
                    .Take(rows)
                    .Select(u => new { u.Id, u.UserName, u.EmailConfirmed, u.CreatedOn, u.IsTrial, u.PayedBefore, Role = UserManager.GetRolesAsync(u.Id).Result[0] }),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
    }
}