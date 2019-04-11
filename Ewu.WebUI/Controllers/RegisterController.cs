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

        public ActionResult validCode()
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


    }
}