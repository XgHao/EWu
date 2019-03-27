using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace Ewu.WebUI.Controllers
{
    public class IdentityController : Controller
    {
        // GET: Identity
        [Authorize]
        public ActionResult Index()
        {
            return View(GetDatas("Index"));
        }

        /// <summary>
        /// 该方法只有管理员才能访问
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult AdminAction()
        {
            return View("Index", GetDatas("AdminAction"));
        }

        private Dictionary<string,object> GetDatas(string actionName)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("Action", actionName);
            dict.Add("User", HttpContext.User.Identity.Name);
            dict.Add("Authenticated", HttpContext.User.Identity.IsAuthenticated);
            dict.Add("Auth Type", HttpContext.User.Identity.AuthenticationType);
            dict.Add("In Users Role", HttpContext.User.IsInRole("Users"));
            return dict;
        }
    }
}