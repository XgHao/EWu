using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Models.ViewModel    
{
    /// <summary>
    /// 为视图提供的PagingInfo视图模型类实例
    /// </summary>
    public class TreasureListViewModel
    {
        //物品信息
        public IEnumerable<Treasure> Treasures { get; set; }

        //分页信息
        public PagingInfo PagingInfo { get; set; }

        //当前分类
        public string CurrentCategory { get; set; }

        //当前用户信息
        public AppUser CurrentUserInfo { get; set; }
    }
}