using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.WebUI.Models.ViewModel;
using Ewu.Domain.Abstract;
using Ewu.Domain.Db;

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

            List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

            int catecount = 0;
            //获取各个分类的总数
            using(var db =new TreasureDataContext())
            {
                //获取所有的分类
                IEnumerable<string> categorise = db.Treasures
                                                           .Select(x => x.TreasureType)
                                                           .Distinct()
                                                           .OrderBy(x => x);

                //保存全部分类的信息
                categoryInfos.Add(new CategoryInfo
                {
                    CateCount = db.Treasures.Count(),
                    CateName = "所有物品",
                    CateLink = "/Treasure/List"
                });

                foreach (var cate in categorise)
                {
                    catecount = db.Treasures.Where(x => x.TreasureType == cate).Count();
                    CategoryInfo categoryInfo = new CategoryInfo
                    {
                        CateName = cate,
                        CateCount = catecount,
                        CateLink = "/Treasure/List/" + cate
                    };
                    categoryInfos.Add(categoryInfo);
                }
            }
            

            IEnumerable<CategoryInfo> AllCateInfos = categoryInfos;

            return PartialView(AllCateInfos);
        }
    }
}