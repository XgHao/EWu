using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewu.Domain.Entities
{
    public class Favorite
    {
        private List<FavoriteLine> lineCollection = new List<FavoriteLine>();

        public void AddItem(Treasure treasure, int quantity)
        {
            FavoriteLine line = lineCollection
                                              .Where(t => t.Treasure.TreasureUID == treasure.TreasureUID)
                                              .FirstOrDefault();

            if (line == null)
            {
                lineCollection.Add(new FavoriteLine
                {
                    Treasure = treasure,
                    Quantity = quantity
                });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Treasure treasure)
        {
            lineCollection.RemoveAll(l => l.Treasure.TreasureUID == treasure.TreasureUID);
        }

        public decimal ComputeTotalValue()
        {
            decimal price = 1;
            return lineCollection.Sum(e => e.Treasure.BrowseNum * price);
        }

        public void Clear()
        {
            lineCollection.Clear();
        }

        public IEnumerable<FavoriteLine> Lines
        {
            get { return lineCollection; }
        }

        public class FavoriteLine
        {
            public Treasure Treasure { get; set; }
            public int Quantity { get; set; }
        }
    }

    
}
