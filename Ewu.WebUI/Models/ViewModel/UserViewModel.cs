using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Ewu.Domain.Entities;

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

    public class UserViewModel
    {
    }
}