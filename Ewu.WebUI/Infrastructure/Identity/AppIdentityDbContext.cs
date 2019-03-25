using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;
using Ewu.Domain.Entities;

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
        }
    }
}