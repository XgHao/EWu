using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Ewu.Domain.Entities
{



    /// <summary>
    /// 用户类派生于IdentityUser基础用户类
    /// </summary>
    public class AppUser : IdentityUser
    {
        #region 集成IdentityUser中原有的属性
        //Id-用户唯一标识
        //UserName-用户名称
        //Email-邮箱
        //EmailConfirmed-邮箱确认
        //PasswordHash-密码哈希值
        //SecurityStamp-安全标记，在用户凭据相关的内容更改时，必须修改该值
        //PhoneNumber-手机号
        //PhoneNumberConfirmed-手机号确认
        //TwoFactorEnabled-是否开启双因子验证
        //LockoutEnabled-只是这个用户可不可以被锁定
        //LockoutEndDateUtc-指定锁定的到期日期
        //AccessFailedCount-记录用户登录失败的次数
        #endregion


        #region 基本信息
        /// <summary>
        /// 头像
        /// </summary>
        public string HeadPortrait { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }          
        /// <summary>
        /// 个性签名
        /// </summary>
        public string Signature { get; set; }       
        #endregion


        #region 个人信息
        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }       
        /// <summary>
        /// 出生年月
        /// </summary>
        public DateTime BirthDay { get; set; }      
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }              
        /// <summary>
        /// 身份号码
        /// </summary>
        public string IDCardNO { get; set; }        
        /// <summary>
        /// 籍贯
        /// </summary>
        public string NativePlace { get; set; }     
        //public string PhoneNumber { get; set; }     //联系方式
        //public string EMail { get; set; }            //邮箱
        //public string PasswordHash { get; set; }        //登录密码
        /// <summary>
        /// QQ
        /// </summary>
        public string OICQ { get; set; }           
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }         
        /// <summary>
        /// 工作职业
        /// </summary>
        public string Job { get; set; }            
        #endregion


        #region 个人设置
        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }  
        /// <summary>
        /// 是否显示个人信息
        /// </summary>
        public string IsShowInfo { get; set; }     
        #endregion


        #region 信誉值
        /// <summary>
        /// 信誉值
        /// </summary>
        public decimal CreditWorthiness { get; set; }  
        /// <summary>
        /// 临时扣除值(默认值为0)
        /// </summary>
        public decimal TempDeductionValue { get; set; } 
        /// <summary>
        /// 减分的倍数(默认值为1)
        /// </summary>
        public decimal MultipleDeduct { get; set; }     
        /// <summary>
        /// 临时扣除的天数
        /// </summary>
        public int PenaltyTime { get; set; }          
        #endregion


        #region 社交
        /// <summary>
        /// 未读的通知数
        /// </summary>
        public int Notice { get; set; }            
        /// <summary>
        /// 收藏数
        /// </summary>
        public int Favorite { get; set; }         
        #endregion


        #region 身份证照
        /// <summary>
        /// 身份证数据
        /// </summary>
        public byte[] IDCardImageData { get; set; }
        /// <summary>
        /// 身份证格式
        /// </summary>
        public string IDCardImageMimeType { get; set; }
        #endregion
    }
}
