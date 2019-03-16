using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Abstract;

namespace Ewu.WebUI.Controllers
{
    /// <summary>
    /// 导航控制器
    /// </summary>
    public class NavController : Controller
    {
        //物品存储库
        private ITreasuresRepository repository;

        //构造函数-传递存储库
        public NavController(ITreasuresRepository repo)
        {
            repository = repo;
        }

        /// <summary>
        /// 导航分部视图
        /// </summary>
        /// <param name="category">分类</param>
        /// <returns>分部视图</returns>
        public PartialViewResult Menu(string category = null)
        {
            //保存选择的分类
            ViewBag.SelectedCategory = category;

            //获取所有的分类
            IEnumerable<string> categorise = repository.Treasures
                                                       .Select(x => x.TreasureType)
                                                       .Distinct()
                                                       .OrderBy(x => x);
            return PartialView(categorise);
        }
    }
}