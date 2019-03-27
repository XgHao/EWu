using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ewu.Domain.Entities;
using Microsoft.AspNet.Identity;

namespace Ewu.WebUI.Infrastructure.Identity
{
    /// <summary>
    /// 该数据库上下文派生于IdentityDbContext<T>，其中T是用户类(AppUser.cs)
    /// 用于对AppUser类进行操作，可以用Code First特性来创建和管理数据库架构，并对数据库中的数据进行访问
    /// </summary>
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        /// <summary>
        /// 构造器调用基类，参数为连接数据库的字符串，用于与数据库连接
        /// </summary>
        public AppIdentityDbContext() : base("EFDBContext") { }

        /// <summary>
        /// 静态构造器
        /// </summary>
        static AppIdentityDbContext()
        {
            //使用Database.SetInitializer方法指定一个种植数据库类
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }

        /// <summary>
        /// 该类用于OWIN在必要时创建类实例
        /// </summary>
        /// <returns></returns>
        public static AppIdentityDbContext Create()
        {
            //返回新实例
            return new AppIdentityDbContext();
        }
    }

    /// <summary>
    /// 种植类-在Entity Framework的Code First特性第一次创建数据库架构使用
    /// </summary>
    public class IdentityDbInit : DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        /// <summary>
        /// 种植数据库
        /// </summary>
        /// <param name="context"></param>
        protected override void Seed(AppIdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        /// <summary>
        /// 数据库初始化配置
        /// </summary>
        /// <param name="context"></param>
        public void PerformInitialSetup(AppIdentityDbContext context)
        {
            //数据初始化操作
            //新建管理器
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            #region 默认用户信息
            string roleName = "Admin";
            string userName = "XgHao";
            string password = "MySecret";
            string eamil = "zxh@example.com";
            #endregion

            //当前角色名不存在
            if (!roleMgr.RoleExists(roleName))
            {
                //根据默认角色名新建
                roleMgr.Create(new AppRole(roleName));
            }

            //根据用户名查找用户对象
            AppUser user = userMgr.FindByName(userName);
            //不存在
            if (user == null)
            {
                //根据用户默认信息创建
                userMgr.Create(new AppUser { UserName = userName, Email = eamil }, password);
                user = userMgr.FindByName(userName);
            }

            //当前用户不存在默认的角色时
            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }
        }
    }
}