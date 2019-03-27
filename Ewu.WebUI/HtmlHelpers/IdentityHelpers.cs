using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Ewu.WebUI.Infrastructure.Identity;

namespace Ewu.WebUI.HtmlHelpers
{
    /// <summary>
    /// 自定义HTML辅助器
    /// </summary>
    public static class IdentityHelpers
    {
        /// <summary>
        /// GetUserName HTML辅助器
        /// </summary>
        /// <param name="html">当前Html</param>
        /// <param name="id">角色ID</param>
        /// <returns>角色名称</returns>
        public static MvcHtmlString GetUserName(this HtmlHelper html,string id)
        {
            AppUserManager mgr = HttpContext.Current.GetOwinContext().GetUserManager<AppUserManager>();
            return new MvcHtmlString(mgr.FindByIdAsync(id).Result.UserName);
        }
    }
}