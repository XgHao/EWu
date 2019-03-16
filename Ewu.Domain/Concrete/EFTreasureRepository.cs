using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;

namespace Ewu.Domain.Concrete
{
    /// <summary>
    /// 存储库类，实现ITreasureRepository接口
    /// </summary>
    public class EFTreasureRepository : ITreasuresRepository
    {
        //一个EFDBContext实例，从数据库接受数据
        private EFDBContext context = new EFDBContext();

        /// <summary>
        /// 实现接口中的方法
        /// 赋予Treasures模型类
        /// </summary>
        public IEnumerable<Treasure> Treasures
        {
            get { return context.Treasures; }
        }

        public Treasure DeleteTreasure(Guid treasureUID)
        {
            Treasure dbEntry = context.Treasures.Find(treasureUID);
            if (dbEntry != null)
            {
                context.Treasures.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        public void SaveTreasure(Treasure treasure)
        {
            if (treasure.TreasureUID == Guid.Empty)
            {
                treasure.TreasureUID = Guid.NewGuid();
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
                    dbEntry.ImageData = treasure.ImageData;
                    dbEntry.ImageMimeType = treasure.ImageMimeType;
                }
            }
            context.SaveChanges();
        }
    }
}
