using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ewu.Domain.Entities;

namespace Ewu.Domain.Abstract
{
    //物品的抽象存储库
    public interface ITreasuresRepository
    {
        //可序列化的物品集合
        IEnumerable<Treasure> Treasures { get; }

        /// <summary>
        /// 保存物品
        /// </summary>
        /// <param name="treasure">操作的物品对象</param>
        void SaveTreasure(Treasure treasure);

        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="treasureUID">操作物品的UID</param>
        /// <returns>删除物品对象</returns>
        Treasure DeleteTreasure(Guid treasureUID);
    }
}
