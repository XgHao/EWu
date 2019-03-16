using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Ewu.WebUI.Models;

namespace Ewu.WebUI.HtmlHelpers
{
    /// <summary>
    /// 实现HTML辅助器方法
    /// </summary>
    public static class PagingHelpers
    {
        /// <summary>
        /// 扩展方法-表示不再进行编码的HTML
        /// 使用PagingInfo对象中提供的信息生成一组页面链接的HTML标记
        /// </summary>
        /// <param name="html">当前的HTML</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <param name="pageUrl">一个FUNC委托方法，用于生成其他页面的链接</param>
        /// <returns></returns>
        public static MvcHtmlString PageLinks(this HtmlHelper html,PagingInfo pagingInfo,Func<int,string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            
            //遍历所有的页码
            for(int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                //用于创建HTML元素的类和属性
                TagBuilder tag = new TagBuilder("a");

                //向标记中添加特性
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                //当前页时，该页码的样式
                if (i == pagingInfo.CurrentPage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");
                }

                //其他页码的样式
                tag.AddCssClass("btn btn-default");

                //添加标记
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}