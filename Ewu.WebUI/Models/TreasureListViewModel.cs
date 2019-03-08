using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ewu.Domain.Entities;

namespace Ewu.WebUI.Models
{
    public class TreasureListViewModel
    {
        //物品信息
        public IEnumerable<Treasure> Treasures { get; set; }
        //分页信息
        public PagingInfo PagingInfo { get; set; }
        //当前分类
        public string CurrentCategory { get; set; }
    }
}