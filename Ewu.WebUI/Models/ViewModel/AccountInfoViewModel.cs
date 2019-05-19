using Ewu.Domain.Entities;
using System;
using System.Collections.Generic;
using Ewu.Domain.Db;
using System.Linq;
using System.Web;

namespace Ewu.WebUI.Models.ViewModel
{
    public class AccountInfoViewModel
    {
        //目标用户
        public AppUser TargetUser { get; set; }

        /// <summary>
        /// 总浏览量
        /// </summary>
        public int TotalBrowseNum { get; set; }

        /// <summary>
        /// 总收藏数
        /// </summary>
        public int TotalFavorite { get; set; }

        /// <summary>
        /// 发布的物品数
        /// </summary>
        public int TotalTreasureNum { get; set; }

        /// <summary>
        /// Ta的物品集合
        /// </summary>
        public IEnumerable<TreasureCard> TargetTreasures { get; set; }

        /// <summary>
        /// Ta的收藏
        /// </summary>
        public IEnumerable<TreasureCard> TargetFavorite { get; set; }

        /// <summary>
        /// 评价
        /// </summary>
        public IEnumerable<UserEvaluation> Evaluations { get; set; }
    }

    /// <summary>
    /// 物品卡片模型
    /// </summary>
    public class TreasureCard
    {
        /// <summary>
        /// 物品对象
        /// </summary>
        public Treasure Treasure { get; set; }

        /// <summary>
        /// 所属人
        /// </summary>
        public AppUser TreasureHolder { get; set; }
    }

    /// <summary>
    /// 用户收到的评价
    /// </summary>
    public class UserEvaluation
    {
        /// <summary>
        /// 是否推荐
        /// </summary>
        public bool IsReaommend { get; set; }

        /// <summary>
        /// 评价信息
        /// </summary>
        public string EvaluationInfo { get; set; }

        /// <summary>
        /// 评价时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 评论人
        /// </summary>
        public BasicUserInfo Holder { get; set; }
    }
}