using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Ewu.Domain.Entities;
using Ewu.Domain.Db;

namespace Ewu.WebUI.Models.ViewModel
{
    /// <summary>
    /// 注册用户验证模型
    /// </summary>
    public class CreateModel
    {
        [Required(ErrorMessage = "请填写用户名")]
        public string Name { get; set; }        //用户名

        [Required(ErrorMessage ="请填写电子邮箱地址")]
        public string Email { get; set; }       //邮箱

        [Required(ErrorMessage = "请设置你的登录密码")]
        public string Password { get; set; }    //密码

        [Required(ErrorMessage = "请再次输入你的登录密码")]
        [Compare("Password",ErrorMessage = "两次密码输入不同，请检查")]
        public string ConfirmedPassWd { get; set; } //确认密码

        [Required]
        public string PhoneNumber { get; set; } //联系方式

        public string RealName { get; set; }    //真实姓名

        public string Gender { get; set; }      //性别 

        public DateTime BirthDay { get; set; }  //出生年月

        public int Age { get; set; }            //年龄

        public string IDCardNO { get; set; }    //身份证号码

        public string NativePlace { get; set; } //籍贯

        //身份证照
        public byte[] IDCardImageData { get; set; }
        public string IDCardImageMimeType { get; set; }
        public string OCRresult { get; set; }       //识别结果

        public string PhoCAPTCHA { get; set; }     //手机验证码
        public bool PhoIsMatch { get; set; }       //验证码是否通过
        public string EmailCAPTCHA { get; set; }    //邮箱验证码
        public bool EmailIsMatch { get; set; }    //邮箱是否通过
    }

    /// <summary>
    /// 登录验证模型
    /// </summary>
    public class LoginModel
    {
        [Required(ErrorMessage = "请填写用户名")]
        public string LoginName { get; set; }        //用户名

        [Required(ErrorMessage = "请填写登录密码")]
        public string LoginPassword { get; set; }    //密码
    }

    /// <summary>
    /// 角色编辑模型
    /// </summary>
    public class RoleEditModel
    {
        public AppRole Role { get; set; }                       //角色对象
        public IEnumerable<AppUser> Members { get; set; }       //成员用户集合
        public IEnumerable<AppUser> NonMembers { get; set; }    //非成员用户集合
    }

    /// <summary>
    /// 角色修改模型
    /// </summary>
    public class RoleModificationModel
    {
        [Required]
        public string RoleName { get; set; }                    //角色名
        public string[] IdsToAdd { get; set; }                  //要添加的角色名集合
        public string[] IdsToDelete { get; set; }               //要删除的角色名集合
    }

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserHeadInfo
    {
        /// <summary>
        /// 用户基本信息
        /// </summary>
        public BasicUserInfo BasicUserInfo { get; set; }

        /// <summary>
        /// 通知
        /// </summary>
        public IEnumerable<NoticeTongZhi> noticeTongZhis { get; set; }
        
        /// <summary>
        /// 留言
        /// </summary>
        public IEnumerable<NoticeLiuYan> noticeLiuYans { get; set; }
    }

    /// <summary>
    /// 通知
    /// </summary>
    public class NoticeTongZhi
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 物品
        /// </summary>
        public Treasure Treasure { get; set; }

        /// <summary>
        /// 动作
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 用户基本信息
        /// </summary>
        public BasicUserInfo BasicUserInfo { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool isRead { get; set; }
    }

    /// <summary>
    /// 留言
    /// </summary>
    public class NoticeLiuYan
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 用户基本信息
        /// </summary>
        public BasicUserInfo BasicUserInfo { get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool isRead { get; set; }
    }

    /// <summary>
    /// 消息视图模型
    /// </summary>
    public class MessageViewModel
    {
        /// <summary>
        /// 所有历史消息
        /// </summary>
        public IEnumerable<Message> Messages { get; set; }

        /// <summary>
        /// 当前消息UID
        /// </summary>
        public string CurrMessUID { get; set; }

        /// <summary>
        /// 当前用户基本信息
        /// </summary>
        public BasicUserInfo BasicUserInfoMy { get; set; }


        /// <summary>
        /// 对方用户基本信息
        /// </summary>
        public BasicUserInfo BasicUserInfoTa { get; set; }
    }


    /// <summary>
    /// 所有消息视图模型
    /// </summary>
    public class AllMessageViewModel
    {
        public int Cnt { get; set; }

        public IEnumerable<Message> Messages { get; set; }
    }
    
    /// <summary>
    /// 消息类
    /// </summary>
    public class Message
    {
        /// <summary>
        /// 用户信息
        /// </summary>
        public BasicUserInfo BasicUserInfo { get; set; }

        /// <summary>
        /// 具体信息
        /// </summary>
        public Notice Notice { get; set; }
    }

    /// <summary>
    /// 上传图片模型
    /// </summary>
    public class UploadImgs
    {
        public string TreasureUID { get; set; }
        public string UserID { get; set; }
        public string TreasureName { get; set; }
    }

    /// <summary>
    /// 物品详情页视图模型
    /// </summary>
    public class TreaInfo
    {
        //物品信息
        public Treasure treasureInfo { get; set; }
        //所有人信息
        public AppUser HolderInfo { get; set; }
        //当前登录人信息
        public AppUser LoginUserInfo { get; set; }
        //图片集合
        public IEnumerable<string> DetailImgs { get; set; }
        //是否收藏
        public bool IsFavorite { get; set; }
        //当前登录人-评论头像显示
        public BasicUserInfo CurrenUser { get; set; }
        //浏览记录
        public IEnumerable<BrowseLog> browseLogs { get; set; }


        //推荐信息
        public int DisRecommend { get; set; }
        public int Recommend { get; set; }

    }

    /// <summary>
    /// 浏览记录
    /// </summary>
    public class BrowseLog
    {
        //浏览者
        public BasicUserInfo Browser { get; set; }

        //浏览时间
        public DateTime BrowserTime { get; set; }
    }

    /// <summary>
    /// 交易模型
    /// </summary>
    public class DealInfo
    {
        //持有人
        public AppUser Holder { get; set; }
        
        //交易的物品对象
        public Treasure DealTreasure { get; set; }

        //当前用户的物品集合
        public DealMyTreasureModel DealMyTreasureModel { get; set; }
    }

    /// <summary>
    /// 用户物品模型
    /// </summary>
    public class DealMyTreasureModel
    {
        //交易物品ID
        public Guid DealTreasureGuid { get; set; }
        //当前登录用户物品模型集合
        public IEnumerable<Treasure> UserTreasures { get; set; }

    }
        
    /// <summary>
    /// 生成交易订单视图模型
    /// </summary>
    public class DealLogCreate
    {
        /// <summary>
        /// 换入物品
        /// </summary>
        public Treasure DealInTreasure { get; set; }

        /// <summary>
        /// 换出物品
        /// </summary>
        public Treasure DealOutTreasure { get; set; }

        #region 联系方式
        public string Email { get; set; }
        public string PhoneNum { get; set; }
        public string WeChat { get; set; }
        public string QQ { get; set; }
        #endregion

        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 交易订单UID
        /// </summary>
        public string DealLogID { get; set; }

    }

    /// <summary>
    /// 用户交易记录
    /// </summary>
    public class UserDeal
    {
        /// <summary>
        /// 订单记录
        /// </summary>
        public IEnumerable<LogDealTableInfo> LogDealTableInfos { get; set; }

    }

    /// <summary>
    /// 交易记录页面视图模型
    /// </summary>
    public class LogDealTableInfo
    {
        /// <summary>
        /// 交易记录
        /// </summary>
        public LogDeal LogDeal { get; set; }

        /// <summary>
        /// 交易用户对象
        /// </summary>
        public BasicUserInfo Trader { get; set; }

        /// <summary>
        /// 交换得到的物品对象
        /// </summary>
        public Treasure DealInTrea { get; set; }

        /// <summary>
        /// 交换换出的物品对象
        /// </summary>
        public Treasure DealOutTrea { get; set; }

        /// <summary>
        /// 当前登录人是什么觉得
        /// </summary>
        public bool? IsSponsor { get; set; }
    }

    /// <summary>
    /// 用户基本信息模型
    /// </summary>
    public class BasicUserInfo
    {
        /// <summary>
        /// 用户UID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        public string HeadImg { get; set; }

        /// <summary>
        /// 用户生日
        /// </summary>
        public string BirthDay { get; set; }

        /// <summary>
        /// 用户签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string PhoNum { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }
    }

    /// <summary>
    /// 我收到/发起/完成交易的视图模型
    /// </summary>
    public class InitiateDealLog
    {
        /// <summary>
        /// 收到交易物品
        /// </summary>
        public Treasure InitiateTreasures { get; set; }

        /// <summary>
        /// 交换我的物品对象
        /// </summary>
        public Treasure MyTreasure { get; set; }

        /// <summary>
        /// 交易记录信息
        /// </summary>
        public LogDeal LogDeal { get; set; }

        /// <summary>
        /// 对方个人模型
        /// </summary>
        public AppUser Dealer { get; set; }
    }

    /// <summary>
    /// 交易完成订单视图模型
    /// </summary>
    public class CompleteDeal
    {
        //发起人及物品
        public BasicUserInfo UserS { get; set; }
        public Treasure TreasureS { get; set; }

        //接收人及物品
        public BasicUserInfo UserR { get; set; }
        public Treasure TreasureR { get; set; }

        //订单对象
        public LogDeal LogDeal { get; set; }

        //评价
        public Evaluation Evaluation { get; set; }
    }

    /// <summary>
    /// 正在交易的视图模型
    /// </summary>
    public class DealingLog
    {
        /// <summary>
        /// 当前交易订单
        /// </summary>
        public LogDeal LogDeal { get; set; }

        /// <summary>
        /// 我的物品-当前登录人
        /// </summary>
        public Treasure MyTreasure { get; set; }

        /// <summary>
        /// TA的物品
        /// </summary>
        public Treasure TaTreasure { get; set; }

        /// <summary>
        /// 当前物流信息
        /// </summary>
        public Tracking Tracking { get; set; }

        /// <summary>
        /// 当前登录人信息
        /// </summary>
        public AppUser My { get; set; }

        /// <summary>
        /// 对方信息
        /// </summary>
        public AppUser Ta { get; set; }

        /// <summary>
        /// 当前用户在本次订单中是什么角色
        /// </summary>
        public string CurrentUserRole { get; set; }

        /// <summary>
        /// 是否已经评价
        /// </summary>
        public bool IsEvaluation { get; set; }

        /// <summary>
        /// 对方的收货地址
        /// </summary>
        public DeliveryAddress DeliveryAddress{ get; set; }
    }

    /// <summary>
    /// 用户收货地址视图模型
    /// </summary>
    public class MyDeliveryAddress
    {
        /// <summary>
        /// 收货地址集合
        /// </summary>
        //public IEnumerable<DeliveryAddress> DeliveryAddresses { get; set; }

        /// <summary>
        /// 新的收货地址
        /// </summary>
        public DeliveryAddress NewdeliveryAddress { get; set; }

        /// <summary>
        /// 当前的交易订单
        /// </summary>
        public LogDeal CurrentLogDeal { get; set; }

        /// <summary>
        /// 当前账户角色
        /// </summary>
        public string CurrentRole { get; set; }
    }

    /// <summary>
    /// 物流信息模型
    /// </summary>
    public class DeliveryNum
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string DLogUID { get; set; }

        /// <summary>
        /// 当前用户角色
        /// </summary>
        public string CurrentRole { get; set; }
    }

    /// <summary>
    /// 评价视图模型
    /// </summary>
    public class Score
    {
        /// <summary>
        /// 当前交易收货的物品-对方的物品
        /// </summary>
        public Treasure CurTreasure { get; set; }

        /// <summary>
        /// 对方信息
        /// </summary>
        public BasicUserInfo SideUser { get; set; }

        /// <summary>
        /// 当前订单对象
        /// </summary>
        public LogDeal LogDeal { get; set; }

        /// <summary>
        /// 订单评价
        /// </summary>
        public NowEvaluation NowEvaluation { get; set; }
    }

    /// <summary>
    /// 当前评价信息
    /// </summary>
    public class NowEvaluation
    {
        /// <summary>
        /// 是否推荐
        /// </summary>
        [Required(ErrorMessage = "该信息是必须的")]
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 评价信息
        /// </summary>
        [Required(ErrorMessage = "该信息是必须的")]
        public string EvaluationInfo { get; set; }
    }

    /// <summary>
    /// 订单详情视图模型
    /// </summary>
    public class DealAllInfo
    {
        /// <summary>
        /// 发起方物品
        /// </summary>
        public Treasure TreasureS { get; set; }

        /// <summary>
        /// 接收方物品
        /// </summary>
        public Treasure TreasureR { get; set; }

        /// <summary>
        /// 发起方收货信息
        /// </summary>
        public DeliveryAddress DeliveryAddressS { get; set; }

        /// <summary>
        /// 接收方收货信息
        /// </summary>
        public DeliveryAddress DeliveryAddressR { get; set; }

        /// <summary>
        /// 发起人
        /// </summary>
        public BasicUserInfo BasicUserInfoS { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public BasicUserInfo BasicUserInfoR { get; set; }

        /// <summary>
        /// 评价
        /// </summary>
        public Evaluation Evaluation { get; set; }

    }

    /// <summary>
    /// 物品类型数
    /// </summary>
    public class TreasureTypeCnt
    {
        public string type { get; set; }
        public int cnt { get; set; }
    }

    /// <summary>
    /// 物品收藏数
    /// </summary>
    public class TreasureFavoriteCnt
    {
        public string treauid { get; set; }
        public int cnt { get; set; }
    }

    public class UserViewModel
    {
    }
}