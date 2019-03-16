using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Ewu.WebUI.Infrastructure.Abstract;

namespace Ewu.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider : IAuthProvider
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <returns>验证的结果</returns>
        public bool Authenticate(string username, string password)
        {
            //【已过时】获取验证的结果
            bool result = FormsAuthentication.Authenticate(username, password);

            if (result)
            {
                //是否添加到cookie中
                FormsAuthentication.SetAuthCookie(username, false);
            }

            //返回结果
            return result;
        }
    }
}