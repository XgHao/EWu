using Ewu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ewu.WebUI.Models.ViewModel
{

    public class AllTreasureViewModel
    {
        public Treasure TreasureInfo { get; set; }

        public BasicUserInfo holderInfo { get; set; }
    }

    public class AllUserViewModel
    {
        public BasicUserInfo userInfo { get; set; }

        public bool isChoose { get; set; }
    }


    public class AdminViewModel
    {
    }
}