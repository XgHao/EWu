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
                defaults: new
                {
                    controller = "Treasure",
                    action = "List",
                    category = (string)null,
                    page = 1
                }
            );

            //当URL只有页面参数时
            routes.MapRoute(
                name: null,
                url: "Page{page}",
                defaults: new
                {
                    controller = "Treasure",
                    action = "List",
                    category = (string)null
                },
                constraints: new
                {
                    page = @"\d+"
                }
            );

            //当URL只有分类的参数时
            routes.MapRoute(
                name: null,
                url: "{category}",
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
                url: "{category}/Page{page}",
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
