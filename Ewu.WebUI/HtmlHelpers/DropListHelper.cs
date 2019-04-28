using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ewu.Domain.Db;
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

        /// <summary>
        /// 清空封面图和细节图
        /// </summary>
        /// <param name="treaguid"></param>
        /// <returns>True代表删除成功</returns>
        public static bool DeletePic(Guid treaguid)
        {
            using (var db = new TreasureDataContext())
            {
                var treasure = db.Treasures.Where(t => t.TreasureUID == treaguid).FirstOrDefault();
                if (treasure != null)
                {
                    try
                    {
                        treasure.Cover = "";
                        treasure.DetailPic = "";
                        db.SubmitChanges();
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
                return false;
            }
        }
    }
}