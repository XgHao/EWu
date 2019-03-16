using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewu.Domain.Entities
{
    /// <summary>
    /// 收藏的物品
    /// </summary>
    public class Favorite
    {
        //收藏物品详情(收藏物品+数量)
        private List<FavoriteLine> lineCollection = new List<FavoriteLine>();

        /// <summary>
        /// 添加收藏
        /// </summary>
        /// <param name="treasure">收藏的物品对象</param>
        /// <param name="quantity">数量</param>
        public void AddItem(Treasure treasure, int quantity)
        {
            //获取当前物品的详情
            FavoriteLine line = lineCollection
                                              .Where(t => t.Treasure.TreasureUID == treasure.TreasureUID)
                                              .FirstOrDefault();

            //如果不存在就添加
            if (line == null)
            {
                lineCollection.Add(new FavoriteLine
                {
                    Treasure = treasure,
                    Quantity = quantity
                });
            }
            //如果存在则数量+1
            else
            {
                line.Quantity += quantity;
            }
        }

        /// <summary>
        /// 取消收藏
        /// </summary>
        /// <param name="treasure">要移除收藏的物品对象</param>
        public void RemoveLine(Treasure treasure)
        {
            lineCollection.RemoveAll(l => l.Treasure.TreasureUID == treasure.TreasureUID);
        }

        /// <summary>
        /// 计算收藏物品的总价
        /// </summary>
        /// <returns>返回收藏物品的总价</returns>
        public decimal ComputeTotalValue()
        {
            decimal price = 1;
            return lineCollection.Sum(e => e.Treasure.BrowseNum * price);
        }

        /// <summary>
        /// 清空收藏夹
        /// </summary>
        public void Clear()
        {
            lineCollection.Clear();
        }

        //可序列化的收藏物品列
        public IEnumerable<FavoriteLine> Lines
        {
            get { return lineCollection; }
        }

        //收藏物品详情
        public class FavoriteLine
        {
            public Treasure Treasure { get; set; }
            public int Quantity { get; set; }
        }
    }

    
}
