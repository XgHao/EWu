using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Ewu.Domain.Entities;
using Microsoft.Owin;
using Ewu.WebUI.Infrastructure.Identity.Validator;

namespace Ewu.WebUI.Infrastructure.Identity
{
    /// <summary>
    /// 用户管理器，派生于UserManager<T>，其中T是用户类
    /// 用来管理用户实例，可以用以创建用户并对用户数据进行操作
    /// </summary>
    public class AppUserManager : UserManager<AppUser>
    {
        public AppUserManager(IUserStore<AppUser> store) : base(store) { }

        /// <summary>
        /// 因为在对用户数据执行操作时
        /// Identity需要一个AppUserManager的实例时，
        /// 所以会调用该静态的Create方法
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context">IOwinContext用一个泛型Get方法，会返回已经在OWIN启动类中注册的对象实例</param>
        /// <returns></returns>
        public static AppUserManager Create(IdentityFactoryOptions<AppUserManager> options,IOwinContext context)
        {
            //为了创建UserStore<AppUser>实例，需要DbContext(APPIdentityDBContext)实例，这里用过OWIN得到
            AppIdentityDbContext db = context.Get<AppIdentityDbContext>();

            //创建AppUserManager类-需要一个UserStore<T>(DbContext)实例，这里T是用户类
            AppUserManager manager = new AppUserManager(new UserStore<AppUser>(db));

            //添加密码策略
            manager.PasswordValidator = new CustomPasswordValidator
            {
                RequiredLength = 6,                     //密码的最小长度
                RequireNonLetterOrDigit = false,        //密码是否必须含有非字母和数字的字符
                RequireDigit = false,                   //密码是否必须含有数字
                RequireLowercase = true,                //密码是否必须含有小写字母
                RequireUppercase = true                 //密码是否必须含有大写字母
            };

            //添加用户细节验证
            //UserValidator类有一个泛型的类型参数，他指定了用户类的类型，即AppUser；它的构造器参数是用户管理器类，即AppUserManager的一个实例
            manager.UserValidator = new CustomUserValidator(manager)
            {
                AllowOnlyAlphanumericUserNames = true,      //用户名是否值只能含有字母数字字段
                RequireUniqueEmail = true                   //邮件地址是否唯一
            };

            return manager;
        }
    }
}