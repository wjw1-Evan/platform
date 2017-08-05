using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Common;
using ImageProcessor;
using ImageProcessor.Imaging;
using IServices.ISysServices;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace Web.Controllers
{
    [Authorize]
    public class FilesController : Controller
    {
        private readonly IUserInfo _iUserInfo;

        public FilesController(IUserInfo iUserInfo)
        {
            _iUserInfo = iUserInfo;
        }

        public enum Filetypes
        {
            全部 = 0,
            图片 = 1
        }

        public class UserUploadFile
        {
            public string Filename { get; set; }
            public string Url { get; set; }
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="filetype"></param>
        /// <param name="ckeditor"></param>
        /// <param name="resizeMode"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile(Filetypes filetype = Filetypes.全部, bool ckeditor = false, ResizeMode resizeMode = ResizeMode.BoxPad)
        {
            var fileurllist = new List<UserUploadFile>();

            if (Request.Files.Count == 0)
            {
                throw new Exception("文件不存在！");
            }

            for (var i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];

                if (file == null) continue;

                var extName = Path.GetExtension(file.FileName);

                if (string.IsNullOrEmpty(extName))
                {
                    throw new Exception("找不到文件扩展名！");
                }
                extName = extName.ToLower();

                if (filetype == Filetypes.全部)
                {
                    if (string.IsNullOrEmpty(extName) ||
                        !ConfigurationManager.AppSettings["UploadFilesExtensions"].Contains(extName.ToLower()))
                    {
                        throw new Exception("文件格式错误！");
                    }
                }

                if (filetype == Filetypes.图片)
                {
                    if (string.IsNullOrEmpty(extName) ||
                        !ConfigurationManager.AppSettings["UploadImagesExtensions"].Contains(extName.ToLower()))
                    {
                        throw new Exception("文件格式错误！");
                    }
                }

                var filename = Guid.NewGuid() + extName;

                var outStream = new MemoryStream();

                if (filetype == Filetypes.图片 && !ckeditor)
                {
                    var imageMaxWidth = 1000;
                    var imageMaxHeight = 750;

                    if (ConfigurationManager.AppSettings["ImageMaxWidth"] != null)
                    {
                        int.TryParse(ConfigurationManager.AppSettings["ImageMaxWidth"], out imageMaxWidth);
                    }

                    if (ConfigurationManager.AppSettings["ImageMaxHeight"] != null)
                    {
                        int.TryParse(ConfigurationManager.AppSettings["ImageMaxHeight"], out imageMaxHeight);
                    }

                    new ImageFactory().Load(file.InputStream)
                        .Resize(new ResizeLayer(new Size(imageMaxWidth, imageMaxHeight), resizeMode, upscale: true))
                        .BackgroundColor(Color.White)
                        .Save(outStream);
                }
                else
                {
                    file.InputStream.CopyTo(outStream);
                }

                var uploadFileRoot = "userfiles";

                if (ConfigurationManager.AppSettings["UploadFileRoot"] != null)
                {
                    uploadFileRoot = ConfigurationManager.AppSettings["UploadFileRoot"];
                }

                //上传到存储 或者网站目录

                if (ConfigurationManager.AppSettings["StorageConnectionString"] != null)
                {
                    var storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

                    var blobClient = storageAccount.CreateCloudBlobClient();

                    var container = blobClient.GetContainerReference(uploadFileRoot);

                    var blockBlob = container.GetBlockBlobReference(_iUserInfo.UserId + "/" + DateTimeOffset.Now.Year+"/" + DateTimeOffset.Now.Month+ "/" + filename);

                    outStream.Position = 0;

                    blockBlob.UploadFromStream(outStream);

                    fileurllist.Add(new UserUploadFile { Filename = blockBlob.Name, Url = blockBlob.Uri.ToString() });
                }
                else
                {
                    var url = "/" + uploadFileRoot + "/" + _iUserInfo.UserId + "/" + DateTimeOffset.Now.Year + "/" + DateTimeOffset.Now.Month + "/";

                    var path = Server.MapPath(url);

                    if (!Directory.Exists(path))//如果不存在就创建file文件夹
                    {
                        Directory.CreateDirectory(path);
                    }

                    var filePhysicalPath = path + filename;//我把它保存在网站根目录的 upload 文件夹，需要在项目中添加对应的文件夹

                    outStream.Position = 0;

                    outStream.CopyTo(new FileStream(filePhysicalPath, FileMode.CreateNew, FileAccess.ReadWrite));  //上传文件到指定文件夹

                    fileurllist.Add(new UserUploadFile { Filename = file.FileName, Url = url + filename });

                }

            }

            //上传成功后，我们还需要通过以下的一个脚本把图片返回到第一个tab选项
            if (!ckeditor) return Json(fileurllist);

            var ckEditorFuncNum = System.Web.HttpContext.Current.Request["CKEditorFuncNum"];

            return Content("<script>window.parent.CKEDITOR.tools.callFunction(" + ckEditorFuncNum + ", \"" + fileurllist.FirstOrDefault()?.Url + "\");</script>");
        }

    }
}
