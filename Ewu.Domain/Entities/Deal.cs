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
        [Key]
        public Guid DLogUID { get; set; }             //交易唯一标识

        //交易双方及交易物 
        public string TraderSponsorID { get; set; }     //交易发起者ID
        public string TreasureSponsorID { get; set; }   //发起者物品ID
        public string TraderRecipientID { get; set; }   //交易接受者ID
        public string TreasureRecipientID { get; set; } //接受者物品ID

        //交易订单
        public DateTime DealBeginTime { get; set; }     //交易开始时间
        public DateTime DealEndTime { get; set; }       //交易结束时间
        public string DealStatus { get; set; }          //交易状态
        
        //评价
        public decimal ScoreSToR { get; set; }          //发起者评分
        public decimal ScoreRToS { get; set; }          //接受者评分
    }
}
