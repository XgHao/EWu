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

        //基本信息
        //public string UserName { get; set; }        //用户名
        public string HeadPortrait { get; set; }    //头像
        public string Gender { get; set; }          //性别
        public string Signature { get; set; }       //个性签名

        //个人信息
        public string RealName { get; set; }        //真实姓名
        public DateTime BirthDay { get; set; }      //出生年月
        public int Age { get; set; }                //年龄
        public string IDCardNO { get; set; }        //身份证号码
        public string NativePlace { get; set; }     //籍贯
        //public string PhoneNumber { get; set; }     //联系方式
        //public string EMail { get; set; }            //邮箱
        //public string PasswordHash { get; set; }        //登录密码
        public string OICQ { get; set; }            //QQ
        public string WeChat { get; set; }          //微信
        public string Job { get; set; }             //工作职业

        //个人设置
        public DateTime RegisterTime { get; set; }  //注册时间
        public string IsShowInfo { get; set; }      //是否显示个人信息

        //信誉值
        public decimal CreditWorthiness { get; set; }   //信誉值
        public decimal TempDeductionValue { get; set; } //临时扣除值(默认值0)
        public decimal MultipleDeduct { get; set; }     //减分的倍数(默认值1)
        public int PenaltyTime { get; set; }            //临时扣除的天数

        //社交
        public int Notice { get; set; }             //未读的通知数
        public int Favorite { get; set; }           //收藏数

        //身份证照
        public byte[] IDCardImageData { get; set; }
        public string IDCardImageMimeType { get; set; }
    }
}
