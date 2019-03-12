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

        public void SaveTreasure(Treasure treasure)
        {
            if (treasure.TreasureUID.ToString() == "0")
            {
                context.Treasures.Add(treasure);
            }
            else
            {
                Treasure dbEntry = context.Treasures.Find(treasure.TreasureUID);
                if (dbEntry != null)
                {
                    dbEntry.TreasureName = treasure.TreasureName;
                    dbEntry.DetailContent = treasure.DetailContent;
                    dbEntry.BrowseNum = treasure.BrowseNum;
                    dbEntry.Favorite = treasure.Favorite;
                }
            }
            context.SaveChanges();
        }
    }
}
