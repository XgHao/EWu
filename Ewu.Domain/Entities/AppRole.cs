using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ewu.Domain.Entities
{
    /// <summary>
    /// 角色存储
    /// </summary>
    public class AppRole : IdentityRole
    {
        /// <summary>
        /// 构造器，调用基类
        /// </summary>
        public AppRole() : base() { }

        /// <summary>
        /// 构造器，调用基类
        /// </summary>
        /// <param name="name">角色名称</param>
        public AppRole(string name) : base(name) { }
    }
}
