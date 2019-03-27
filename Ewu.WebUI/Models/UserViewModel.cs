using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Models
{
    /// <summary>
    /// 创建新用户验证模型
    /// </summary>
    public class CreateModel
    {
        [Required]
        public string Name { get; set; }        //用户名

        [Required]
        public string Email { get; set; }       //邮箱

        [Required]
        public string Password { get; set; }    //密码
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