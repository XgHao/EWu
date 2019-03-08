using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ewu.Domain.Entities
{
    public class Notice
    {
        [Key]
        public Guid NoticeUID { get; set; }           //通知唯一标识

        public string RecipientID { get; set; }         //接收人ID
        public string SponsorID { get; set; }           //发起人ID
        public string NoticeObject { get; set; }        //通知主题
        public string NoticeContent { get; set; }       //通知内容
        public DateTime NoticeTime { get; set; }        //通知时间
        public bool IsRead { get; set; }             //是否已读
    }
}
