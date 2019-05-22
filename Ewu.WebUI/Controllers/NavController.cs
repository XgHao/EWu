using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.WebUI.Models.ViewModel;
using Ewu.WebUI.API;
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
        public PartialViewResult Category(string category = null)
        {
            //保存选择的分类
            ViewBag.SelectedCategory = category;

            List<CategoryInfo> categoryInfos = new List<CategoryInfo>();

            int catecount = 0;
            //获取各个分类的总数
            using(var db =new TreasureDataContext())
            {
                var treas = db.Treasures.Where(t => (category == null || t.TreasureType == category) && (t.Cover != null && t.DetailPic != null) && (t.DLogUID == null));

                //获取所有的分类
                IEnumerable<string> categorise = treas
                                    .Select(x => x.TreasureType)
                                    .Distinct()
                                    .OrderBy(x => x);

                //保存全部分类的信息
                categoryInfos.Add(new CategoryInfo
                {
                    CateCount = treas.Count(),
                    CateName = "所有物品",
                    CateLink = "/Treasure/List"
                });

                foreach (var cate in categorise)
                {
                    catecount = treas.Where(x => x.TreasureType == cate).Count();
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

        /// <summary>
        /// 排序分部视图
        /// </summary>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public PartialViewResult Orderby(string orderby = null)
        {
            //保存选择的排序
            ViewBag.SelectedOrderby = orderby;

            //获取所有可供排序显示
            Dictionary<string, string> orderbys = new Dictionary<string, string>();

            //orderbys.Add("Favorite", "收藏数 ↑") ;
            //orderbys.Add("Favorite", "收藏数 ↓") ;
            //orderbys.Add("BrowseNum", "浏览量 ↑") ;
            //orderbys.Add("BrowseNum", "浏览量 ↓") ;
            //orderbys.Add("UploadTime", "上传时间 ↑");
            //orderbys.Add("UploadTime", "上传时间 ↓");
            //orderbys.Add("UpdateTime", "更新时间 ↑");
            //orderbys.Add("UpdateTime", "更新时间 ↓");

            orderbys.Add("收藏数（正序）", "↑");
            orderbys.Add("收藏数（倒序）", "↓");
            orderbys.Add("浏览量（正序）", "↑");
            orderbys.Add("浏览量（倒序）", "↓");
            orderbys.Add("上传时间（正序）", "↑");
            orderbys.Add("上传时间（倒序）", "↓");
            orderbys.Add("更新时间（正序）", "↑");
            orderbys.Add("更新时间（倒序）", "↓");

            return PartialView(orderbys);
        }
    }
}