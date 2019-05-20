using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.WebUI.API;
using Ewu.WebUI.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ewu.Domain.Entities;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Globalization;
using Ewu.WebUI.Models.ViewModel;
using Ewu.Domain.Db;



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
        /// 创建新用户页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 创建新用户动作[HttpPost]
        /// </summary>
        /// <param name="model">目标用户验证模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model, HttpPostedFileBase idcardImg = null)
        {
            //验证模型无误
            if(ModelState.IsValid)
            {
                //检查有无上传图片
                if (idcardImg != null)
                {
                    //文件MimeType
                    model.IDCardImageMimeType = idcardImg.ContentType;
                    
                    //文件数据
                    model.IDCardImageData = new byte[idcardImg.ContentLength];

                    //数据以二进制的形势写入到流中
                    idcardImg.InputStream.Read(model.IDCardImageData, 0, idcardImg.ContentLength);

                    //转换base64格式
                    string base64 = Convert.ToBase64String(model.IDCardImageData);

                    //使用API获取身份证信息
                    Dictionary<string, string> info = new Identity().IdentityORC(base64);

                    //识别成功
                    if (info["Status"] == "SUCCESS")
                    {
                        //识别成功后，检查改身份证是否已经使用
                        using (var db = new AspNetUserDataContext())
                        {
                            var idcard = db.AspNetUsers.Where(x => x.IDCardNO.ToString() == info["num"]).ToList();
                            
                            //该身份证已被注册
                            if (idcard.Count > 0)
                            {
                                model.OCRresult = "该身份证已被注册";
                                return View("ReUpLoadIdCard", model);
                            }
                        }

                        DateTime birth;
                        //转换时间
                        if (DateTime.TryParseExact(info["birth"], "yyyyMMdd", null, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal, out birth))
                        {
                            model.BirthDay = birth;
                            model.Age = DateTime.Now.Year - birth.Year;
                        }
                        model.NativePlace = info["address"];
                        model.RealName = info["name"];
                        model.IDCardNO = info["num"];
                        model.Gender = info["sex"];

                        //根据模型生成对应的用户实例
                        AppUser user = new AppUser
                        {
                            UserName = model.Name,                              //用户名
                            Email = model.Email,                                //电子邮箱
                            Age = model.Age,                                    //年龄
                            BirthDay = model.BirthDay,                          //出生年月
                            RegisterTime = DateTime.Now,                        //注册时间
                            Gender = model.Gender,                              //性别
                            HeadPortrait = @"\images\usr_avatar.png",          //默认头像
                            IDCardImageData = model.IDCardImageData,            //身份证照
                            IDCardImageMimeType = model.IDCardImageMimeType,    //身份证照格式
                            IDCardNO = model.IDCardNO,                          //身份证号码
                            NativePlace = model.NativePlace,                    //家庭住址
                            RealName = model.RealName,                          //真实姓名
                            PhoneNumber = model.PhoneNumber,                    //手机号码
                            EmailConfirmed = true,                              //电子邮箱验证是否通过
                            PhoneNumberConfirmed = true,                        //手机号码验证是否通过
                            Signature = model.Gender == "男" ? "他什么也没留下" : "她什么也没留下"     //初始化个性签名
                        };


                        //创建用户，并返回结果
                        IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                        //成功
                        if (result.Succeeded)
                        {
                            model.OCRresult = "注册成功";
                            //页面重定向到注册页面
                            return View("ReUpLoadIdCard", model);
                        }
                        //失败
                        else
                        {
                            //添加错误模型
                            AddErrorsFromResult(result);
                            //返回Error页面
                            return View();
                        }
                    }
                    //识别失败，跳转页面重新上传身份证
                    else if (info["Status"] == "ERROR")
                    {
                        model.OCRresult = "识别失败";
                        return View("ReUpLoadIdCard", model);
                    }
                }
                //无图片
                else
                {
                    model.OCRresult = "图片为空，请下方重新上传";
                    return View("ReUpLoadIdCard", model);
                }
            }
            return View(model);
        }

        /// <summary>
        /// 身份证识别失败，重新上传
        /// </summary>
        /// <returns></returns>
        public ActionResult ReUpLoadIdCard(CreateModel model)
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
            string email = Request["Email"];
            using(var db = new AspNetUserDataContext())
            {
                var appUser = db.AspNetUsers.Where(a => a.Email == email).FirstOrDefault();
                //该邮箱已存在(即appUser不为空)返回YES，否则返回NO
                string result = appUser != null ? "YES" : "NO";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// 检查当前手机号码是否已存在
        /// </summary>
        /// <returns></returns>
        public JsonResult isExistPhoNum()
        {
            string PhoNum = Request["PhoNum"];
            using (var db = new AspNetUserDataContext())
            {
                var appUser = db.AspNetUsers.Where(a => a.PhoneNumber == PhoNum).FirstOrDefault();
                //该邮箱已存在(即appUser不为空)返回YES，否则返回NO
                string result = appUser != null ? "YES" : "NO";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// 检查当前用户名是否已存在
        /// </summary>
        /// <returns></returns>
        public JsonResult isExistUserName()
        {
            string userName = Request["Name"];
            string result = "Error";

            //检查长度
            if (userName.Length >= 3 && userName.Length <= 20)
            {
                //检查是否只含数字和字母
                string pattern = @"^[A-Za-z0-9]+$";
                Regex regex = new Regex(pattern);
                
                //验证通过
                if (regex.IsMatch(userName))
                {
                    AppUser appUser = UserManager.FindByName(userName);
                    //该用户名已存在(即appUser不为空)返回YES，否则返回NO
                    result = appUser != null ? "YES" : "NO";
                }
                else
                {
                    result = "用户名只能包含数字和字母";
                }
            }
            else
            {
                result = "用户名必须在3-20个字符";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 验证当前账号信息
        /// </summary>
        /// <returns></returns>
        public JsonResult ValidCreateUser()
        {
            string userName = Request["Name"] != "" ? Request["Name"] : "00";
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
                        var ErrorList = result.Errors.FirstOrDefault().Split('。');

                        //遍历所有错误
                        foreach (string error in ErrorList)
                        {
                            if (error.Contains("密码"))
                            {
                                returnRes = error;
                                break;
                            }
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
        /// 添加验证模型的错误集合
        /// </summary>
        /// <param name="result">身份操作的结果</param>
        private void AddErrorsFromResult(IdentityResult result)
        {
            //遍历所有错误
            foreach (string error in result.Errors)
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