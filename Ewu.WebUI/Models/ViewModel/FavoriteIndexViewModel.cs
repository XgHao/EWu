using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Models.ViewModel
{
    /// <summary>
    /// 收藏视图模型详情
    /// </summary>
    public class FavoriteIndexViewModel
    {
        //收藏物品
        public Favorite Favorite { get; set; }

        //返回的Url
        public string ReturnUrl { get; set; }
    }
}