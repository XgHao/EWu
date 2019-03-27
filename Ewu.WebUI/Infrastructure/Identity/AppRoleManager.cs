using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Infrastructure.Identity
{
    /// <summary>
    /// 角色管理器派生于RoleManager<T>，其中T是IRole接口的实现
    /// Entity Framework实现了IRole接口，使用的是一个名称为IdentityRole的类
    /// </summary>
    public class AppRoleManager : RoleManager<AppRole>, IDisposable
    {
        public AppRoleManager(RoleStore<AppRole> store) : base(store) { }

        /// <summary>
        /// 该方法能让OWIN启动类能够为每一个访问Identity数据的请求创建实例
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static AppRoleManager Create(IdentityFactoryOptions<AppRoleManager> options, IOwinContext context)
        {
            return new AppRoleManager(new RoleStore<AppRole>(context.Get<AppIdentityDbContext>()));
        }
    }
}