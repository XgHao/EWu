using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.WebUI.API;

//腾讯云短信
using qcloudsms_csharp;
using qcloudsms_csharp.json;
using qcloudsms_csharp.httpclient;
using Newtonsoft.Json.Linq;
using Ewu.WebUI.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取手机验证码并发送
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPhoCode()
        {
            string code = Request["Code"];
            string phoNum = Request["phoNum"];
            //发送短信
            string result = new Identity().MobileMsg(code, phoNum);
            if (result == "OK")
            {
                //保存验证码
                Session.Add("PhoCode", code);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取邮箱验证码并发送
        /// </summary>
        /// <returns></returns>
        public ActionResult GetEmailCode()
        {
            string code = Request["Code"];
            string email = Request["Email"];
            //验证邮箱
            Dictionary<string, string> result = new Identity().ValidEmail(email);
            if (result["Status"] == "200")
            {
                //格式有效，发送邮件
                new Identity().SendMail(email, code);
                Session.Add("EmailCode", code);
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            else
            {
                //返回错误信息
                return Json(result["Msg"], JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 验证手机号和邮箱
        /// </summary>
        /// <returns></returns>
        public JsonResult validCode()
        {
            string code = Request["Code"];
            string type = Request["Type"];
            string result = string.Empty;



            //手机验证
            if (type.Equals("Pho"))
            {
                //Session验证码已存在
                if (Session["PhoCode"] != null)
                {
                    result = Session["PhoCode"].ToString() == code ? "OK" : "Fail";
                }
                else
                {
                    result = "Error";
                }
            }
            //邮箱验证
            else if(type.Equals("Email")){
                //Session验证码已存在
                if (Session["EmailCode"] != null)
                {
                    result = Session["EmailCode"].ToString() == code ? "OK" : "Fail";
                }
                else
                {
                    result = "Error";
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 检查当前电子邮件是否已存在
        /// </summary>
        /// <returns></returns>
        public JsonResult isExistEmail()
        {
            string email = Request["Eamil"];
            AppUser appUser = UserManager.FindByEmail(email);
            //该邮箱已存在(即appUser不为空)返回NO，否则返回YES
            string result = appUser != null ? "NO" : "YES";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 检查当前用户名是否已存在
        /// </summary>
        /// <returns></returns>
        public JsonResult isExistUserName()
        {
            string userName = Request["Name"];
            AppUser appUser = UserManager.FindByName(userName);
            //该用户名已存在(即appUser不为空)返回YES，否则返回NO
            string result = appUser != null ? "YES" : "NO";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 验证当前账号信息
        /// </summary>
        /// <returns></returns>
        public JsonResult ValidCreateUser()
        {
            string userName = Request["Name"];
            string passWord = Request["PassWD"];
            string email = Request["Email"];
            string returnRes = string.Empty;

            AppUser user = new AppUser
            {
                UserName = userName,
                Email = email,
                BirthDay = DateTime.Now,
                RegisterTime = DateTime.Now,
            };

            //邮箱不为空
            if (email != "")
            {
                try
                {
                    //尝试创建用户，并返回错误结果
                    IdentityResult result = UserManager.Create(user, passWord);
                    //如果可以创建，则删除
                    if (result.Succeeded)
                    {
                        UserManager.Delete(user);
                        returnRes = "OK";
                    }
                    //返回错误集合
                    else
                    {
                        //遍历所有错误
                        foreach (string error in result.Errors)
                        {
                            returnRes += error;
                        }
                    }
                    return Json(returnRes, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
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