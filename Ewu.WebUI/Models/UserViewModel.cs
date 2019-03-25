using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

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


    public class UserViewModel
    {
    }
}