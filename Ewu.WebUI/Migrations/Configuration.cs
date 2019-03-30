namespace Ewu.WebUI.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Ewu.Domain.Entities;
    using Ewu.WebUI.Infrastructure.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<Ewu.WebUI.Infrastructure.Identity.AppIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Ewu.WebUI.Infrastructure.Identity.AppIdentityDbContext";
        }

        protected override void Seed(Ewu.WebUI.Infrastructure.Identity.AppIdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  此方法将在迁移到最新版本时调用

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //  你可以使用DbSet<T>.AddOrUpdate()辅助器方法来避免创建重复的种子数据

            //为新的属性值添加默认值
            //获取用户和角色的管理器
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

            //添加属性的默认值
            foreach(AppUser dbUser in userMgr.Users)
            {
                dbUser.Gender = Gender.保密;
                dbUser.Signature = "Ta什么也没留下。";
                dbUser.RealName = dbUser.UserName + "(用户名临时替代)";
                dbUser.Age = 0;
                dbUser.IDCardNO = "未知";
            }
            context.SaveChanges();
        }
    }
}
