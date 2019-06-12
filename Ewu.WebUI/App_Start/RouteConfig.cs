using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Ewu.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //当URL为空时的默认值
            routes.MapRoute(
                name: null,
                url: "",
                //默认值-主页
                defaults: new
                {
                    controller = "Treasure",
                    action = "Index",
                    //category = (string)null,
                    //page = 1
                }
            );

            //当URL只有页面参数时
            routes.MapRoute(
                name: null,
                url: "Treasure/List/Page{page}",
                //默认值-物品详情第page页，全部分类
                defaults: new
                {
                    controller = "Treasure",
                    action = "List",
                    category = (string)null
                },
                //限制page参数为数值
                constraints: new
                {
                    page = @"\d+"
                }
            );

            //当URL只有分类的参数时
            routes.MapRoute(
                name: null,
                url: "Treasure/List/{category}",
                //默认值-category类物品的第一页
                defaults: new
                {
                    controller = "Treasure",
                    action = "List",
                    page = 1
                }
            );

            //当URL既有分类和分页时
            routes.MapRoute(
                name: null,
                url: "Treasure/List/{category}/Page{page}",
                //默认值-category类物品的第page页
                defaults: new
                {
                    controller = "Treasure",
                    action = "List"
                },
                constraints: new
                {
                    page = @"\d+"
                }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Treasure", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}
