using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ewu.Domain.Entities;

namespace Ewu.Domain.Abstract
{
    public interface ITreasuresRepository
    {
        IEnumerable<Treasure> Treasures { get; }

        void SaveTreasure(Treasure treasure);
    }
}
