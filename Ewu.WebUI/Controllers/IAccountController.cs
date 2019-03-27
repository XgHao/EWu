using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Ewu.WebUI.Models;
using Ewu.Domain.Entities;
using Microsoft.Owin.Security;
using System.Security.Claims;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ewu.WebUI.Infrastructure.Identity;

namespace Ewu.WebUI.Controllers
{
    [Authorize]
    public class IAccountController : Controller
    {
        /// <summary>
        /// 登录页面
        /// </summary>
        /// <param name="returnUrl">返回的URL(当用户请求一个受限的URL时，转入到验证页面时需要传递URL，以便验证成功后能返回)</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //当前用户没有通过验证
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "拒绝访问" });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// 登录操作[HttpPost]
        /// </summary>
        /// <param name="details">页面细节</param>
        /// <param name="returnUrl">返回的URL(当用户请求一个受限的URL时，转入到验证页面时需要传递URL，以便验证成功后能返回)</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]                //将动作方法默认限制到已认证用户，但又能允许未认证用户登录到应用程序
        [ValidateAntiForgeryToken]      //与Html.AntiForgeryToken辅助器方法联合工作，防止Cross-Site-Request Forgery(CSRF,跨网站请求伪造)
        public async Task<ActionResult> Login(LoginModel details, string returnUrl)
        {
            //如果验证模型无误
            if (ModelState.IsValid)
            {
                //根据用户名和密码查找用户，返回结果
                AppUser user = await UserManager.FindAsync(details.Name, details.Password);
                //用户不存在
                if (user == null)
                {
                    ModelState.AddModelError("", "用户不存在或密码错误");
                }
                //用户存在
                else
                {
                    //创建一个标识该用户的ClaimsIdentity对象，由用户管理器(AppUserManager)的CreateIdentityAsync方法创建得到
                    ClaimsIdentity ident = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    //让已认证的Cookie失效
                    AuthManager.SignOut();              //SignOut()签出用户，意味着使标识已认证用户的Cookie失效
                    //创建一个新的Cookie
                    AuthManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false            //该属性设为TRUE表示该认证Cookie在浏览器中是持久化的，表明用户在开始新会话时不必再次验证
                    }, ident);                          //SignIn(options,identity)签入用户，意味着创建用来标识已认证请求的Cookie

                    //重定向到原来的页面
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(details);
        }

        /// <summary>
        /// 注销操作
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Logout()
        {
            AuthManager.SignOut();
            return RedirectToAction("Index", "Identity");
        }

        private IAuthenticationManager AuthManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}