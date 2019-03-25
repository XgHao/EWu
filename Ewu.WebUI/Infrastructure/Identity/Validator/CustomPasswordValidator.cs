using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace Ewu.WebUI.Infrastructure.Identity.Validator
{
    /// <summary>
    /// 自定义的密码验证器
    /// </summary>
    public class CustomPasswordValidator : PasswordValidator
    {
        /// <summary>
        /// 重写验证方法
        /// </summary>
        /// <param name="passwd">要验证的密码</param>
        /// <returns></returns>
        public override async Task<IdentityResult> ValidateAsync(string passwd)
        {
            IdentityResult result = await base.ValidateAsync(passwd);
            if (passwd.Contains("123"))
            {
                var errors = result.Errors.ToList();
                errors.Add("密码不能包含123");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}