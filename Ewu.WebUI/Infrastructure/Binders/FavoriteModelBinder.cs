using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Ewu.Domain.Entities;


namespace Ewu.WebUI.Infrastructure.Binders
{
    public class FavoriteModelBinder : IModelBinder
    {
        private const string sessionKey = "Favorite";

        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            //通过会话来获取Favorite
            Favorite favorite = null;
            if (controllerContext.HttpContext.Session != null)
            {
                favorite = (Favorite)controllerContext.HttpContext.Session[sessionKey];
            }

            //若会话中没有Favorite，则创建一个
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