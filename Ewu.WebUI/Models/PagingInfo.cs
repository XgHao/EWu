using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ewu.WebUI.Models
{
    public class PagingInfo
    {
        public int TotalItem { get; set; }          //总数
        public int ItemsPerPage { get; set; }       //每页的数
        public int CurrentPage { get; set; }        //当前页
        public int TotalPages {
            get { return (int)Math.Ceiling((decimal)TotalItem / ItemsPerPage); }
        }                                           //总页数
    }
}