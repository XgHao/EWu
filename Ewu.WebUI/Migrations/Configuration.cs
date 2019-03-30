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
            //  �˷�������Ǩ�Ƶ����°汾ʱ����

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            //  �����ʹ��DbSet<T>.AddOrUpdate()���������������ⴴ���ظ�����������

            //Ϊ�µ�����ֵ���Ĭ��ֵ
            //��ȡ�û��ͽ�ɫ�Ĺ�����
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole>(context));

            #region Ĭ���û���Ϣ
            string roleName = "Admin";
            string userName = "XgHao";
            string password = "MySecret";
            string eamil = "zxh@example.com";
            #endregion

            //��ǰ��ɫ��������
            if (!roleMgr.RoleExists(roleName))
            {
                //����Ĭ�Ͻ�ɫ���½�
                roleMgr.Create(new AppRole(roleName));
            }

            //�����û��������û�����
            AppUser user = userMgr.FindByName(userName);
            //������
            if (user == null)
            {
                //�����û�Ĭ����Ϣ����
                userMgr.Create(new AppUser { UserName = userName, Email = eamil }, password);
                user = userMgr.FindByName(userName);
            }

            //��ǰ�û�������Ĭ�ϵĽ�ɫʱ
            if (!userMgr.IsInRole(user.Id, roleName))
            {
                userMgr.AddToRole(user.Id, roleName);
            }

            //������Ե�Ĭ��ֵ
            foreach(AppUser dbUser in userMgr.Users)
            {
                dbUser.Gender = Gender.����;
                dbUser.Signature = "TaʲôҲû���¡�";
                dbUser.RealName = dbUser.UserName + "(�û�����ʱ���)";
                dbUser.Age = 0;
                dbUser.IDCardNO = "δ֪";
            }
            context.SaveChanges();
        }
    }
}
