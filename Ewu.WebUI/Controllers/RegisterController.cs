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
        /// 获取验证码并发送
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCode()
        {
            string code = Request["Code"];
            string phoNum = Request["phoNum"];
            //发送短信
            string result = new Identity().MobileMsg(code, phoNum);
            if (result == "OK")
            {
                //保存验证码
                Session.Add("Code", code);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}