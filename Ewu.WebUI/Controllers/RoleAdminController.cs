using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Ewu.WebUI.Infrastructure.Identity;
using Ewu.Domain.Entities;
using Ewu.WebUI.Models.ViewModel;

namespace Ewu.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleAdminController : Controller
    {
        /// <summary>
        /// 主页显示所有的角色
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        /// <summary>
        /// 创建角色页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 创建角色动作[HttpPost]
        /// </summary>
        /// <param name="name">角色名称</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            //验证模型无误
            if (ModelState.IsValid)
            {
                //根据角色名称创建角色，并返回结果
                IdentityResult result = await RoleManager.CreateAsync(new AppRole(name));
                //创建成功
                if (result.Succeeded)
                {
                    //重定向到主页Index
                    return RedirectToAction("Index");
                }
                //创建失败
                else
                {
                    //添加错误模型
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        /// <summary>
        /// 删除动作[HttpPost]
        /// </summary>
        /// <param name="id">当前角色ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            //根据ID查找对象角色对象
            AppRole role = await RoleManager.FindByIdAsync(id);
            //角色存在
            if (role != null)
            {
                //根据当前角色对象删除对应角色，并返回结果
                IdentityResult result = await RoleManager.DeleteAsync(role);
                //删除成功
                if (result.Succeeded)
                {
                    //重定向到主页Index
                    return RedirectToAction("Index");
                }
                //删除失败
                else
                {
                    //转到Error页面
                    return View("Error", result.Errors);
                }
            }
            //角色不存在
            else
            {
                //转到Error页面
                return View("Error", new string[] { "角色不存在" });
            }
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        public async Task<ActionResult> Edit(string id)
        {
            //根据id获取角色对象
            AppRole role = await RoleManager.FindByIdAsync(id);
            //获取当期那角色下所有用户的ID
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            //根据用户ID的集合获取用户对象集合，获取在当前角色的用户集合
            IEnumerable<AppUser> members = UserManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            //排除根据上面做得到的用户对象集合，获取不在当前角色的用户集合
            IEnumerable<AppUser> nonMembers = UserManager.Users.Except(members);
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        /// <summary>
        /// 编辑动作[HttpPost]
        /// </summary>
        /// <param name="model">修改验证模型</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            //新建身份验证结果
            IdentityResult result;
            //修改验证模型无误
            if (ModelState.IsValid)
            {
                //遍历要增加到当前角色的用户
                foreach (string userId in model.IdsToAdd ?? new string[] { }) 
                {
                    //角色新增用户，并返回结果(方法需要的参数用户的id和角色名)
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    //失败
                    if (!result.Succeeded)
                    {
                        //重定向到Error页面
                        return View("Error", result.Errors);
                    }
                }
                //遍历要从当前角色中移除的用户
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    //从角色中移除用户，并返回对象(方法需要的参数用户的id和角色名)
                    result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                    //失败
                    if (!result.Succeeded)
                    {
                        //重定向到Error页面
                        return View("Error", result.Errors);
                    }
                }
                //操作完成后，重定向到主页Index
                return RedirectToAction("Index");
            }
            
            return View("Error", new string[] { "未找到当前角色" });
        }

        /// <summary>
        /// 添加错误模型
        /// </summary>
        /// <param name="result">操作后的模型结构</param>
        private void AddErrorsFromResult(IdentityResult result)
        {
            //遍历所有错误
            foreach(string error in result.Errors)
            {
                //添加模型错误
                ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// 定义用户管理器实例对象-简化代码
        /// </summary>
        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }

        /// <summary>
        /// 定义角色管理器实例对象-简化代码
        /// </summary>
        private AppRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>();
            }
        }
    }
}