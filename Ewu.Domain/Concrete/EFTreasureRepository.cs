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

        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="treasureUID">操作物品的UID</param>
        /// <returns></returns>
        public Treasure DeleteTreasure(Guid treasureUID)
        {
            //根据UID获取物品对象
            Treasure dbEntry = context.Treasures.Find(treasureUID);

            //当前对象存在
            if (dbEntry != null)
            {
                context.Treasures.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        /// <summary>
        /// 保存物品
        /// </summary>
        /// <param name="treasure">操作的物品对象</param>
        public void SaveTreasure(Treasure treasure)
        {
            //如果当前物品对象的GUID位空，则新建
            if (treasure.TreasureUID == Guid.Empty)
            {
                treasure.TreasureUID = Guid.NewGuid();
                context.Treasures.Add(treasure);
            }
            //否则就更新数据
            else
            {
                //获取当前操作的物品对象
                Treasure dbEntry = context.Treasures.Find(treasure.TreasureUID);

                //当前对象不为空，更新操作
                if (dbEntry != null)
                {
                    dbEntry.TreasureName = treasure.TreasureName;
                    dbEntry.DetailContent = treasure.DetailContent;
                    dbEntry.BrowseNum = treasure.BrowseNum;
                    dbEntry.Favorite = treasure.Favorite;
                    //图片保存
                    dbEntry.ImageData = treasure.ImageData;
                    dbEntry.ImageMimeType = treasure.ImageMimeType;
                }
            }

            //保存更改
            context.SaveChanges();
        }
    }
}
