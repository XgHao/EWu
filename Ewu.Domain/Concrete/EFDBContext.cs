using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ewu.Domain.Entities;
using System.Data.Entity;

namespace Ewu.Domain.Concrete
{
    public class EFDBContext : DbContext
    {
        public DbSet<Treasure> Treasures { get; set; }
    }
}
