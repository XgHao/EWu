using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Entities;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.WebUI.Models.ViewModel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 用户验证控制器
    /// </summary>
    public class AccountController : Controller
    {
        //验证接口
        IAuthProvider authProvider;

        //构造函数
        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ViewResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录页面[HttpPost]
        /// </summary>
        /// <param name="model">登录的视图模型</param>
        /// <param name="returnUrl">返回的URL</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel model,string returnUrl)
        {
            //登录视图信息验证无误
            if (ModelState.IsValid)
            {
                //验证成功时
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    //重定向到ReturnUrl，如果为空则默认为Admin/Index
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                //验证失败时
                else
                {
                    //添加验证错误
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public PartialViewResult UserInfo()
        {
            if (CurrentUser != null)
            {
                return PartialView(CurrentUser);
            }
            else
            {
                CurrentUser.UserName = "请登录";
                CurrentUser.HeadPortrait = "";
                return PartialView(CurrentUser);
            }
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
        /// 获取用户管理器
        /// </summary>
        private AppUserManager UserManager
        {
            get
            {
                //Microsoft.Owin.Host.SystemWeb程序集为HttpContext类添加了一些扩展方法，其中之一便是GetOwinContext()方法
                //GetOwinContext通过IOwinContext对象，将基于请求的上下文对象提供给OWIN API
                //在这其中有一个扩展方法GetUserManager<T>，可以用来得到用户管理器类实例
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}