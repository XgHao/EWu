using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ewu.Domain.Entities
{
    public class User
    {
        [Key]
        public Guid UserUID { get; set; }         //用户唯一标识
        
        //基本信息
        public string NickName { get; set; }        //昵称
        public string HeadPortrait { get; set; }    //头像
        public string Gender { get; set; }          //性别
        public string Signature { get; set; }       //个性签名

        //个人信息
        public string RealName { get; set; }        //真实姓名
        public DateTime BirthDay { get; set; }      //出生年月
        public int Age { get; set; }                //年龄
        public string IDCardNO { get; set; }        //身份证号码
        public string NativePlace { get; set; }     //籍贯
        public string ContactInfo { get; set; }     //联系方式
        public string Mail { get; set; }            //邮箱
        public string PassWord { get; set; }        //登录密码
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
