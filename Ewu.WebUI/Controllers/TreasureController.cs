using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.WebUI.Models;
using Ewu.WebUI.Infrastructure.Abstract;
using Ewu.WebUI.Models.ViewModel;
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
        /// 物品列表
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="page">页码</param>
        /// <returns></returns>
        [Authorize]
        public ViewResult List(string category, int page = 1, int PageSize = 3)
        {
            //生成一个具体的列表视图模型
            TreasureListViewModel model = new TreasureListViewModel
            {
                //根据页码以及分类来确定具体要显示的物品列表
                Treasures = repository.Treasures
                                    .Where(t => category == null || t.TreasureType == category)
                                    .OrderBy(t => t.TreasureName)
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize),
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
        /// 个人物品页面
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public ActionResult MyList(string category, int page = 1, int PageSize = 3)
        {
            //生成一个具体的列表视图模型
            TreasureListViewModel model = new TreasureListViewModel
            {
                //根据页码以及分类来确定具体要显示的物品列表
                Treasures = repository.Treasures
                                    .Where(t => category == null || t.TreasureType == category)
                                    .OrderBy(t => t.TreasureName)
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize),
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
                new SelectListItem(){ Text="其他",Value="其他",Selected=true }
            };

            Session["Types"] = types;
            #endregion

            #region 物品成色集合
            IEnumerable<SelectListItem> damageDegree = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="完好",Value="网络设备",Selected=true },
                new SelectListItem(){ Text="九五新",Value="九五新" },
                new SelectListItem(){ Text="九成新",Value="九成新" },
                new SelectListItem(){ Text="八五新",Value="八五新" },
                new SelectListItem(){ Text="八成新",Value="八成新" },
                new SelectListItem(){ Text="七五新",Value="七五新" },
                new SelectListItem(){ Text="七成新",Value="七成新" },
                new SelectListItem(){ Text="六成及以下",Value="六成及以下" },
            };

            Session["DamageDegrees"] = damageDegree;
            #endregion

            #region 物品交易范围集合
            IEnumerable<SelectListItem> tradeRange = new List<SelectListItem>()
            {
                new SelectListItem(){ Text="市内",Value="市内" },
                new SelectListItem(){ Text="省内",Value="省内" },
                new SelectListItem(){ Text="临近省",Value="临近省" },
                new SelectListItem(){ Text="全国(港澳台除外)",Value="全国" },
                new SelectListItem(){ Text="不限",Value="不限",Selected=true }
            };

            Session["TradeRanges"] = tradeRange;
            #endregion

            Treasure treasure = new Treasure()
            {
                TreasureUID = Guid.NewGuid(),
                HolderID = CurrentUser.Id,
            };
            return View(treasure);
        }

        [HttpPost]
        public ActionResult UploadItem(Treasure treasure)
        {
            if (ModelState.IsValid)
            {
                //保存数据库

            }
            return View(treasure);
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








        //public ActionResult SaveUploadedFile()
        //{
        //    bool isSavedSuccessfully = true;
        //    string fName = "";
        //    foreach(var fileName in Request.Files)
        //    {
        //        HttpPostedFileBase file = Request.Files[fileName];
        //        //保存文件
        //        fName = file.FileName;
        //        if (file != null && file.ContentLength > 0)
        //        {
        //        }
        //    }
        //}
    }
}