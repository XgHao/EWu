using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Threading.Tasks;
using Ewu.WebUI.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ewu.Domain.Entities;
using Ewu.WebUI.Models;

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

        /// <summary>
        /// 用户信息页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UserProps()
        {
            return View(CurrentUser);
        }

        /// <summary>
        /// 用户信息动作[HttpPost]
        /// </summary>
        /// <param name="model">更改用户信息模型</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> UserProps(ChangeInfoViewModel model)
        {
            AppUser user = CurrentUser;
            user.Gender = model.Gender;
            user.Signature = model.Signature;
            user.RealName = model.RealName;
            user.Age = model.Age;
            user.IDCardNO = model.IDCardNO;
            await UserManager.UpdateAsync(user);
            return View(user);
        }

        /// <summary>
        /// 获取当前用户
        /// </summary>
        private AppUser CurrentUser
        {
            get
            {
                return UserManager.FindByName(HttpContext.User.Identity.Name);
            }
        }

        /// <summary>
        /// 获取当前用户管理器
        /// </summary>
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}