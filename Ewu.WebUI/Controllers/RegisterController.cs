using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult GetCode()
        {
            string code = Request["Code"];
            string phoNum = Request["phoNum"];

            //短信应用SDK AppID
            int appid = 1400187647;

            //短信应用SDK AppKey
            string appkey = "225561ebc612eb400a62819edd1f192e";

            //需要发送短信的手机号码
            string[] phoneNumbers = { phoNum };

            //短信模板ID，需要在短信应用中申请
            int templateId = 281441;

            //签名
            string smsSign = "郑兴豪个人开发测试";

            try
            {
                SmsSingleSender ssender = new SmsSingleSender(appid, appkey);
                var info = ssender.sendWithParam("86", phoneNumbers[0], templateId, new[] { code, "5" }, smsSign, "", "");
                int result = info.result;
                string errMsg = info.errMsg;
                return Json(info + "||" + errMsg, JsonRequestBehavior.AllowGet);

            }
            catch (JSONException e)
            {
                ViewBag.Result = e;
            }
            catch (HTTPException e)
            {
                ViewBag.Result = e;
            }
            catch (Exception e)
            {
                ViewBag.Result = e;
            }
            return View("Index");
        }
    }
}