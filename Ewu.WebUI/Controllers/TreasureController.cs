using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.WebUI.Models;
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

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="treasuresRepository">传入储存库</param>
        public TreasureController(ITreasuresRepository treasuresRepository)
        {
            repository = treasuresRepository;
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
        /// 发布新的物品
        /// </summary>
        /// <returns></returns>
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

            ViewData["Type"] = types;
            #endregion

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