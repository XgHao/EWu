using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;

namespace Ewu.Domain.Concrete
{
    public class EFTreasureRepository : ITreasuresRepository
    {
        private EFDBContext context = new EFDBContext();

        public IEnumerable<Treasure> Treasures
        {
            get { return context.Treasures; }
        }
    }
}
