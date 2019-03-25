using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Ewu.WebUI.Infrastructure.Identity;

namespace Ewu.WebUI.App_Start
{
    /// <summary>
    /// OWIN启动项为了加载和配置中间件，并执行所需要的其他配置工作
    /// </summary>
    public class IdentityConfig
    {
        /// <summary>
        /// 该方法会被OWIN基础架构调用，由它支持应用程序所需中间件的设置
        /// </summary>
        /// <param name="app">IAppBuilder接口是由一些扩展方法提供的，这些扩展方法的定义在OWIN命名空间的一些类中</param>
        public void Configuration(IAppBuilder app)
        {
            //CreatePerOwinContext用于创建AppUserManager的新实例和APPIdentityDbContext类用于每一个请求
            app.CreatePerOwinContext<AppIdentityDbContext>(AppIdentityDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(AppUserManager.Create);

            //该方法告诉ASP.NET Identity如何用Cookie去标识已认证的用户
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login")
            });
        }
    }
}