using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ewu.Domain.Entities;

namespace Ewu.Domain.Abstract
{
    /// <summary>
    /// 订单接口
    /// </summary>
    public interface IOrderProcessor
    {
        /// <summary>
        /// 实现订单
        /// </summary>
        /// <param name="favorite">收藏详情</param>
        /// <param name="shippingDetails">详情发送信息</param>
        void ProcessOrder(Favorite favorite, ShippingDetails shippingDetails);
    }
}
