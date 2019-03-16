using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ewu.Domain.Entities;
using System.Data.Entity;

namespace Ewu.Domain.Concrete
{
    /// <summary>
    /// 创建一个关于物品的上下文类
    /// 关联模型与数据库
    /// </summary>
    public class EFDBContext : DbContext
    {
        /// <summary>
        /// DbSet模型类型
        /// Treasures为属性名即数据库中的表名
        /// </summary>
        public DbSet<Treasure> Treasures { get; set; }
    }
}
