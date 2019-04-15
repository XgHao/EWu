using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.WebUI.Models;
using Ewu.Domain.Entities;
using System.Threading.Tasks;
using System.Globalization;
using Ewu.WebUI.API;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 用于集中化的用户管理工具
    /// </summary>
    //[Authorize(Roles = "Admin")]
    public class IAdminController : Controller
    {
        // GET: IAdmin
        public ActionResult Index()
        {
            return View(UserManager.Users);
        }

        /// <summary>
        /// 删除用户[HttpPost]
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            //根据用户id获取用户对象
            AppUser user = await UserManager.FindByIdAsync(id);
            //用户存在
            if (user != null)
            {
                //删除用户，返回结果
                IdentityResult result = await UserManager.DeleteAsync(user);
                //删除成功
                if (result.Succeeded)
                {
                    //重定向到Index
                    return RedirectToAction("Index");
                }
                //删除失败
                else
                {
                    //重定向到Error页面并传递错误集合
                    return View("Error", result.Errors);
                }
            }
            //用户不存在
            else
            {
                return View("Error", new string[] { "没有找到该用户" });
            }
        }

        /// <summary>
        /// 编辑用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            //根据用户ID获取操作的对象
            AppUser user = await UserManager.FindByIdAsync(id);
            //如果对象存在
            if(user != null)
            {
                //返回当前对象
                return View(user);
            }
            //不存在
            else
            {
                //重定向到Index
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 编辑动作[HttpPost]
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="email">用户邮箱</param>
        /// <param name="passwd">用户密码</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string passwd)
        {
            //根据用户ID获取操作对象
            AppUser user = await UserManager.FindByIdAsync(id);
            //用户存在
            if (user != null)
            {
                //设置邮箱
                user.Email = email;
                //验证邮箱，并返回结果
                IdentityResult validEmail = await UserManager.UserValidator.ValidateAsync(user);
                //验证失败
                if (!validEmail.Succeeded)
                {
                    //添加错误模型
                    AddErrorsFromResult(validEmail);
                }

                //验证密码，并返回结果
                IdentityResult validPass = null;
                //密码不为空
                if (passwd != string.Empty)
                {
                    //验证密码，并返回结果
                    validPass = await UserManager.PasswordValidator.ValidateAsync(passwd);
                    //验证成功
                    if (validPass.Succeeded)
                    {
                        //保存密码的HASH值
                        user.PasswordHash = UserManager.PasswordHasher.HashPassword(passwd);
                    }
                    //验证失败
                    else
                    {
                        //添加错误模型
                        AddErrorsFromResult(validPass);
                    }
                }
                //1.当邮箱验证成功，密码为空即未填写
                //2.或邮箱验证成功，密码填写且验证通过
                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && passwd != string.Empty && validPass.Succeeded))
                {
                    //更新用户信息，并返回结果
                    IdentityResult result = await UserManager.UpdateAsync(user);
                    //更新成功
                    if (result.Succeeded)
                    {
                        //重定向到Index
                        return RedirectToAction("Index");
                    }
                    //更新失败
                    else
                    {
                        //添加错误模型
                        AddErrorsFromResult(result);
                    }
                }
            }
            //用户不存在
            else
            {
                ModelState.AddModelError("", "用户没有找到");
            }
            return View(user);
        }

        /// <summary>
        /// 添加验证模型的错误集合
        /// </summary>
        /// <param name="result">身份操作的结果</param>
        private void AddErrorsFromResult(IdentityResult result)
        {
            //遍历所有错误
            foreach(string error in result.Errors)
            {
                //添加错误到错误模型
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// 因为在实现不用的管理功能时，会反复使用APpUserManager类。所以定义UserManager以方便
        /// </summary>
        private AppUserManager UserManager
        {
            get
            {
                //Microsoft.Owin.Host.SystemWeb程序集为HttpContext类添加了一些扩展方法，其中之一便是GetOwinContext
                //GetOwinContext通过IOwinContext对象，将基于请求的上下文对象提供给OWIN API
                //在这其中有一个扩展方法GetUserManager<T>，可以用来得到用户管理器类实例
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}