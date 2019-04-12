using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Infrastructure.Identity.Validator
{
    /// <summary>
    /// 用户自定义验证器
    /// </summary>
    public class CustomUserValidator : UserValidator<AppUser>
    {
        /// <summary>
        /// 该构造器必须以用户管理器实例为参数，并调用基实现才能执行内建的验证检查
        /// </summary>
        /// <param name="mgr"></param>
        public CustomUserValidator(AppUserManager mgr) : base(mgr) { }

        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="user">AppUser用户对象</param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(AppUser user)
        {
            IdentityResult result = await base.ValidateAsync(user);
            
            return result;
        }
    }
}