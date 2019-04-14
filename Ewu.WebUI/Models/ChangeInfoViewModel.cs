using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Models
{
    /// <summary>
    /// 更改信息模型
    /// </summary>
    public class ChangeInfoViewModel
    {
        //基本信息
        public string HeadPortrait { get; set; }    //头像
        [Required]
        public string Gender { get; set; }          //性别
        [Required]
        public string Signature { get; set; }       //个性签名

        //个人信息
        [Required]
        public string RealName { get; set; }        //真实姓名
        public DateTime BirthDay { get; set; }      //出生年月
        [Required]
        public int Age { get; set; }                //年龄
        [Required]
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
    }
}