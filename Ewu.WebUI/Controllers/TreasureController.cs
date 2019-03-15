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
            TreasureListViewModel model = new TreasureListViewModel
            {
                Treasures = repository.Treasures
                                    .Where(t => category == null || t.TreasureType == category)
                                    .OrderBy(t => t.TreasureName)
                                    .Skip((page - 1) * PageSize)
                                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItem = category == null
                                          ? repository.Treasures.Count()
                                          : repository.Treasures.Where(e => e.TreasureType == category).Count()
                },
                CurrentCategory = category
            };
            return View(model);
        }

        public FileContentResult GetImage(Guid treasureUID)
        {
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