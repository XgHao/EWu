using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Entities;


namespace Ewu.WebUI.Infrastructure.Binders
{
    //收藏夹的模型绑定
    public class FavoriteModelBinder : IModelBinder
    {
        //常量sessionKey关键字
        private const string sessionKey = "Favorite";

        /// <summary>
        /// 实现内置方法
        /// 当MVC框架接收到一个请求后，会首先考查动作方法的参数
        /// 然后考查可用的绑定器列表，尝试去找到一个能够为各个参数类型创建实例的绑定器
        /// </summary>
        /// <param name="controllerContext">控制器上下文（可以访问控制器类所具有的全部信息）</param>
        /// <param name="bindingContext">模型绑定上下文（能提供给“要求你建立的模型对象”）</param>
        /// <returns>收藏物品对象</returns>
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //通过会话来获取Favorite
            Favorite favorite = null;

            //如果当前页存在Session对象，则获取
            if (controllerContext.HttpContext.Session != null)
            {
                favorite = (Favorite)controllerContext.HttpContext.Session[sessionKey];
            }

            //若会话中没有Favorite即favorite为空，则创建一个
            if (favorite == null)
            {
                favorite = new Favorite();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = favorite;
                }
            }

            //返回Favorite
            return favorite;
        }
    }
}