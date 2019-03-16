using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Ewu.Domain.Entities
{
    /// <summary>
    /// 详情发送信息
    /// </summary>
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        [Display(Name = "收件人")]
        public string Name { get; set; }                        //收件人

        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name = "省")]
        public string Line1 { get; set; }                       //省
        [Display(Name = "市")]
        public string Line2 { get; set; }                       //市
        [Display(Name = "区")]
        public string Line3 { get; set; }                       //区

        [Required(ErrorMessage = "Please enter a city name")]
        [Display(Name = "城市")]
        public string City { get; set; }                        //城市

        [Required(ErrorMessage = "Please enter a state name")]
        [Display(Name = "洲")]   
        public string State { get; set; }                       //洲
        [Display(Name = "邮编")]
        public string Zip { get; set; }                         //邮编

        [Required(ErrorMessage = "Please enter a country name")]
        [Display(Name = "乡村")]                                 
        public string Country { get; set; }                     //村
        [Display(Name = "是否赠送")]                              
        public bool GiftWrap { get; set; }                      //是否赠送
    }
}
