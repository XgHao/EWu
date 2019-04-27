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
        /// <summary>
        /// 唯一通知标识
        /// </summary>
        [Key]
        public Guid NoticeUID { get; set; }           

        /// <summary>
        /// 接收人UID
        /// </summary>
        public string RecipientID { get; set; }         
        /// <summary>
        /// 发起人UID
        /// </summary>
        public string SponsorID { get; set; }           
        /// <summary>
        /// 通知主题
        /// </summary>
        public string NoticeObject { get; set; }       
        /// <summary>
        /// 通知内容
        /// </summary>
        public string NoticeContent { get; set; }      
        /// <summary>
        /// 通知时间
        /// </summary>
        public DateTime NoticeTime { get; set; }        
        /// <summary>
        /// 是否已读
        /// </summary>
        public bool IsRead { get; set; }           
    }
}
