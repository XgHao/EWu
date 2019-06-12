using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ewu.WebUI.Models.ViewModel
{
    public class IndexViewModel
    {
        /// <summary>
        /// 随便看看
        /// </summary>
        public IEnumerable<TreasureAndHolderInfo> RandomTrea { get; set; }

        /// <summary>
        /// 最新物品
        /// </summary>
        public IEnumerable<TreasureAndHolderInfo> NewestTrea { get; set; }

        /// <summary>
        /// 最近热门
        /// </summary>
        public IEnumerable<TreasureAndHolderInfo> HotTrea { get; set; }

        /// <summary>
        /// 推荐物品
        /// </summary>
        public IEnumerable<TreasureAndHolderInfo> RecommandTrea { get; set; }

        /// <summary>
        /// 物品数
        /// </summary>
        public int TreasureCnt { get; set; }

        /// <summary>
        /// 交易中的物品
        /// </summary>
        public int DealingCnt { get; set; }

        /// <summary>
        /// 评价数
        /// </summary>
        public int EvaluationCnt { get; set; }

        /// <summary>
        /// 用户数
        /// </summary>
        public int UserCnt { get; set; }

    }
}