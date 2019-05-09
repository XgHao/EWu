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
                    //当前登录用户所有的物品
                    var userTrea = repository.Treasures.Where(t => t.HolderID == CurrentUser.Id);

                    //生成对应视图模型
                    DealInfo dealInfo = new DealInfo
                    {
                        //当前物品的所属人对象
                        Holder = UserManager.FindById(holderid),
                        //当前物品对象
                        DealTreasure = repository.Treasures.Where(t => t.TreasureUID == TreasureGuid).FirstOrDefault(),
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
        /// 交易记录状态：
        /// 1.待确认：发起交易阶段---订单未结束
        /// 2.拒绝：对方拒绝交易---订单已结束
        /// 3.接受：对方接受交易---订单未结束
        /// 4.交易中：双方交易阶段---订单未结束
        /// 5.交易失败：交易双方中有一方无法完成交易---订单已结束
        /// 6.交易成功：交易成功，进入评价阶段---订单已结束
        /// 7.交易取消：发起人在接受人 接受/拒绝 之前取消交易---订单已结束
        /// </summary>
        /// <param name="TreasureSponsorID">发起人物品UID</param>
        /// <param name="TreasureRecipientID">接收人物品UID</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult MakeDeal(string TreasureSponsorID, string TreasureRecipientID)
        {
            //首先要先验证该订单是不是出现过
            using(var db=new LogDealDataContext())
            {
                //获取与当前交易相同的记录
                var deallogs = db.LogDeal.Where(d => d.TreasureSponsorID == TreasureSponsorID && d.TreasureRecipientID == TreasureRecipientID);
                //遍历记录
                foreach(var dlog in deallogs)
                {
                    //当记录中出现以下哞种情况（即订单未完成），说明当前交易订单还在处理，这不允许创建订单，返回Error页面
                    if (dlog.DealStatus == "待确认" || dlog.DealStatus == "接受" || dlog.DealStatus == "交易中")
                    {
                        return View("Error");
                    }
                }
            }


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
                Guid guid = Guid.NewGuid();
                //插入数据库
                using (var db = new LogDealDataContext())
                {
                    LogDeal logDeal = new LogDeal
                    {
                        DealBeginTime = DateTime.Now,
                        DealStatus = "待确认",
                        DLogUID = guid,
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

                        //更新当前物品交易记录
                        var treasure = repository.Treasures.Where(t => t.TreasureUID == dealLogCreate.DealOutTreasure.TreasureUID).FirstOrDefault();
                        treasure.DLogUID = guid.ToString();
                        repository.SaveTreasure(treasure);
                    }
                    catch(Exception ex)
                    {
                        return View("Error", ex.Message);
                    }
                }
                return RedirectToAction("InitiateDealLog", "Account");
            }
        }


        #region 订单进行状态
        /// <summary>
        /// 修改备注信息-发起的申请
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditRemarks(string DLogUID = "")
        {
            if (string.IsNullOrEmpty(DLogUID))
            {
                return View("Error");
            }
            //获取当前交易信息
            LogDeal deal = new LogDeal();
            using(var db=new LogDealDataContext())
            {
                deal = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();
            }

            //换入物品
            var treaR = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureRecipientID))
                                .FirstOrDefault();
            //换出物品
            var treaS = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureSponsorID))
                                .FirstOrDefault();
            if (treaR != null && treaS != null)
            {
                return View(new DealLogCreate
                {
                    DealInTreasure = treaR,
                    DealOutTreasure = treaS,
                    Remark = deal.RemarkSToR,
                    DealLogID = DLogUID
                });
            }

            return View("Error");
        }

        /// <summary>
        /// 修改备注信息-发起的申请[HttpPost]
        /// </summary>
        /// <param name="dealLogCreate"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult EditRemarks(DealLogCreate dealLogCreate)
        {
            using(var db = new LogDealDataContext())
            {
                //修改备注信息
                var log = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(dealLogCreate.DealLogID)).FirstOrDefault();
                if (log != null)
                {
                    log.RemarkSToR = dealLogCreate.Remark;
                }
                db.SubmitChanges();
            }
            return RedirectToAction("InitiateDealLog", "Account");
        }

        /// <summary>
        /// 取消交易-发起的交易
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public JsonResult CancelDeal(string DealLogUID = "")
        {
            string result = "Fail";
            if (!string.IsNullOrEmpty(DealLogUID))
            {
                using(var db=new LogDealDataContext())
                {
                    var log = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(DealLogUID)).FirstOrDefault();
                    if (log != null)
                    {
                        log.DealStatus = "交易取消";
                        db.SubmitChanges();
                        result = "OK";
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 接受申请
        /// </summary>
        /// <returns></returns>
        public ActionResult AgreeDeal(string DLogUID = "")
        {
            //首先改变订单状态
            using (var db = new LogDealDataContext())
            {
                var log = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();
                if (log != null)
                {
                    log.DealStatus = "交易中";
                    db.SubmitChanges();
                }
            }
            //在物流信息中添加一项
            using (var db=new trackingDataContext())
            {
                //当前订单号不存在时
                if (db.Tracking.Where(t => t.DLogUID == DLogUID).FirstOrDefault() == null)
                {
                    db.Tracking.InsertOnSubmit(new Tracking
                    {
                        DLogUID = DLogUID
                    });
                    db.SubmitChanges();
                }
            }

            return RedirectToAction("DealingLog", "Account");
        }


        /// <summary>
        /// 拒绝申请-我收到的交易申请
        /// </summary>
        /// <returns></returns>
        public ActionResult DisagreeDeal(string DLogUID="")
        {
            if (string.IsNullOrEmpty(DLogUID))
            {
                return View("Error");
            }
            //获取当前交易信息
            LogDeal deal = new LogDeal();
            using (var db = new LogDealDataContext())
            {
                deal = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(DLogUID)).FirstOrDefault();
            }

            //换入物品-对于接收人来说，换入物品是本次申请的发起人物品
            var treaR = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureSponsorID))
                                .FirstOrDefault();
            //换出物品-对于接收人来说，换出物品是本次申请的接收人物品
            var treaS = repository.Treasures
                                .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureRecipientID))
                                .FirstOrDefault();
            if (treaR != null && treaS != null)
            {
                return View(new DealLogCreate
                {
                    DealInTreasure = treaR,
                    DealOutTreasure = treaS,
                    Remark = deal.RemarkSToR,
                    DealLogID = DLogUID
                });
            }

            return View("Error");
        }

        /// <summary>
        /// 拒绝申请[HttpPost]
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult DisagreeDeal(DealLogCreate dealLogCreate)
        {
            //更新本次订单的记录
            using (var db = new LogDealDataContext())
            {
                var log = db.LogDeal.Where(d => d.DLogUID == Guid.Parse(dealLogCreate.DealLogID)).FirstOrDefault();
                log.RemarkRToS = dealLogCreate.Remark;
                log.DealStatus = "拒接";
                log.DealEndTime = DateTime.Now;
                db.SubmitChanges();
            }
            return View();
        }
        #endregion





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