using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 管理员控制器
    /// </summary>
    [Authorize]     //启动验证
    public class AdminController : Controller
    {
        //物品存储库
        private ITreasuresRepository repository;

        //构造函数，传递存储库
        public AdminController(ITreasuresRepository repo)
        {
            repository = repo;
        }

        // 显示主页
        public ActionResult Index()
        {
            return View(repository.Treasures);
        }


        /// <summary>
        /// 编辑物品
        /// </summary>
        /// <param name="treasureUID">物品的UID</param>
        /// <returns></returns>
        public ViewResult Edit(Guid treasureUID)
        {
            //根据UID获取物品对象
            Treasure treasure = repository.Treasures.FirstOrDefault(t => t.TreasureUID == treasureUID);
            return View(treasure);
        }

        /// <summary>
        /// 编辑物品[HttpPost]
        /// </summary>
        /// <param name="treasure">当前操作的物品对象</param>
        /// <param name="image">物品图片</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Treasure treasure, HttpPostedFileBase image = null)
        {
            //如果模型验证没有错误
            if (ModelState.IsValid)
            {
                //有无上传的图片
                if (image != null)
                {
                    //文件的MimeType
                    treasure.ImageMimeType = image.ContentType;
                    //图片数据
                    treasure.ImageData = new byte[image.ContentLength];
                    //数据以二进制的形势写入到流中
                    image.InputStream.Read(treasure.ImageData, 0, image.ContentLength);
                }

                //保存数据
                repository.SaveTreasure(treasure);
                //临时数据(TempData)保存消息，在HTTP请求结束后会被删除
                //ViewBag不能用于跨请求情况下控制器与视图之间的数据传递，因为这里用的是重定向
                TempData["message"] = string.Format("{0} has been saved", treasure.TreasureName);
                //页面重定向到Index
                return RedirectToAction("Index");
            }
            else
            {
                return View(treasure);
            }
        }

        /// <summary>
        /// 创建新物品-使用Edit页面
        /// </summary>
        /// <returns></returns>
        public ViewResult Create()
        {
            return View("Edit", new Treasure());
        }

        /// <summary>
        /// 删除物品[HttpPost]
        /// </summary>
        /// <param name="treasureUID">操作对象的GUID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(Guid treasureUID)
        {
            //根据GUID获取对象
            Treasure deletedTreasure = repository.DeleteTreasure(treasureUID);

            //对象不为空
            if (deletedTreasure != null)
            {
                TempData["message"] = string.Format("{0} was deleted", deletedTreasure.TreasureName);
            }

            //重定向到Index
            return RedirectToAction("Index");
        }
    }
}