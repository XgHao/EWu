using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.WebUI.Models;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 物品控制器
    /// </summary>
    public class TreasureController : Controller
    {
        private ITreasuresRepository repository;    //定义的物品储存库
        public int PageSize = 2;                    //每页显示数

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
        public ViewResult List(string category, int page = 1)
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
                CurrentCategory = category
            };
            return View(model);
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
    }
}