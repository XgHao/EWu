using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ewu.Domain.Entities
{
    /// <summary>
    /// 快递信息
    /// </summary>
    public class Delivery
    {
        /// <summary>
        /// 快递查询结果编号
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 状态信息-
        /// -1：单号或代码错误；
        /// 0：暂无轨迹；
        /// 1:快递收件；
        /// 2：在途中；
        /// 3：签收；
        /// 4：问题件 
        /// 5.疑难件 
        /// 6.退件签收
        /// 7：快递收件(揽件)
        /// </summary>
        public string StateInfo { get; set; }

        /// <summary>
        /// 查询信息
        /// </summary>
        public string msg { get; set; }

        /// <summary>
        /// 快递公司名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 快递公司LOGO
        /// </summary>
        public string logo { get; set; }

        /// <summary>
        /// 快递信息集合
        /// </summary>
        public IEnumerable<DeliveryInfo> deliveryInfos { get; set; }
    }

    /// <summary>
    /// 快递信息
    /// </summary>
    public class DeliveryInfo
    {
        /// <summary>
        /// 快递信息内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 快递内容时间
        /// </summary>
        public string time { get; set; }
    }
}
