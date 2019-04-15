using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.WebUI.Models.ViewModel;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 收藏列表控制器
    /// </summary>
    public class FavoriteController : Controller
    {
        //收藏物品的存储库
        private ITreasuresRepository repository;
        //订单
        private IOrderProcessor orderProcessor;

        //构造函数-传递存储库
        public FavoriteController(ITreasuresRepository repo, IOrderProcessor proc)
        {
            repository = repo;
            orderProcessor = proc;
        }

        /// <summary>
        /// 主页列表显示
        /// </summary>
        /// <param name="favorite">收藏物品</param>
        /// <param name="returnUrl">返回的Url</param>
        /// <returns></returns>
        public ViewResult Index(Favorite favorite, string returnUrl)
        {
            return View(new FavoriteIndexViewModel
            {
                Favorite = favorite,
                ReturnUrl = returnUrl
            });
        }

        /// <summary>
        /// 添加到收藏列表
        /// </summary>
        /// <param name="favorite">收藏的物品对象</param>
        /// <param name="treasureUID">收藏物品的UID</param>
        /// <param name="returnUrl">返回的Url</param>
        /// <returns>重定向浏览器，浏览一个新的URL</returns>
        public RedirectToRouteResult AddToFavorite(Favorite favorite, Guid treasureUID, string returnUrl)
        {
            //获取当前UID的物品对象
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);

            //如果物品对象存在，则添加收藏
            if (treasure != null)
            {
                favorite.AddItem(treasure, 1);
            }
            return RedirectToAction(
                    actionName: "Index",
                    routeValues: new
                    {
                        returnUrl
                    }
                );
        }

        /// <summary>
        /// 从收藏夹中删除
        /// </summary>
        /// <param name="favorite">要移除的物品对象</param>
        /// <param name="treasureUID">移除物品的UID</param>
        /// <param name="returnUrl">返回的Url</param>
        /// <returns>重定向浏览器，浏览一个新的URL</returns>
        public RedirectToRouteResult RemoveFromFavorite(Favorite favorite, Guid treasureUID,string returnUrl)
        {
            //获取当前UID的物品对象
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);

            //如果物品存在，则移除
            if (treasure != null)
            {
                favorite.RemoveLine(treasure);
            }
            return RedirectToAction(
                    actionName: "Index",
                    routeValues: new
                    {
                        returnUrl
                    }
                );
        }


        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        /// <summary>
        /// 订单发送
        /// </summary>
        /// <param name="favorite">收藏页</param>
        /// <param name="shippingDetails">详情发送信息</param>
        /// <returns></returns>
        [HttpPost]
        public ViewResult Checkout(Favorite favorite, ShippingDetails shippingDetails)
        {
            if (favorite.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, Your favorite is empty");
            }
            //验证模型，如果无误
            if (ModelState.IsValid)
            {
                orderProcessor.ProcessOrder(favorite, shippingDetails);
                favorite.Clear();
                return View("Completed");
            }
            else
            {
                return View(shippingDetails);
            }
        }

        /// <summary>
        /// 顶部摘要
        /// </summary>
        /// <param name="favorite">收藏详情对象</param>
        /// <returns></returns>
        public PartialViewResult Summary(Favorite favorite)
        {
            return PartialView(favorite);
        }

        /// <summary>
        /// 存储和接受Favorite对象
        /// 现已抛弃，被模型绑定(ModelBinder)取代
        /// Infrastructure\Binders\FavoriteModelBinder.cs
        /// </summary>
        /// <returns></returns>
        private Favorite GetFavorite()
        {
            Favorite favorite = (Favorite)Session["Favorite"];

            //如果当前没有favorite对象，则存储一个新对象
            if (favorite == null)
            {
                favorite = new Favorite();
                Session["Favorite"] = favorite;
            }

            return favorite;
        }
    }
}