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
        //物品信息及对应所属人信息
        public IEnumerable<TreasureAndHolderInfo> TreasureAndHolderInfos { get; set; }

        //分页信息
        public PagingInfo PagingInfo { get; set; }

        //当前分类
        public string CurrentCate { get; set; }

        //分类信息
        public IEnumerable<CategoryInfo>  CategoryInfo { get; set; }

        //当前用户信息
        public AppUser CurrentUserInfo { get; set; }

        //显示样式
        public string Display { get; set; }

        //所有物品数
        public int AllCnt { get; set; }
    }

    //导航分类
    public class CategoryInfo
    {
        //分类名称
        public string CateName { get; set; }

        //分类数量
        public int CateCount { get; set; }

        //分类链接
        public string CateLink { get; set; }
    }

    /// <summary>
    /// 物品信息及对应所属人信息
    /// </summary>
    public class TreasureAndHolderInfo
    {
        public Treasure Treasure { get; set; }
        public AppUser Holder { get; set; }

        public bool IsFavorite { get; set; }
    }
}