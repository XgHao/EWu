using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.Domain.Db;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.WebUI.Models.ViewModel;
using Ewu.WebUI.HtmlHelpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 用户验证控制器
    /// </summary>
    [Authorize]
    public class AccountController : Controller
    {
        //物品存储库
        private ITreasuresRepository repository;
        //验证接口
        IAuthProvider authProvider;

        //构造函数
        public AccountController(ITreasuresRepository repo,IAuthProvider auth)
        {
            repository = repo;
            authProvider = auth;
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ViewResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登录页面[HttpPost]
        /// </summary>
        /// <param name="model">登录的视图模型</param>
        /// <param name="returnUrl">返回的URL</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Login(LoginViewModel model,string returnUrl)
        {
            //登录视图信息验证无误
            if (ModelState.IsValid)
            {
                //验证成功时
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    //重定向到ReturnUrl，如果为空则默认为Admin/Index
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin"));
                }
                //验证失败时
                else
                {
                    //添加验证错误
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }


        /// <summary>
        /// 用户信息
        /// </summary>
        /// <returns></returns>
        public PartialViewResult UserInfo()
        {
            if (CurrentUser != null)
            {
                return PartialView(CurrentUser);
            }
            else
            {
                CurrentUser.UserName = "请登录";
                CurrentUser.HeadPortrait = "";
                return PartialView(CurrentUser);
            }
        }

        /// <summary>
        /// 编辑物品
        /// </summary>
        /// <param name="TreasureUID">物品UID</param>
        /// <param name="HolderID">物品所属人UID</param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(Guid TreasureUID)
        {
            if (string.IsNullOrEmpty(TreasureUID.ToString()))
            {
                return RedirectToAction("MyList", "Treasure");
            }
            //获取当前用户的UID
            string HolderID = CurrentUser.Id;
            //验证用户UID，确保该物品是所属人在操作
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == TreasureUID && t.HolderID == HolderID);
            //如果存在，转入编辑页面
            if (treasure != null)
            {
                #region 物品类别集合
                IEnumerable<SelectListItem> types = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text="网络设备",Value="网络设备" },
                    new SelectListItem(){ Text="电脑配件",Value="电脑配件" },
                    new SelectListItem(){ Text="图书画册",Value="图书画册" },
                    new SelectListItem(){ Text="电子产品",Value="电子产品" },
                    new SelectListItem(){ Text="其他",Value="其他" }
                };
                types = DropListHelper.SetDefault(types, treasure.TreasureType);
                Session["Types"] = types;
                #endregion

                #region 物品成色集合
                IEnumerable<SelectListItem> damageDegree = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text="全新",Value="全新" },
                    new SelectListItem(){ Text="九八新",Value="九八新" },
                    new SelectListItem(){ Text="九五新",Value="九五新" },
                    new SelectListItem(){ Text="九成新",Value="九成新" },
                    new SelectListItem(){ Text="八五新",Value="八五新" },
                    new SelectListItem(){ Text="八成新",Value="八成新" },
                    new SelectListItem(){ Text="七成新",Value="七成新" },
                    new SelectListItem(){ Text="七成及以下",Value="七成及以下" },
                };
                damageDegree = DropListHelper.SetDefault(damageDegree, treasure.DamageDegree);
                Session["DamageDegrees"] = damageDegree;
                #endregion

                #region 物品交易范围集合
                IEnumerable<SelectListItem> tradeRange = new List<SelectListItem>()
                {
                    new SelectListItem(){ Text="市内",Value="市内" },
                    new SelectListItem(){ Text="省内",Value="省内" },
                    new SelectListItem(){ Text="临近省",Value="临近省" },
                    new SelectListItem(){ Text="全国(港澳台除外)",Value="全国" },
                    new SelectListItem(){ Text="不限",Value="不限" }
                };
                tradeRange = DropListHelper.SetDefault(tradeRange, treasure.TradeRange);
                Session["TradeRanges"] = tradeRange;
                #endregion

                return View(treasure);
            }
            return View("Error");
        }

        /// <summary>
        /// 编辑物品[HttpPost]
        /// </summary>
        /// <param name="treasure">编辑物品对象</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Treasure treasure)
        {
            //验证视图模型
            if (ModelState.IsValid)
            {
                //保存物品对象
                repository.SaveTreasure(treasure);
                //重定向到我的物品页面
                return RedirectToAction("MyList", "Treasure");
            }
            return View(treasure);
        }

        /// <summary>
        /// 修改图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult EditImg(string TreasureUID)
        {
            //清空图片
            if (DropListHelper.DeletePic(Guid.Parse(TreasureUID)))
            {
                Treasure treasure = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(TreasureUID)).FirstOrDefault();
                return View(new UploadImgs
                {
                    TreasureUID = TreasureUID,
                    UserID = treasure.HolderID,
                    TreasureName = treasure.TreasureName
                });
            }
            return View("Error");
        }

        /// <summary>
        /// 用户交易记录所有现存的交易
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult AllDealLog()
        {
            //获取当前用户ID
            string currID = CurrentUser.Id;
            //新建List
            List<LogDealTableInfo> logDealTableInfos = new List<LogDealTableInfo>();
            //获取当前登录用户的交易记录
            using(var db = new LogDealDataContext())
            {
                //获取有当前登录用户参与的交易信息
                var deals = db.LogDeal.Where(d => ((d.TraderSponsorID == currID) || (d.TraderRecipientID == currID))).ToList().AsEnumerable();
              
                //遍历所有交易信息
                foreach (var deal in deals)
                {
                    //获取对方用户对象
                    var TradeID = deal.TraderRecipientID == currID ? deal.TraderSponsorID : deal.TraderRecipientID;
                    var Trader = UserManager.FindById(TradeID);

                    //添加交易记录
                    logDealTableInfos.Add(new LogDealTableInfo
                    {
                        //交易信息
                        LogDeal = deal,
                        //对方用户对象
                        Trader = new BasicUserInfo
                        {
                            UserID = Trader.Id,
                            UserName = Trader.UserName,
                            RealName = Trader.RealName
                        },
                        //换入物品-即对方的物品
                        DealInTrea = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(deal.TraderRecipientID == currID ? deal.TreasureSponsorID : deal.TreasureRecipientID)).FirstOrDefault(),
                        //换出物品-即我的物品
                        DealOutTrea = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(deal.TraderRecipientID == currID ? deal.TreasureRecipientID : deal.TreasureSponsorID)).FirstOrDefault(),
                        IsSponsor = deal.TraderSponsorID == currID
                    });
                }
                return View(new UserDeal
                {
                    LogDealTableInfos = logDealTableInfos.AsEnumerable()
                });
            }
        }

        /// <summary>
        /// 我收到的交易记录
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult RecipientDealLog()
        {
            //新建视图模型List
            List<InitiateDealLog> model = new List<InitiateDealLog>();
            //获取当前登录用户ID
            string userid = CurrentUser.Id;

            //获取当前登录用户接受到交易申请的记录集合
            using(var db=new LogDealDataContext())
            {
                var deals = db.LogDeal.Where(d => ((d.TraderRecipientID == userid)&&(d.DealStatus.Contains("待确认"))));
                foreach(var deal in deals)
                {
                    //收到交易的物品
                    var DealInTrea = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(deal.TreasureSponsorID)).FirstOrDefault();
                    //当前用户的物品
                    var DealOutTrea = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(deal.TreasureRecipientID)).FirstOrDefault();
                    //交易对方信息
                    var Dealer = UserManager.FindById(DealInTrea.HolderID);
                    //添加到视图模型中
                    model.Add(new InitiateDealLog
                    {
                        Dealer = Dealer,
                        InitiateTreasures = DealInTrea,
                        MyTreasure = DealOutTrea,
                        LogDeal = deal
                    });
                }
            }
            return View(model.AsEnumerable());
        }

        /// <summary>
        /// 我发起的交易记录
        /// </summary>
        /// <returns></returns>
        public ActionResult InitiateDealLog()
        {
            //新建视图模型List
            List<InitiateDealLog> model = new List<InitiateDealLog>();
            //获取当前登录用户ID
            string userid = CurrentUser.Id;

            //获取当前登录用户接受到交易申请的记录集合
            using (var db = new LogDealDataContext())
            {
                var deals = db.LogDeal
                                .Where(d => (d.TraderSponsorID == userid) && (d.DealStatus == "待确认"));
                foreach (var deal in deals)
                {
                    //收到交易的物品
                    var DealInTrea = repository.Treasures
                                                .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureRecipientID))
                                                .FirstOrDefault();
                    //当前用户的物品
                    var DealOutTrea = repository.Treasures
                                                .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureSponsorID))
                                                .FirstOrDefault();
                    //交易对方信息
                    var Dealer = UserManager.FindById(DealInTrea.HolderID);
                    //添加到视图模型中
                    model.Add(new InitiateDealLog
                    {
                        Dealer = Dealer,
                        InitiateTreasures = DealInTrea,
                        MyTreasure = DealOutTrea,
                        LogDeal = deal
                    });
                }
            }
            return View(model.AsEnumerable());
        }

        /// <summary>
        /// 结束的交易
        /// </summary>
        /// <returns></returns>
        public ActionResult CompleteDealLog()
        {
            //新建视图模型List
            List<CompleteDeal> model = new List<CompleteDeal>();
            //获取当前登录用户ID
            string userid = CurrentUser.Id;

            //获取当前登录用结束的交易
            using(var db = new LogDealDataContext())
            {
                var deals = db.LogDeal.Where(l => ((l.TraderRecipientID == userid) || (l.TraderSponsorID == userid)) && l.DealStatus.Contains("交易成功"));
                foreach(var deal in deals)
                {
                    //发起方物品
                    var TreaS = repository.Treasures
                                          .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureSponsorID))
                                          .FirstOrDefault();
                    //发起人
                    var UserS = UserManager.FindById(TreaS.HolderID);

                    //接受方物品
                    var TreaR = repository.Treasures
                                          .Where(t => t.TreasureUID == Guid.Parse(deal.TreasureRecipientID))
                                          .FirstOrDefault();
                    //接收人
                    var UserR = UserManager.FindById(TreaR.HolderID);
                    using (var db2 = new EvaluationDataContext())
                    {
                        var Eva = db2.Evaluation.Where(e => e.DLogUID == deal.DLogUID.ToString()).FirstOrDefault();
                        
                        //添加对象
                        model.Add(new CompleteDeal
                        {
                            UserS = new BasicUserInfo
                            {
                                UserID = UserS.Id,
                                RealName = UserS.RealName,
                                HeadImg = UserS.HeadPortrait,
                                Sign = UserS.Signature,
                                BirthDay = UserS.BirthDay.ToString("yyyy-MM-dd"),
                                UserName = UserS.UserName
                            },
                            UserR = new BasicUserInfo
                            {
                                UserID = UserR.Id,
                                RealName = UserR.RealName,
                                HeadImg = UserR.HeadPortrait,
                                Sign = UserR.Signature,
                                BirthDay = UserR.BirthDay.ToString("yyyy-MM-dd"),
                                UserName = UserR.UserName
                            },
                            TreasureS = TreaS,
                            TreasureR = TreaR,
                            Evaluation = Eva,
                            LogDeal = deal
                        });
                    }
                }
            }

            return View(model.AsEnumerable());
        }

        /// <summary>
        /// 正在进行的交易
        /// </summary>
        /// <returns></returns>
        public ActionResult DealingLog()
        {
            //获取当前登录人ID
            string Id = CurrentUser.Id;

            //新建List视图模型
            List<DealingLog> dealingLogs = new List<DealingLog>();

            //获取状态是“交易中”的交易集合
            using(var db = new LogDealDataContext())
            {
                var logs = db.LogDeal.Where(d => (d.DealStatus == "交易中") && ((d.TraderRecipientID == Id) || (d.TraderSponsorID == Id)));

                foreach (var log in logs)
                {
                    //获取对方个人ID
                    string TaID = log.TraderRecipientID == Id ? log.TraderSponsorID : log.TraderRecipientID;

                    //获取对方信息
                    AppUser TaInfo = UserManager.FindById(TaID);

                    //获取物品信息
                    //接受人的物品
                    var TreaR = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(log.TreasureRecipientID)).FirstOrDefault();
                    //发起人的物品
                    var TreaS = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(log.TreasureSponsorID)).FirstOrDefault();

                    //检查当前用户是否已经评价
                    bool IsEvaluation = false;
                    using(var db3 = new EvaluationDataContext())
                    {
                        var eva = db3.Evaluation.Where(e => e.DLogUID == log.DLogUID.ToString()).FirstOrDefault();
                        //如果当前订单的评价信息存在
                        if (eva != null)
                        {
                            //判断当前用户是否已经评价
                            if (Id == log.TraderSponsorID)
                            {
                                //当前用户时发起人
                                if (eva.IsRecommendSToR != null)
                                {
                                    IsEvaluation = true;
                                }
                            }
                            else if (Id == log.TraderRecipientID)
                            {
                                if (eva.IsRecommendRToS != null)
                                {
                                    IsEvaluation = true;
                                }
                            }
                            else
                            {
                                return View("Error");
                            }

                        }
                    }

                    //查看对方收货地址
                    DeliveryAddress deliveryAddress = new DeliveryAddress();
                    using(var db4 = new DeliveryAddressDataContext())
                    {
                        if (log.TraderRecipientID == TaID)
                        {
                            deliveryAddress = db4.DeliveryAddress.Where(a => a.DeliveryAddressUID == log.DeliveryAddressRecipientID).FirstOrDefault();
                        }
                        else
                        {
                            deliveryAddress = db4.DeliveryAddress.Where(a => a.DeliveryAddressUID == log.DeliveryAddressSponsorID).FirstOrDefault();
                        }
                    }

                    //添加视图模型
                    if (TreaR != null && TreaS != null)
                    {
                        using (var db2 = new trackingDataContext())
                        {
                            var tracking = db2.Tracking.Where(t => t.DLogUID == log.DLogUID.ToString()).FirstOrDefault();
                            dealingLogs.Add(new DealingLog
                            {
                                LogDeal = log,
                                My = CurrentUser,
                                //我的物品-如果这个接受物品所属人的ID不是当前登录人ID，则当前登录人即我是发起人
                                MyTreasure = TreaR.HolderID == Id ? TreaR : TreaS,
                                Ta = TaInfo,
                                TaTreasure = TreaR.HolderID == TaID ? TreaR : TreaS,
                                Tracking = tracking,
                                //当前用户在本次交易中是什么角色
                                CurrentUserRole = TreaR.HolderID == Id ? "Recipient" : "Sponsor",
                                IsEvaluation = IsEvaluation,
                                DeliveryAddress = deliveryAddress
                            });
                        }
                    }
                    else
                    {
                        break;
                    }

                }
            }

            //返回视图
            return View(dealingLogs.AsEnumerable());
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