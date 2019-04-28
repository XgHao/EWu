using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ewu.Domain.Entities
{
    public class Deal
    {
        /// <summary>
        /// 交易唯一标识
        /// </summary>
        [Key]
        public Guid DLogUID { get; set; }

        #region 交易双方及交易物
        /// <summary>
        /// 交易发起者UID
        /// </summary>
        public string TraderSponsorID { get; set; }     
        /// <summary>
        /// 发起者物品UID
        /// </summary>
        public string TreasureSponsorID { get; set; }   
        /// <summary>
        /// 交易接受者UID
        /// </summary>
        public string TraderRecipientID { get; set; }   
        /// <summary>
        /// 接受者物品UID
        /// </summary>
        public string TreasureRecipientID { get; set; }
        #endregion

        #region 交易订单
        /// <summary>
        /// 交易开始时间
        /// </summary>
        public DateTime DealBeginTime { get; set; }     
        /// <summary>
        /// 交易结束时间
        /// </summary>
        public DateTime DealEndTime { get; set; }       
        /// <summary>
        /// 交易状态
        /// </summary>
        public string DealStatus { get; set; }
        #endregion

        #region 备注
        /// <summary>
        /// 发起人对接收人的备注信息
        /// </summary>
        public string RemarkSToR { get; set; }
        /// <summary>
        /// 接收人对发起人的备注信息
        /// </summary>
        public string RemarkRToS { get; set; }
        #endregion

        #region 评价
        /// <summary>
        /// 发起者评分
        /// </summary>
        public decimal ScoreSToR { get; set; }          
        /// <summary>
        /// 接受者评分
        /// </summary>
        public decimal ScoreRToS { get; set; }
        #endregion
    }
}
