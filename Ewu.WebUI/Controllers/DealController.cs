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
using Ewu.WebUI.Models.ViewModel;
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
        [Authorize]
        public ActionResult CreateDeal(string TreasureUID = "")
        {
            //根据id生成对象的GUID
            Guid TreasureGuid = Guid.Parse(TreasureUID);

            if (!string.IsNullOrEmpty(TreasureUID))
            {
                //获取当前选择物品的所属人
                string holderid= repository.Treasures
                                        .Where(t => t.TreasureUID == TreasureGuid)
                                        .FirstOrDefault().HolderID;

                if (!string.IsNullOrEmpty(holderid))
                {
                    var userTrea = repository.Treasures.Where(t => t.HolderID == CurrentUser.Id);

                    //生成对应视图模型
                    DealInfo dealInfo = new DealInfo
                    {
                        //当前物品的所属人对象
                        Holder = UserManager.FindById(holderid),
                        //当前物品对象
                        DealTreasure = repository.Treasures.FirstOrDefault(),
                        //交易-当前登录用户物品集合-模型
                        DealMyTreasureModel = new DealMyTreasureModel
                        {
                            DealTreasureGuid = TreasureGuid,
                            UserTreasures = userTrea
                        }
                    };
                    return View(dealInfo);
                }
            }
            return View();
        }



        /// <summary>
        /// 生成交易记录
        /// </summary>
        /// <param name="TreasureSponsorID">发起人物品UID</param>
        /// <param name="TreasureRecipientID">接收人物品UID</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult MakeDeal(string TreasureSponsorID, string TreasureRecipientID)
        {
            if (string.IsNullOrEmpty(TreasureSponsorID) || string.IsNullOrEmpty(TreasureRecipientID))
            {
                return View();
            }
            //当前登录用户UID
            string CurId = CurrentUser.Id;

            //这里验证当前发起人的物品是当前用户的，并且验证接收人的物品不是当前用户的
            var treaS = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(TreasureSponsorID)).FirstOrDefault();
            var treaR = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(TreasureRecipientID)).FirstOrDefault();
            if (treaR != null & treaS != null)
            {
                //确保发起物品是登录用户的，接受物品不是当前用户的
                if(treaS.HolderID == CurId && treaR.HolderID != CurId)
                {
                    //发起人id-当前登录人
                    string TraderSponsorID = CurId;
                    //接收人id-从物品获取
                    string TraderRecipientID = repository.Treasures
                                                        .Where(t => t.TreasureUID == Guid.Parse(TreasureRecipientID))
                                                        .FirstOrDefault().HolderID;
                    DealLogCreate dealLog = new DealLogCreate();

                    #region 生成视图模型
                    dealLog.DealInTreasure = treaR;
                    dealLog.DealOutTreasure = treaS;
                    dealLog.QQ = (string.IsNullOrEmpty(CurrentUser.OICQ)) ? "TA没有完善该信息" : CurrentUser.OICQ;
                    dealLog.WeChat = (string.IsNullOrEmpty(CurrentUser.WeChat)) ? "TA没有完善该信息" : CurrentUser.WeChat;
                    dealLog.Email = CurrentUser.Email;
                    dealLog.PhoneNum = CurrentUser.PhoneNumber;
                    #endregion

                    return View(dealLog);
                }
            }
            return View("Error");
        }

        /// <summary>
        /// 生成交易记录[HttpPost]
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult MakeDeal(DealLogCreate dealLogCreate)
        {
            //验证不为空
            if (string.IsNullOrEmpty(dealLogCreate.DealInTreasure.TreasureUID.ToString()) || string.IsNullOrEmpty(dealLogCreate.DealOutTreasure.TreasureUID.ToString()))
            {
                return View("Error");
            }
            else
            {
                //插入数据库
                using (var db = new LogDealDataContext())
                {
                    LogDeal logDeal = new LogDeal
                    {
                        DealBeginTime = DateTime.Now,
                        DealStatus = "发起",
                        DLogUID = Guid.NewGuid(),
                        //备注-发起人对接收人
                        RemarkSToR = dealLogCreate.Remark,
                        //交易接收人ID
                        TraderRecipientID = dealLogCreate.DealInTreasure.HolderID,
                        //交易发起人ID
                        TraderSponsorID = dealLogCreate.DealOutTreasure.HolderID,
                        //交易给出物品ID
                        TreasureSponsorID = dealLogCreate.DealOutTreasure.TreasureUID.ToString(),
                        //交易接受物品ID
                        TreasureRecipientID = dealLogCreate.DealInTreasure.TreasureUID.ToString()
                    };
                    try
                    {
                        db.LogDeal.InsertOnSubmit(logDeal);
                        //保存操作
                        db.SubmitChanges();
                    }
                    catch(Exception ex)
                    {
                        return View("Error", ex.Message);
                    }
                }
                return RedirectToAction("MyDeal", "Account");
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