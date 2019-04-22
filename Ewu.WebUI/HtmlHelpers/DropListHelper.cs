using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ewu.WebUI.HtmlHelpers
{
    public static class DropListHelper
    {
        /// <summary>
        /// 设置DeopList的默认选项
        /// </summary>
        /// <param name="old">selectListItems</param>
        /// <param name="SelectItem">默认的值</param>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> SetDefault(IEnumerable<SelectListItem> selectListItems, string SelectItem)
        {
            foreach (var STL in selectListItems)
            {
                if (STL.Value == SelectItem || STL.Text == SelectItem)
                {
                    STL.Selected = true;
                    break;
                }
            }
            return selectListItems;
        }
    }
}