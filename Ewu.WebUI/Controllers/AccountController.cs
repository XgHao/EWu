using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Models;

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
    }
}