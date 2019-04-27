using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Ewu.Domain.Entities
{
    public class Treasure
    {
        /// <summary>
        /// 物品唯一标识
        /// </summary>
        [Key]                                     
        [HiddenInput(DisplayValue = false)]       
        public Guid TreasureUID { get; set; }    

        /// <summary>
        /// 所有者UID
        /// </summary>
        public string HolderID { get; set; }        

        /// <summary>
        /// 物品名称
        /// </summary>
        [Required(ErrorMessage = "请填写物品名称")]
        public string TreasureName { get; set; }    

        /// <summary>
        /// 物品分类
        /// </summary>
        public string TreasureType { get; set; }    

        /// <summary>
        /// 损坏程度
        /// </summary>
        public string DamageDegree { get; set; }    

        /// <summary>
        /// 交易范围
        /// </summary>
        public string TradeRange { get; set; }      

        /// <summary>
        /// 封面图片路径
        /// </summary>
        public string Cover { get; set; }          

        /// <summary>
        /// 细节图片路径
        /// </summary>
        public string DetailPic { get; set; }       

        /// <summary>
        /// 补充说明
        /// </summary>
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "请说明一下你的物品")]
        public string DetailContent { get; set; }   

        #region 地点
        /// <summary>
        /// 地点-省
        /// </summary>
        public string LocationProvince { get; set; }        //地点-省
        /// <summary>
        /// 地点-城市
        /// </summary>
        public string LocationCity { get; set; }        //地点-城市
        /// <summary>
        /// 城市-地区
        /// </summary>
        public string LocationDistrict { get; set; }        //地点-地区
        #endregion

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks { get; set; }         
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime UploadTime { get; set; }    
        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }    
        /// <summary>
        /// 编辑次数
        /// </summary>
        public int EditCount { get; set; }         
        /// <summary>
        /// 收藏数
        /// </summary>
        public int Favorite { get; set; }           
        /// <summary>
        /// 物品的详情页
        /// </summary>
        public string Link { get; set; }          
        /// <summary>
        /// 物品的浏览量
        /// </summary>
        public int BrowseNum { get; set; }        

        //图像上传测试
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
