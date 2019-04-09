using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Models
{
    /// <summary>
    /// 注册用户验证模型
    /// </summary>
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }        //用户名

        public string Email { get; set; }       //邮箱

        [Required]
        public string Password { get; set; }    //密码

        [Required]
        [Compare("Password")]
        public string ConfirmedPassWd { get; set; } //确认密码

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

        public string CAPTCHA { get; set; }     //验证码
        public bool IsMatch { get; set; }       //验证码是否通过
    }

    /// <summary>
    /// 登录验证模型
    /// </summary>
    public class LoginModel
    {
        [Required]
        public string Name { get; set; }        //用户名
        [Required]
        public string Password { get; set; }    //密码
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