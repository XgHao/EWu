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
        [Key]                                     //主键
        [HiddenInput(DisplayValue = false)]       
        public Guid TreasureUID { get; set; }     //物品唯一标识

        public string HolderID { get; set; }        //所有者ID


        [Required(ErrorMessage = "请填写物品名称")]
        public string TreasureName { get; set; }    //物品名称

        public string TreasureType { get; set; }    //物品类别
        public string DamageDegree { get; set; }    //损坏程度
        public string TradeRange { get; set; }      //交易范围
        public string Cover { get; set; }           //封面图片
        public string DetailPic { get; set; }       //补充图片

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "请说明一下你的物品")]
        public string DetailContent { get; set; }   //补充说明

        public string LocationProvince { get; set; }        //地点-省
        public string LocationCity { get; set; }        //地点-城市
        public string LocationDistrict { get; set; }        //地点-地区

        public string Remarks { get; set; }         //备注
        public DateTime UploadTime { get; set; }    //上传时间
        public DateTime UpdateTime { get; set; }    //最后更新时间
        public int Favorite { get; set; }           //收藏数

        public string Link { get; set; }            //物品的详情页

        public int BrowseNum { get; set; }          //物品的浏览量

        //图像上传测试
        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }
    }
}
