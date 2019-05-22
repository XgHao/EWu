using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.Domain.Db;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.WebUI.Models;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Models.ViewModel;
using Ewu.WebUI.HtmlHelpers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 物品控制器
    /// </summary>
    public class TreasureController : Controller
    {
        private ITreasuresRepository repository;    //定义的物品储存库
        private IAuthProvider authProvider;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="treasuresRepository">传入储存库</param>
        public TreasureController(ITreasuresRepository treasuresRepository,IAuthProvider auth)
        {
            repository = treasuresRepository;
            authProvider = auth;
        }

        /// <summary>
        /// 主页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult Index()
        {
            //新建视图
            IndexViewModel model = new IndexViewModel();

            var Alltreasure = repository.Treasures.Where(t => t.DLogUID == null).ToList().AsEnumerable();

            #region 随便看看
            //判断数量，决定要显示的物品数量
            int cnt = Alltreasure.Count() < 50 ? 4 : Alltreasure.Count() / 10;
            cnt = cnt > Alltreasure.Count() ? Alltreasure.Count() : cnt;
            List<Treasure> treasureRandom = new List<Treasure>();
            for (int i = 0; i < cnt; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                int res = r.Next(0, Alltreasure.Count());
                treasureRandom.Add(Alltreasure.ElementAt(res));
            }
            List<TreasureAndHolderInfo> treasuresRandomList = new List<TreasureAndHolderInfo>();
            foreach(var trea in treasureRandom)
            {
                //获取物品所属人对象
                var holder = UserManager.FindById(trea.HolderID);

                //是否被收藏
                bool IsFavorite = false;
                using(var db = new FavoriteDataContext())
                {
                    var log = db.Favorite.Where(f => (f.TreasureID == trea.TreasureUID.ToString() && f.UserID == CurrentUser.Id)).FirstOrDefault();
                    if (log != null)
                    {
                        IsFavorite = true;
                    }
                }
                if (holder != null)
                {
                    var detail = trea.DetailPic.Split('|');
                    trea.DetailPic = detail.Last();
                    treasuresRandomList.Add(new TreasureAndHolderInfo
                    {
                        Holder = holder,
                        Treasure = trea,
                        IsFavorite = IsFavorite
                    });
                }
            }
            model.RandomTrea = treasuresRandomList.AsEnumerable();
            #endregion

            #region 最新物品
            int cnt2 = Alltreasure.Count() > 6 ? 6 : Alltreasure.Count();
            Alltreasure.OrderBy(t => t.UploadTime);
            List<Treasure> treasuresNew = Alltreasure.Take(cnt2).ToList();
            List<TreasureAndHolderInfo> treasuresNewList = new List<TreasureAndHolderInfo>();
            foreach(var trea in treasuresNew)
            {
                //获取物品所属人
                var holder = UserManager.FindById(trea.HolderID);

                //是否被收藏
                bool IsFavorite = false;
                using(var db = new FavoriteDataContext())
                {
                    var log = db.Favorite.Where(f => (f.TreasureID == trea.TreasureUID.ToString() && f.UserID == CurrentUser.Id)).FirstOrDefault();
                    if (log != null)
                    {
                        IsFavorite = true;
                    }
                }
                if (holder != null)
                {
                    treasuresNewList.Add(new TreasureAndHolderInfo
                    {
                        Holder = holder,
                        Treasure = trea,
                        IsFavorite = IsFavorite
                    });
                }
            }
            model.NewestTrea = treasuresNewList.AsEnumerable();
            #endregion

            #region 最近热门
            var treaHot = repository.Treasures.Where(t => t.DLogUID == null).OrderBy(t => t.BrowseNum);
            int cnt3 = treaHot.Count() > 6 ? 6 : treaHot.Count();
            var treaHotList = treaHot.Take(cnt3);
            List<TreasureAndHolderInfo> treasuresHotList = new List<TreasureAndHolderInfo>();
            foreach(var trea in treaHotList)
            {
                var holder = UserManager.FindById(trea.HolderID);
                bool isFavorite = false;
                using(var db = new FavoriteDataContext())
                {
                    var log = db.Favorite.Where(f => (f.TreasureID == trea.TreasureUID.ToString() && f.UserID == CurrentUser.Id)).FirstOrDefault();
                    if (log != null)
                    {
                        isFavorite = true;
                        
                    }
                }
                if (holder != null)
                {
                    treasuresHotList.Add(new TreasureAndHolderInfo
                    {
                        Holder = holder,
                        Treasure = trea,
                        IsFavorite = isFavorite
                    });
                }
            }
            model.HotTrea = treasuresHotList.AsEnumerable();
            #endregion

            #region 数据
            model.TreasureCnt = repository.Treasures.Count();
            using(var db =new LogDealDataContext())
            {
                var deallog = db.LogDeal.Where(l => (l.DealStatus == "交易中" || l.DealStatus == "待确认"));
                model.DealingCnt = deallog.Count();
            }
            using(var db = new EvaluationDataContext())
            {
                var Evaluation = db.Evaluation;
                model.EvaluationCnt = Evaluation.Count();
            }
            using(var db = new AspNetUserDataContext())
            {
                var user = db.AspNetUsers;
                model.UserCnt = user.Count();
            }
            #endregion

            return View(model);
        }

        /// <summary>
        /// 搜索
        /// </summary>
        public ActionResult Search(string KeyWord)
        {
            if (!string.IsNullOrEmpty(KeyWord))
            {
                //首先把用户全部过滤出来
                using(var db = new AspNetUserDataContext())
                {
                    var users = db.AspNetUsers.Where(u => u.UserName.Contains(KeyWord)).ToList();
                    string usersIDs = string.Empty;
                    foreach(var user in users)
                    {
                        usersIDs += "|" + user.Id;
                    }

                    //获取物品集合
                    var treasures = repository.Treasures.Where(t => (t.DetailContent.Contains(KeyWord) || t.TreasureName.Contains(KeyWord) || t.UploadTime.ToString("yyyy/MM/dd").Contains(KeyWord) || t.TreasureType.Contains(KeyWord) || usersIDs.Contains(t.HolderID))).ToList();
                    List<TreasureAndHolderInfo> model = new List<TreasureAndHolderInfo>();
                    foreach(var trea in treasures)
                    {
                        //是否被收藏
                        bool IsFavorite = false;
                        using (var db2 = new FavoriteDataContext())
                        {
                            var log = db2.Favorite.Where(f => (f.TreasureID == trea.TreasureUID.ToString() && f.UserID == CurrentUser.Id)).FirstOrDefault();
                            if (log != null)
                            {
                                IsFavorite = true;
                            }
                        }
                        var holder = UserManager.FindById(trea.HolderID);
                        if (holder != null)
                        {
                            model.Add(new TreasureAndHolderInfo
                            {
                                Holder = holder,
                                IsFavorite = IsFavorite,
                                Treasure = trea
                            });
                        }
                    }
                    return View(model.AsEnumerable());
                }
            }
            return View(new LinkedList<TreasureAndHolderInfo>().AsEnumerable());
        }

        /// <summary>
        /// 物品列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [Authorize]
        public ViewResult List(string category, int page = 1, int PageSize = 12)
        {
            //获取当前用户ID
            string id = CurrentUser.Id;

            //1.首先获取当前条件下的所有物品集合
            var Treasures = repository.Treasures
                                //筛选-1.当前类或者类型为空的 2.不能选择图片为空的(图片为空当作未完成项) 3.有正在交易的订单
                                .Where(t => (category == null || t.TreasureType == category) && (t.Cover != null && t.DetailPic != null) && (t.DLogUID == null))
                                .OrderBy(t => t.TreasureName)
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize);

            //新建一个List
            List<TreasureAndHolderInfo> treasureAndHolders = new List<TreasureAndHolderInfo>();
            //遍历物品集合，填充数据
            foreach (var trea in Treasures)
            {
                AppUser holder = UserManager.FindById(trea.HolderID);
                bool IsFavorite = false;
                //检查是否已收藏
                using(var db = new FavoriteDataContext())
                {
                    var fav = db.Favorite.Where(f => (f.UserID == id && f.TreasureID == trea.TreasureUID.ToString())).FirstOrDefault();
                    //不等于空，既有收藏记录
                    if (fav != null)
                    {
                        IsFavorite = true;
                    }
                }
                //添加模型
                treasureAndHolders.Add(new TreasureAndHolderInfo
                {
                    Treasure = trea,
                    Holder = holder,
                    IsFavorite = IsFavorite
                });
            }


            //生成一个具体的列表视图模型
            TreasureListViewModel model = new TreasureListViewModel
            {
                //物品用户信息
                TreasureAndHolderInfos = treasureAndHolders,
                //分页信息
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    //总页数，无选择分类这全部，否则按当前的分类
                    TotalItem = category == null
                                          ? repository.Treasures.Count()
                                          : repository.Treasures.Where(e => e.TreasureType == category).Count()
                },
                //当前分类
                CurrentCate = category,
                //当前用户信息
                CurrentUserInfo = CurrentUser,
                AllCnt=repository.Treasures.Count()
            };
            return View(model);
        }

        /// <summary>
        /// 个人物品页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MyList(string category, int page = 1, int PageSize = 6)
        {
            //根据页码以及分类来确定具体要显示的物品列表
            var Treasures = repository.Treasures
                                //筛选-1.类型为空或者当前类 2.是当前登录用户的物品 3.图片为空不显示
                                .Where(t => (category == null || t.TreasureType == category) && t.HolderID == CurrentUser.Id)
                                .OrderBy(t => t.TreasureName)
                                .Skip((page - 1) * PageSize)
                                .Take(PageSize);

            //新建一个List
            List<TreasureAndHolderInfo> treasureAndHolders = new List<TreasureAndHolderInfo>();
            //遍历物品集合，填充数据
            foreach (var trea in Treasures)
            {
                AppUser holder = UserManager.FindById(trea.HolderID);
                treasureAndHolders.Add(new TreasureAndHolderInfo
                {
                    Treasure = trea,
                    Holder = holder
                });
            }

            //生成一个具体的列表视图模型
            TreasureListViewModel model = new TreasureListViewModel
            {
                TreasureAndHolderInfos = treasureAndHolders,
                //分页信息
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    //总页数，无选择分类这全部，否则按当前的分类
                    TotalItem = category == null
                                          ? repository.Treasures.Count()
                                          : repository.Treasures.Where(e => e.TreasureType == category).Count()
                },
                //当前分类
                CurrentCate = category,
                //当前用户信息
                CurrentUserInfo = CurrentUser
            };
            return View(model);
        }

        /// <summary>
        /// 用户物品列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public ActionResult AccountList(string UserID = "")
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                var appuser = UserManager.FindById(UserID);
                if (appuser != null)
                {
                    //获取当前用户的ID
                    var curUserid = CurrentUser.Id;

                    //如果显示的列表是当前登录用户，则跳转到MyList
                    if (curUserid == UserID)
                    {
                        return RedirectToAction("MyList");
                    }

                    //获取用户列表
                    var treasures = repository.Treasures.Where(t => (t.HolderID == UserID) && (t.DLogUID == null)).OrderBy(t => t.UploadTime);

                    if (treasures != null)
                    {
                        //新建视图模型
                        List<TreasureCard> model = new List<TreasureCard>();


                        //遍历
                        foreach (var trea in treasures)
                        {
                            //获取对应物品的用户
                            var holder = UserManager.FindById(trea.HolderID);
                            model.Add(new TreasureCard
                            {
                                Treasure = trea,
                                userInfo = new BasicUserInfo
                                {
                                    UserName = appuser.UserName
                                }
                            });
                        }
                        return View(model.AsEnumerable());
                    }
                }
            }

            return View();
        }

        /// <summary>
        /// 用户收藏列表
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult AccountFavorite(string UserID = "")
        {
            if (!string.IsNullOrEmpty(UserID))
            {
                var appuser = UserManager.FindById(UserID);
                if (appuser != null)
                {
                    //获取当前用户的ID
                    var curUserid = CurrentUser.Id;

                    //获取收藏列表
                    List<string> favoriteList = new List<string>();
                    using (var db = new FavoriteDataContext())
                    {
                        var favorites = db.Favorite.Where(f => f.UserID == UserID).OrderBy(f => f.FavoriteTime);
                        foreach(var favo in favorites)
                        {
                            favoriteList.Add(favo.TreasureID);
                        }
                    }

                    //根据收藏列表取出对应的物品列表
                    List<Treasure> favoriteTrea = new List<Treasure>();
                    foreach(var favo in favoriteList)
                    {
                        var favoT = repository.Treasures.Where(t => t.TreasureUID == Guid.Parse(favo)).FirstOrDefault();
                        if (favoT != null)
                        {
                            favoriteTrea.Add(favoT);
                        }
                    }

                    //新建视图模型
                    List<TreasureCard> model = new List<TreasureCard>();

                    //遍历
                    foreach (var trea in favoriteTrea)
                    {
                        //获取对应物品的用户
                        var holder = UserManager.FindById(trea.HolderID);
                        model.Add(new TreasureCard
                        {
                            Treasure = trea,
                            TreasureHolder = new BasicUserInfo
                            {
                                UserID = holder.Id,
                                HeadImg = holder.HeadPortrait,
                                UserName = holder.UserName
                            },
                            userInfo = new BasicUserInfo
                            {
                                UserName = appuser.UserName
                            },
                        });
                    }
                    return View(model.AsEnumerable());
                }
            }
            return View("Error");
        }

        /// <summary>
        /// 物品详情页
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult TreasureInfo(string TreasureUID = "")
        {
            //获取当前用户id
            string id = CurrentUser.Id;

            if (!string.IsNullOrEmpty(TreasureUID))
            {
                Guid Treasureguid = Guid.Parse(TreasureUID);

                #region 增加一次浏览量
                //判断当前用户，当前物品的浏览记录是否已经存在
                using (var db = new LogDataContext())
                {
                    var logbrowse = db.LogBrowse.Where(b => (b.TreasureID == TreasureUID && b.BrowserID == id)).FirstOrDefault();

                    //不存在记录，则增加一条
                    if (logbrowse == null)
                    {
                        db.LogBrowse.InsertOnSubmit(new LogBrowse
                        {
                            BLogUID = Guid.NewGuid(),
                            BrowserID = id,
                            TreasureID = TreasureUID,
                            BrowserTime = DateTime.Now
                        });
                        //物品浏览量加一
                        var trea = repository.Treasures.Where(t => t.TreasureUID == Treasureguid).FirstOrDefault();
                        trea.BrowseNum++;
                        repository.SaveTreasure(trea);
                    }
                    //若存在则修改访问时间
                    else
                    {
                        logbrowse.BrowserTime = DateTime.Now;

                    }
                    db.SubmitChanges();
                }
                #endregion

                Treasure treasure = repository.Treasures.Where(t => t.TreasureUID == Treasureguid).FirstOrDefault();
                var imgs = treasure.DetailPic.Split('|');
                if (treasure != null)
                {
                    //判断是否已经收藏
                    bool isFavarite = false;
                    using(var db = new FavoriteDataContext())
                    {
                        var fav = db.Favorite.Where(f => (f.UserID == id && f.TreasureID == TreasureUID)).FirstOrDefault();
                        //若不为空，即存在记录，则说明已经收藏
                        if (fav != null)
                        {
                            isFavarite = true;
                        }
                    }

                    //获取浏览记录
                    List<BrowseLog> browses = new List<BrowseLog>();
                    using(var db = new LogDataContext())
                    {
                        var logBrowses = db.LogBrowse.Where(b => b.TreasureID == TreasureUID).OrderByDescending(b => b.BrowserTime).Take(6);
                        foreach(var log in logBrowses)
                        {
                            var user = UserManager.FindById(log.BrowserID);
                            if (user != null)
                            {
                                browses.Add(new BrowseLog
                                {
                                    Browser = new BasicUserInfo
                                    {
                                        HeadImg = user.HeadPortrait,
                                        UserID = user.Id,
                                        Gender = user.Gender,
                                        UserName = user.UserName
                                    },
                                    BrowserTime = log.BrowserTime
                                });
                            }
                        }
                    }


                    //生成推荐信息
                    int DisRecommend = 0;
                    int Recommend = 0;
                    // 1.首先找出当前用户完成的订单
                    using (var db = new LogDealDataContext())
                    {
                        var logs = db.LogDeal.Where(l => ((l.TraderRecipientID == id || l.TraderSponsorID == id) && (l.DealStatus == "交易成功")));
                        using (var db2 = new EvaluationDataContext())
                        {
                            //遍历所有完成的订单
                            foreach(var log in logs)
                            {
                                var eva = db2.Evaluation.Where(e => e.DLogUID == log.DLogUID.ToString()).FirstOrDefault();
                                //本次交易用户是接收人，则需要发起人的评价
                                if (log.TraderRecipientID == id)
                                {
                                    //推荐
                                    if (eva.IsRecommendSToR == true)
                                    {
                                        Recommend++;
                                    }else if (eva.IsRecommendSToR == false)
                                    {
                                        DisRecommend++;
                                    }
                                }
                                else
                                {
                                    if (eva.IsRecommendRToS == true)
                                    {
                                        Recommend++;
                                    }else if (eva.IsRecommendRToS == false)
                                    {
                                        DisRecommend++;
                                    }
                                }
                            }
                        }
                    }


                    //定义一个视图模型
                    TreaInfo treaInfo = new TreaInfo
                    {
                        HolderInfo = GetLoginUserInfo(treasure.HolderID),
                        LoginUserInfo = CurrentUser,
                        treasureInfo = treasure,
                        //108是生成图片路径的固定的长度
                        DetailImgs = imgs.Where(t => t.Length == 108),
                        IsFavorite = isFavarite,
                        CurrenUser = new BasicUserInfo
                        {
                            HeadImg = CurrentUser.HeadPortrait
                        },
                        browseLogs = browses.AsEnumerable(),
                        DisRecommend = DisRecommend,
                        Recommend = Recommend
                    };
                    return View(treaInfo);
                }
            }
            return View("List");
        }


        /// <summary>
        /// 发布新的物品
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UploadItem()
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
            types = DropListHelper.SetDefault(types, "其他");
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
            damageDegree = DropListHelper.SetDefault(damageDegree, "全新");
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
            tradeRange = DropListHelper.SetDefault(tradeRange, "不限");
            Session["TradeRanges"] = tradeRange;
            #endregion

            Treasure treasure = new Treasure()
            {
                TreasureUID = Guid.NewGuid(),
                HolderID = CurrentUser.Id,
            };
            return View(treasure);
        }

        /// <summary>
        /// 发布新的物品[HttpPost]
        /// </summary>
        /// <param name="treasure"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public ActionResult UploadItem(Treasure treasure)
        {
            if (ModelState.IsValid)
            {
                #region 数据初始化
                treasure.BrowseNum = 0;
                treasure.Favorite = 0;
                treasure.UpdateTime = DateTime.Now;
                treasure.UploadTime = DateTime.Now;
                treasure.EditCount = 0;
                treasure.Link = "/Treasure/TreasureInfo?TreasureUID=" + treasure.TreasureUID.ToString();
                if (string.IsNullOrEmpty(treasure.Remarks))
                {
                    treasure.Remarks = "无";
                }
                #endregion
                repository.SaveTreasure(treasure);
                UploadImgs uploadImgs = new UploadImgs
                {
                    TreasureUID = treasure.TreasureUID.ToString(),
                    UserID = treasure.HolderID,
                    TreasureName = treasure.TreasureName
                };

                //再跳转到上传图片页面前，要先清空原来的图片路径
                if (DropListHelper.DeletePic(treasure.TreasureUID))
                {
                    return View("UpLoadImg", uploadImgs);
                }
            }
            return View(treasure);
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult UpLoadImg()
        {
            return View();
        }

        /// <summary>
        /// 获取图片
        /// FileContentResult将二进制文件的内容发送到响应
        /// </summary>
        /// <param name="treasureUID">操作的物品对象GUID</param>
        /// <returns></returns>
        public FileContentResult GetImage(Guid treasureUID)
        {
            //根据GUID获取对象
            Treasure trea = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);
            if (trea != null)
            {
                return File(trea.ImageData, trea.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取物品所有者用户信息
        /// </summary>
        /// <param name="holderid">用户ID</param>
        /// <returns></returns>
        public AppUser GetLoginUserInfo(string holderid)
        {
            return UserManager.FindById(holderid);
        }

        /// <summary>
        /// 错误视图
        /// </summary>
        /// <returns></returns>
        public ViewResult Error()
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