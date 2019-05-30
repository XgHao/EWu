using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.WebUI.API;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.Domain.Db;
using Ewu.WebUI.Models.ViewModel;
using Ewu.WebUI.Infrastructure.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 管理员控制器
    /// </summary>
    [Authorize(Roles = "Admin")]     //启动验证
    public class AdminController : Controller
    {
        //物品存储库
        private ITreasuresRepository repository;

        //构造函数，传递存储库
        public AdminController(ITreasuresRepository repo)
        {
            repository = repo;
        }

        /// <summary>
        /// 物品管理
        /// </summary>
        /// <returns></returns>
        public ActionResult AllTreasure()
        {
            //生成视图模型
            List<AllTreasureViewModel> treasureholders = new List<AllTreasureViewModel>();

            foreach(var trea in repository.Treasures)
            {
                using(var db = new AspNetUserDataContext())
                {
                    var user = db.AspNetUsers.Where(u => u.Id == trea.HolderID).FirstOrDefault();
                    if (user != null)
                    {
                        treasureholders.Add(new AllTreasureViewModel
                        {
                            TreasureInfo = trea,
                            holderInfo = new BasicUserInfo
                            {
                                UserID = user.Id,
                                UserName = user.UserName,
                                RealName = user.RealName
                            }
                        });
                    }
                }
            }

            return View(treasureholders.AsEnumerable());
        }

        /// <summary>
        /// 用户管理
        /// </summary>
        /// <returns></returns>
        public ActionResult AllUser(string UserID = "")
        {
            //生成视图模型
            List<AllUserViewModel> allUsers = new List<AllUserViewModel>();

            //获取所有用户
            using(var db = new AspNetUserDataContext())
            {
                var users = db.AspNetUsers.Where(u => u.UserName != "XgHao");
                foreach(var user in users)
                {
                    allUsers.Add(new AllUserViewModel
                    {
                        isChoose = user.Id == UserID ? true : false,
                        userInfo = new BasicUserInfo
                        {
                            UserID = user.Id,
                            UserName = user.UserName,
                            RealName = user.RealName,
                            BirthDay = user.BirthDay.ToString("yyyy-MM-dd"),
                            RegisterTime = user.RegisterTime,
                            Gender = user.Gender
                        }
                    });
                }
            }

            return View(allUsers.AsEnumerable());
        }

        /// <summary>
        /// 下架物品[HttpPost]
        /// </summary>
        /// <param name="treasureUID">操作对象的GUID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid treasureUID)
        {
            var trea = repository.Treasures.Where(t => t.TreasureUID == treasureUID).FirstOrDefault();

            //首先要判断该物品有否有正在进行的交易
            using (var db = new LogDealDataContext())
            {
                var logs = db.LogDeal.Where(l => l.TreasureSponsorID == treasureUID.ToString() || l.TreasureRecipientID == treasureUID.ToString());
                //如果有正在交易的记录,不能删除物品等待交易完成
                foreach(var log in logs)
                {
                    if (log.DealStatus.Contains("交易中"))
                    {
                        TempData["error"] = string.Format("{0} 有正在进行的交易，目前无法下架", trea.TreasureName);
                        return RedirectToAction("AllTreasure");
                    }
                    else if (log.DealStatus.Contains("待确认"))
                    {
                        log.DealStatus = "交易取消";
                        log.DealEndTime = DateTime.Now;
                        db.SubmitChanges();
                        
                    }
                }
            }

            //根据GUID获取对象
            Treasure deletedTreasure = repository.DeleteTreasure(treasureUID);

            //发送信息
            new Identity().AddNotice(trea.HolderID, CurrentUser.Id, "警告", string.Format("{0}有违规信息，已被管理员强制下架。", trea.TreasureName), treasureUID.ToString());

            //对象不为空
            if (deletedTreasure != null)
            {
                TempData["message"] = string.Format("{0} 已经强制下架", deletedTreasure.TreasureName);
            }

            //重定向到AllTreasure
            return RedirectToAction("AllTreasure");
        }

        /// <summary>
        /// 封禁帐号
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Block(string UserID)
        {
            //获取用户对象
            using(var db = new AspNetUserDataContext())
            {
                var user = db.AspNetUsers.Where(u => u.Id == UserID).FirstOrDefault();
                if (user != null)
                {
                    //更改用户信息
                    user.PasswordHash = UserID + DateTime.Now;
                    user.HeadPortrait = @"\images\usr_avatar.png";
                    user.UserName = "账户已注销";
                    user.RealName = "账户已注销";
                    db.SubmitChanges();
                    TempData["message"] = string.Format("用户“{0}({1})”已被注销", user.RealName, user.UserName);
                }
                else
                {
                    TempData["error"] = string.Format("用户ID:{0} 不存在", UserID);
                }
            }

            //重定向到AllUser
            return RedirectToAction("AllUser");
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