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
            if (ModelState.IsValid)
            {
                repository.SaveTreasure(treasure);
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
            string currID = CurrentUser.Id;
            UserDeal userDeal = new UserDeal();
            //新建List
            List<LogDealTableInfo> logDealTableInfos = new List<LogDealTableInfo>();
            //获取当前登录用户的交易记录
            using(var db = new LogDealDataContext())
            {
                //获取发起人是当前登录用户的交易信息
                var deals = db.LogDeal.Where(d => d.TraderSponsorID == currID);
                if (deals != null)
                {
                    foreach (var deal in deals)
                    {
                        logDealTableInfos.Add(new LogDealTableInfo
                        {
                            LogDeal = deal,
                            TraderRecipientName = UserManager.FindById(deal.TraderRecipientID).UserName,
                            DealInTreaName = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(deal.TreasureRecipientID)).FirstOrDefault().TreasureName,
                            DealOutTreaName = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(deal.TreasureSponsorID)).FirstOrDefault().TreasureName
                        });
                    }
                    userDeal.LogDealTableInfos = logDealTableInfos;
                    return View(userDeal);
                }
            }
            return View("Error");
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
                var deals = db.LogDeal.Where(d => (d.TraderRecipientID == userid));
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
        public ActionResult EndDealLog()
        {
            return View();
        }

        /// <summary>
        /// 正在进行的交易
        /// </summary>
        /// <returns></returns>
        public ActionResult DealingLog()
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