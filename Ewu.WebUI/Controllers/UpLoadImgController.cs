using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ewu.WebUI.Controllers
{
    public class UpLoadImgController : Controller
    {
        private static string DetailImgSrc = string.Empty;

        // GET: UpLoadImg
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 封面上传
        /// </summary>
        /// <returns></returns>
        public ActionResult Cover()
        {
            bool isSavedSuccessfully = true;
            var userID = Request.Params["UserUID"];
            var treasureUID = Request.Params["TreasureUID"];
            string fName = "";
            //遍历文件
            foreach (string fileName in Request.Files)
            {
                //获取文件对象
                HttpPostedFileBase file = Request.Files[fileName];
                //保存文件
                fName = file.FileName;
                //文件不为空，长度大于0
                if (file != null && file.ContentLength > 0 && file.ContentLength < 2097152)
                {
                    //储存路径
                    DirectoryInfo originalDirectory = new DirectoryInfo(string.Format("{0}images\\Treasure", Server.MapPath(@"\")));

                    //组合字符串成新的路径
                    string pathString = Path.Combine(originalDirectory.ToString(), userID, treasureUID);

                    //返回路径的文件名和扩展名
                    var fileFullName = Path.GetFileName(file.FileName);

                    //组成相对路径，用于存入数据库显现图片
                    string ImageSrc = "~/images/Treasure" + "/" + userID + "/" + treasureUID + "/" + fileFullName;

                    //获取该路径是否已经存在
                    bool isExists = Directory.Exists(pathString);

                    //不存在则新建
                    if (!isExists)
                    {
                        Directory.CreateDirectory(pathString);
                    }
                    //完整文件路径
                    var path = string.Format("{0}\\{1}", pathString, file.FileName);
                    //保存文件
                    file.SaveAs(path);
                }
                else
                {
                    isSavedSuccessfully = false;
                }
            }
            string msg = isSavedSuccessfully ? fName : "Error in saving file";
            return Json(new { Message = msg });
        }

        /// <summary>
        /// 细节图片
        /// </summary>
        /// <returns></returns>
        public ActionResult DetailImg()
        {
            bool isSavedSuccessfully = true;
            var userID = Request.Params["UserUID"];
            var treasureUID = Request.Params["TreasureUID"];
            string fName = "";
            //遍历文件
            foreach (string fileName in Request.Files)
            {
                //获取文件对象
                HttpPostedFileBase file = Request.Files[fileName];
                //保存文件
                fName = file.FileName;
                //文件不为空，长度大于0
                if (file != null && file.ContentLength > 0 && file.ContentLength < 2097152)
                {
                    //储存路径
                    DirectoryInfo originalDirectory = new DirectoryInfo(string.Format("{0}images\\Treasure", Server.MapPath(@"\")));

                    //组合字符串成新的路径
                    string pathString = Path.Combine(Path.Combine(originalDirectory.ToString(), userID), treasureUID);

                    //返回路径的文件名和扩展名
                    var fileFullName = Path.GetFileName(file.FileName);

                    //组成相对路径，用于存于数据库显示图片
                    //如果不为空
                    if (string.IsNullOrEmpty(DetailImgSrc))
                    {
                        DetailImgSrc += "~/images/Treasure" + "/" + userID + "/" + treasureUID + "/" + fileFullName;
                    }
                    else
                    {
                        DetailImgSrc += "|||~/images/Treasure" + "/" + userID + "/" + treasureUID + "/" + fileFullName;
                    }

                    //获取该路径是否已经存在
                    bool isExists = Directory.Exists(pathString);

                    //不存在则新建
                    if (!isExists)
                    {
                        Directory.CreateDirectory(pathString);
                    }
                    //完整文件路径
                    var path = string.Format("{0}\\{1}", pathString, file.FileName);
                    //保存文件
                    file.SaveAs(path);

                    //存入数据库
                }
                else
                {
                    isSavedSuccessfully = false;
                }
            }
            string msg = isSavedSuccessfully ? fName : "Error in saving file";
            return Json(new { Message = msg });
        }
    }
}