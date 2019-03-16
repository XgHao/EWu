using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewu.WebUI.Infrastructure.Abstract
{
    /// <summary>
    /// 该接口作为FormsAuthentication类中静态方法的封装程序
    /// </summary>
    public interface IAuthProvider
    {
        bool Authenticate(string username, string password);
    }
}
