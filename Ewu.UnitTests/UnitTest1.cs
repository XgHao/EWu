using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Moq;
using Ewu.Domain.Abstract;
using Ewu.Domain.Entities;
using Ewu.WebUI.Controllers;
using Ewu.WebUI.HtmlHelpers;
using Ewu.WebUI.Models;

namespace Ewu.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        public void Can_Cenerate_Links()
        {
            //准备-定义一个HTML辅助器，这是必须的，目的是运用扩展方法
            HtmlHelper myHelper = null;

            //准备-创建PagingInfo数据
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                ItemsPerPage = 10,
                TotalItem = 28
            };

            //准备-用lambda表达式建立委托
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //动作-
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //断言
        }

    }
}
