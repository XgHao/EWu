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
        // GET: UpLoadImg
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Cover()
        {
            bool isSavedSuccessfully = true;
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
                    string pathString = Path.Combine(originalDirectory.ToString(), "imagepath");
                    //返回路径的文件名和扩展名
                    var fileName1 = Path.GetFileName(file.FileName);
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
    }
}