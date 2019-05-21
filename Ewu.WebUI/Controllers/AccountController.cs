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
using Ewu.WebUI.API;
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
        [Authorize]
        public PartialViewResult UserInfo()
        {
            //获取当前用户id
            string userid = CurrentUser.Id;

            //新建通知留言List
            List<NoticeTongZhi> tongZhis = new List<NoticeTongZhi>();
            List<NoticeLiuYan> liuYans = new List<NoticeLiuYan>();

            //获取信息
            using (var db = new NoticeDataContext())
            {
                var notices = db.Notice.Where(n => (n.RecipientID == userid && n.IsRead == false)).OrderByDescending(n => n.NoticeTime);
                //遍历所有的Notice
                foreach(var notice in notices)
                {
                    //获取当前发起人的信息
                    var user = UserManager.FindById(notice.SponsorID);
                    if (user != null)
                    {
                        //留言类
                        if (notice.NoticeObject == "留言")
                        {
                            liuYans.Add(new NoticeLiuYan
                            {
                                BasicUserInfo = new BasicUserInfo
                                {
                                    UserID = user.Id,
                                    HeadImg = user.HeadPortrait,
                                    UserName = user.UserName
                                },
                                Content = notice.NoticeContent,
                                isRead = notice.IsRead,
                                Time = notice.NoticeTime,
                                Id = notice.NoticeUID.ToString()
                            });
                        }
                        //其他通知类
                        else
                        {
                            //获取物品对象
                            var trea = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(notice.TreasureUID)).FirstOrDefault();
                            if (trea != null)
                            {
                                tongZhis.Add(new NoticeTongZhi
                                {
                                    BasicUserInfo = new BasicUserInfo
                                    {
                                        UserID = user.Id,
                                        UserName = user.UserName,
                                        HeadImg = user.HeadPortrait
                                    },
                                    Action = notice.NoticeObject,
                                    isRead = notice.IsRead,
                                    Time = notice.NoticeTime,
                                    Treasure = trea,
                                    Id = notice.NoticeUID.ToString()
                                });
                            }
                        }
                    }
                }
            }

            return PartialView(new UserHeadInfo
            {
                BasicUserInfo = new BasicUserInfo
                {
                    HeadImg = CurrentUser.HeadPortrait,
                    UserID = CurrentUser.Id,
                    RealName = CurrentUser.RealName
                },
                noticeLiuYans = liuYans,
                noticeTongZhis = tongZhis
            });
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
        /// 用户个人信息
        /// </summary>
        /// <param name="UserID">对象用户UID</param>
        /// <returns></returns>
        public ActionResult AccountInfo(string UserID = "")
        {
            //新建视图模型
            AccountInfoViewModel model = new AccountInfoViewModel
            {
                TotalBrowseNum = 0,
                TotalFavorite = 0,
                TotalTreasureNum = 0
            };

            if (!string.IsNullOrEmpty(UserID))
            {
                //获取查看的用户对象
                var user = UserManager.FindById(UserID);
                if (user != null)
                {
                    //添加用户对象
                    model.TargetUser = user;
                    //获取该用户的物品集合,且没有交易
                    var treasures = repository.Treasures.Where(t => t.HolderID == UserID);
                    if (treasures != null)
                    {
                        //遍历
                        foreach (var trea in treasures)
                        {
                            model.TotalBrowseNum += trea.BrowseNum;
                            model.TotalFavorite += trea.Favorite;
                            model.TotalTreasureNum++;
                        }
                        //添加物品集合，选择前三个，按时间排序，并且没有正在的交易
                        var TargetTrea = treasures.Where(t => t.DLogUID == null).OrderBy(t => t.UploadTime).Take(3);

                        using(var db = new FavoriteDataContext())
                        {
                            //根据Treasure生成对应的TreasureCard
                            List<TreasureCard> treasureCards_T = new List<TreasureCard>();
                            foreach(var trea in TargetTrea)
                            {
                                treasureCards_T.Add(new TreasureCard
                                {
                                    Treasure = trea,
                                });
                            }
                            model.TargetTreasures = treasureCards_T.AsEnumerable();

                            //收藏
                            string FavoriteTreaID = string.Empty;
                            var favorites = db.Favorite.Where(f => f.UserID == UserID).OrderBy(f => f.FavoriteTime).Take(3);
                            foreach (var favo in favorites)
                            {
                                FavoriteTreaID += "|||" + favo.TreasureID;
                            }
                            //获取收藏的物品
                            var favoriteTrea = repository.Treasures.Where(t => FavoriteTreaID.Contains(t.TreasureUID.ToString()));

                            //收藏物品
                            List<TreasureCard> treasureCards_F = new List<TreasureCard>();
                            foreach(var trea in favoriteTrea)
                            {
                                //获取物品所属人
                                var holder = UserManager.FindById(trea.HolderID);

                                treasureCards_F.Add(new TreasureCard
                                {
                                    Treasure = trea,
                                    TreasureHolder = new BasicUserInfo
                                    {
                                        UserID = holder.Id,
                                        UserName = holder.UserName,
                                        HeadImg = holder.HeadPortrait
                                    }
                                });
                            }

                            //添加视图
                            model.TargetFavorite = treasureCards_F.AsEnumerable();
                        }

                        //评价
                        using(var db2 = new LogDealDataContext())
                        {
                            //首先获取有当前用户的所有订单
                            var logs = db2.LogDeal.Where(l => (l.TraderRecipientID == UserID || l.TraderSponsorID == UserID));
                            List<UserEvaluation> userEvaluations = new List<UserEvaluation>();
                            //遍历所有订单，获取获取每个订单中的评价
                            foreach (var log in logs)
                            {
                                using (var db3 = new EvaluationDataContext())
                                {
                                    //获取订单中的评价信息
                                    var evaluation = db3.Evaluation.Where(e => e.DLogUID == log.DLogUID.ToString()).FirstOrDefault();
                                    //如果有评价信息
                                    if (evaluation != null)
                                    {
                                        //用户是接收人，则需要的评论是发起人
                                        if (log.TraderRecipientID == UserID)
                                        {
                                            //获取评论人对象
                                            var evaUser = UserManager.FindById(log.TraderSponsorID);

                                            //添加评价信息
                                            userEvaluations.Add(new UserEvaluation
                                            {
                                                Time = evaluation.EvaTimeSToR,
                                                EvaluationInfo = evaluation.EvaluationSToR,
                                                Holder = new BasicUserInfo
                                                {
                                                    HeadImg = evaUser.HeadPortrait,
                                                    UserName = evaUser.UserName,
                                                    UserID = evaUser.Id
                                                },
                                                IsReaommend = evaluation.IsRecommendSToR
                                            });
                                        }
                                        else if (log.TraderSponsorID == UserID)
                                        {
                                            //获取评论人对象
                                            var evaUser = UserManager.FindById(log.TraderRecipientID);

                                            //添加评价信息
                                            userEvaluations.Add(new UserEvaluation
                                            {
                                                Time = evaluation.EvaTimeRToS,
                                                EvaluationInfo = evaluation.EvaluationRToS,
                                                Holder = new BasicUserInfo
                                                {
                                                    HeadImg = evaUser.HeadPortrait,
                                                    UserName = evaUser.UserName,
                                                    UserID = evaUser.Id
                                                },
                                                IsReaommend = evaluation.IsRecommendRToS
                                            });
                                        }
                                    }
                                    //添加数据
                                    model.Evaluations = userEvaluations;
                                }
                            }

                            return View(model);
                        }
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="TreaUID"></param>
        /// <returns></returns>
        public JsonResult AddFavorite(string TreaUID="")
        {
            string result = "Fail";
            //获取当前用户id
            var curruserid = CurrentUser.Id;

            if (!string.IsNullOrEmpty(TreaUID))
            {
                using(var db = new FavoriteDataContext())
                {
                    //首先检查是不是已经收藏了
                    var fav = db.Favorite.Where(f => (f.UserID == curruserid && f.TreasureID == TreaUID)).FirstOrDefault();
                    //为空，则添加记录
                    if (fav == null)
                    {
                        db.Favorite.InsertOnSubmit(new Domain.Db.Favorite
                        {
                            FavoriteUID = Guid.NewGuid().ToString(),
                            FavoriteTime = DateTime.Now,
                            TreasureID = TreaUID,
                            UserID = curruserid
                        });
                        db.SubmitChanges();
                        result = "OK";
                        //相应的物品收藏量加一
                        var trea = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(TreaUID)).FirstOrDefault();
                        if (trea != null)
                        {
                            trea.Favorite++;
                            repository.SaveTreasure(trea);
                        }
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="TreaUID"></param>
        /// <returns></returns>
        public JsonResult CancelFavorite(string TreaUID = "")
        {
            string result = "Fail";
            //获取当前用户id
            var curruserid = CurrentUser.Id;

            if (!string.IsNullOrEmpty(TreaUID))
            {
                using (var db = new FavoriteDataContext())
                {
                    //首先检查是不是已经收藏了
                    var fav = db.Favorite.Where(f => (f.UserID == curruserid && f.TreasureID == TreaUID)).FirstOrDefault();
                    //为空，则删除记录
                    if (fav != null)
                    {
                        db.Favorite.DeleteOnSubmit(fav);
                        db.SubmitChanges();
                        result = "OK";
                        //相应的物品收藏量减一
                        var trea = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(TreaUID)).FirstOrDefault();
                        if (trea != null)
                        {
                            trea.Favorite--;
                            repository.SaveTreasure(trea);
                        }
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 评价
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public JsonResult Comment(string UserID="",string Comment = "",string TreaUID = "")
        {
            string result = "Fail";
            if (!string.IsNullOrEmpty(UserID) && !string.IsNullOrEmpty(Comment) && !string.IsNullOrEmpty(TreaUID)) 
            {
                //获取当前用户id
                string curruserid = CurrentUser.Id;
                if (curruserid != UserID)
                {
                    if (UserManager.FindById(UserID) != null)
                    {
                        //添加Notice
                        new Identity().AddNotice(UserID, curruserid, "咨询", Comment, TreaUID, null);
                        result = "OK";
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 回复
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        public JsonResult Reply(string UserID = "", string Comment = "", string NoticeId = "")
        {
            string result = "Fail";
            if (!string.IsNullOrEmpty(UserID) && !string.IsNullOrEmpty(Comment) && !string.IsNullOrEmpty(NoticeId))
            {
                //获取当前用户id
                string curruserid = CurrentUser.Id;
                if (curruserid != UserID)
                {
                    if (UserManager.FindById(UserID) != null)
                    {
                        //添加Notice
                        new Identity().AddNotice(UserID, curruserid, "留言", Comment, null, NoticeId);
                        result = "OK";
                    }
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 我的通知
        /// </summary>
        /// <returns></returns>
        public ActionResult Message(string NoticeUID = "")
        {
            if (!string.IsNullOrEmpty(NoticeUID))
            {
                //获取通知对象
                using(var db = new NoticeDataContext())
                {
                    var notice = db.Notice.Where(n => n.NoticeUID == Guid.Parse(NoticeUID)).FirstOrDefault();

                    //将当前通知设为已读
                    notice.IsRead = true;
                    db.SubmitChanges();

                    if (notice != null)
                    {
                        //新建视图模型
                        MessageViewModel model = new MessageViewModel
                        {
                            BasicUserInfoMy = new BasicUserInfo
                            {
                                UserID = CurrentUser.Id,
                                UserName=CurrentUser.UserName
                            },
                            BasicUserInfoTa = new BasicUserInfo
                            {
                                UserID = UserManager.FindById(notice.SponsorID).Id,
                                UserName=UserManager.FindById(notice.SponsorID).UserName
                            },
                            CurrMessUID = notice.NoticeUID.ToString(),
                        };

                        //新建历史消息集合
                        List<Message> messageLog = new List<Message>();
                        bool isR = true;
                        //不为空，
                        if (notice.RelpyNoticeUID != null)
                        {
                            do
                            {
                                //获取发起人
                                var Spon = UserManager.FindById(notice.SponsorID);
                                if (Spon != null)
                                {
                                    messageLog.Add(new Message
                                    {
                                        BasicUserInfo = new BasicUserInfo
                                        {
                                            UserID = Spon.Id,
                                            HeadImg = Spon.HeadPortrait,
                                            Email = Spon.Email,
                                            UserName = Spon.UserName
                                        },
                                        Notice = notice
                                    });
                                }
                                if (notice.RelpyNoticeUID == null)
                                {
                                    break;
                                }
                                else
                                {
                                    notice = db.Notice.Where(n => n.NoticeUID == Guid.Parse(notice.RelpyNoticeUID)).FirstOrDefault();
                                }
                            } while (true);
                        }
                        else
                        {
                            do
                            {
                                //获取发起人
                                var Spon = UserManager.FindById(notice.SponsorID);
                                if (Spon != null)
                                {
                                    messageLog.Add(new Message
                                    {
                                        BasicUserInfo = new BasicUserInfo
                                        {
                                            UserID = Spon.Id,
                                            HeadImg = Spon.HeadPortrait,
                                            Email = Spon.Email,
                                            UserName = Spon.UserName
                                        },
                                        Notice = notice
                                    });
                                }
                                //检查当前有无回复目标
                                notice = db.Notice.Where(t => t.RelpyNoticeUID == notice.NoticeUID.ToString()).FirstOrDefault();
                                if (notice == null)
                                {
                                    break;
                                }
                            } while (true);
                            isR = false;
                        }
                        if (isR)
                        {
                            var notice2 = db.Notice.Where(n => n.NoticeUID == Guid.Parse(NoticeUID)).FirstOrDefault();
                            int cnt = 0;
                            do
                            {
                                //获取发起人
                                var Spon = UserManager.FindById(notice2.SponsorID);
                                if (Spon != null)
                                {
                                    if (cnt != 0)
                                    {
                                        messageLog.Add(new Message
                                        {
                                            BasicUserInfo = new BasicUserInfo
                                            {
                                                UserID = Spon.Id,
                                                HeadImg = Spon.HeadPortrait,
                                                Email = Spon.Email,
                                                UserName = Spon.UserName
                                            },
                                            Notice = notice2
                                        });
                                    }
                                }
                                //检查当前有无回复目标
                                notice2 = db.Notice.Where(t => t.RelpyNoticeUID == notice2.NoticeUID.ToString()).FirstOrDefault();
                                if (notice2 == null)
                                {
                                    break;
                                }
                                cnt = 1;
                            } while (true);
                        }

                        //添加数据
                        model.Messages = messageLog.OrderBy(m=>m.Notice.NoticeTime).Distinct().AsEnumerable();
                        return View(model);
                    }
                }
            }

            return View("Error");
        }

        /// <summary>
        /// 所有消息
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult AllMessage()
        {
            string id = CurrentUser.Id;
            List<Message> model = new List<Message>();

            using (var db = new NoticeDataContext())
            {
                var notices = db.Notice.Where(n => ((n.RecipientID == id || n.SponsorID == id) && (n.NoticeObject == "留言" || n.NoticeObject == "咨询")));
                foreach(var no in notices)
                {
                    var user = UserManager.FindById(no.SponsorID);
                    model.Add(new Message
                    {
                        BasicUserInfo = new BasicUserInfo
                        {
                            UserID = user.Id,
                            HeadImg = user.HeadPortrait,
                            UserName = user.UserName
                        },
                        Notice = no
                    });
                }
                return View(new AllMessageViewModel
                {
                    Cnt = notices.Count(),
                    Messages = model.OrderBy(m=>m.Notice.NoticeTime).AsEnumerable()
                });
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