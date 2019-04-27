using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Concrete;
using Ewu.Domain.Db;
using Ewu.Domain.Entities;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Infrastructure.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Ewu.WebUI.Controllers
{
    public class DealController : Controller
    {
        private ITreasuresRepository repository;
        private IAuthProvider authProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="treasuresRepository"></param>
        /// <param name="auth"></param>
        public DealController(ITreasuresRepository treasuresRepository,IAuthProvider auth)
        {
            repository = treasuresRepository;
            authProvider = auth;
        }

        /// <summary>
        /// 创建交易
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateDeal(string TreasureUID = "")
        {
            if (!string.IsNullOrEmpty(TreasureUID))
            {

            }
            return View();
        }



        /// <summary>
        /// 生成交易记录
        /// </summary>
        /// <param name="TreasureSponsorID">发起人物品UID</param>
        /// <param name="TreasureRecipientID">接收人物品UID</param>
        /// <returns></returns>
        public ActionResult MakeDeal(string TreasureSponsorID,string TreasureRecipientID)
        {
            //发起人id-当前登录人
            string TraderSponsorID = CurrentUser.Id;
            //接收人id-从物品获取
            string TraderRecipientID = repository.Treasures
                                                .Where(t => t.TreasureUID == Guid.Parse(TreasureRecipientID))
                                                .FirstOrDefault().HolderID;
            Deal deal = new Deal();

            #region 数据初始化
            deal.DealBeginTime = DateTime.Now;
            deal.DealStatus = "发起";
            deal.DLogUID = new Guid();
            deal.TraderRecipientID = TraderRecipientID;
            deal.TraderSponsorID = TraderSponsorID;
            deal.TreasureRecipientID = TreasureRecipientID;
            deal.TreasureSponsorID = TreasureSponsorID;
            #endregion

            //插入数据库
            using (var db = new LogDealDataContext())
            {
                LogDeal logDeal = new LogDeal
                {
                    DealBeginTime = deal.DealBeginTime,
                    DealStatus = deal.DealStatus,
                    DLogUID = deal.DLogUID,
                    TraderRecipientID = deal.TraderRecipientID,
                    TraderSponsorID = deal.TraderSponsorID,
                    TreasureRecipientID = deal.TreasureRecipientID,
                    TreasureSponsorID = deal.TreasureSponsorID
                };
                db.LogDeal.InsertOnSubmit(logDeal);
                //保存操作
                db.SubmitChanges();
            }
            return View();
        }

        // GET: Deal
        public ActionResult Index()
        {
            return View();
        }



        /// <summary>
        /// 获取当前用户
        /// </summary>
        private AppUser CurrentUser
        {
            get
            {
                var q = UserManager.FindById("");
                return UserManager.FindByName(HttpContext.User.Identity.Name);
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