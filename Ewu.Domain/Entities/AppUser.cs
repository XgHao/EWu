using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ewu.Domain.Entities
{
    /// <summary>
    /// 用户类派生于IdentityUser基础用户类
    /// </summary>
    public class AppUser : IdentityUser
    {
        //集成IdentityUser中原有的属性
    }
}
