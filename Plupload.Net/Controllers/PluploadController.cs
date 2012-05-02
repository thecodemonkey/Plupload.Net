using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Script.Serialization;
using Plupload.Net.Model;
using Plupload.Net.Utils;
using System.Drawing;

namespace Plupload.Net.Controllers
{
    public class PluploadController : ControllerBase
    {
        //String[] _imageFiles = new string[] { ".jpg", ".png", ".gif", ".jpeg",".bmp" };

        public ActionResult Index()
        {
            return this.EmbeddedPartialView(PluploadConstants.VIEWS_RAZOR_UPLOAD_INDEX, this.Configuration);
        }

        public virtual ActionResult IFrame() 
        {
            return this.EmbeddedPartialView(PluploadConstants.VIEWS_RAZOR_UPLOAD_IFRAME_INDEX);
        }

        public virtual ActionResult Script(string path)
        {
            this.ViewBag.ResourcePath = path;
            return this.EmbeddedPartialView(PluploadConstants.VIEWS_RAZOR_UPLOAD_SCRIPT);
        }

        public virtual ActionResult Style(string path)
        {
            this.ViewBag.ResourcePath = path;
            return this.EmbeddedPartialView(PluploadConstants.VIEWS_RAZOR_UPLOAD_STYLE);
        }


        public virtual ActionResult SaveFile()
        {                                           
            LogWriter.Debug("receive uploaded file...");
            
            Message message = new Message();
            HttpPostedFileBase file = Request.Files["file"];

            if (file != null)
            {                       
                LogWriter.Debug(String.Format("file '{0}' successfull received.", file.FileName));

                FileInfo fi = GetSaveFileInfo(message, Path.GetFileName(file.FileName));
                SaveFileToSystem(message, file, fi);
                //CreateImagePaths(message, fi);
            }
            else 
            {
                LogWriter.Warning("uploaded file is null!");
            }

            return Json(message);
        }

        protected virtual bool SaveFileToSystem(Message message, HttpPostedFileBase uploadedFile, FileInfo saveFileInfo)
        {
            bool result = false;

            LogWriter.Debug(String.Format("try to save the file '{0}'", saveFileInfo.FullName));

            switch (this.Configuration.SaveOptions)
            {
                case SaveOptions.Override:
                    LogWriter.Debug(String.Format("override file if exists '{0}'", saveFileInfo.Name));

                    this.Save(uploadedFile, saveFileInfo);
                    LogWriter.Debug(String.Format("file '{0}' was successful saved!", saveFileInfo.FullName));
                    break;
                case SaveOptions.CancelIfExists:
                    if (saveFileInfo.Exists)
                    {
                        LogWriter.Debug(String.Format("file '{0}' already exists, setting CancelIfExists is true, so you cannot override it!", saveFileInfo.Name));
                        message.MessageType = "Error";
                        message.Text = "File Exists already";
                    }
                    else
                    {
                        this.Save(uploadedFile, saveFileInfo);
                        LogWriter.Debug(String.Format("file '{0}' was successful saved!", saveFileInfo.FullName));
                    }

                    break;
                case SaveOptions.None:
                case SaveOptions.RenameIfExists:
                    while (saveFileInfo.Exists)
                    {
                        LogWriter.Debug(String.Format("file '{0}' already exists, increase nameindex", saveFileInfo.Name));
                        int i = 1;
                        string filename = saveFileInfo.Name.Replace(saveFileInfo.Extension,"") + i;
                        saveFileInfo = new FileInfo(Path.Combine(saveFileInfo.DirectoryName, filename + saveFileInfo.Extension));
                    }

                    this.Save(uploadedFile, saveFileInfo);
                    LogWriter.Debug(String.Format("file '{0}' was successful saved!", saveFileInfo.FullName));
                    
                    break;
                default:
                    break;
            }

            CreateImagePaths(message, saveFileInfo);
            return result;
        }

        protected virtual FileInfo GetSaveFileInfo(Message message, String fileName)
        {
            string path = this.Configuration.GetPhysicalUploadPath();
            string fullPath = Path.Combine(path, fileName);
            FileInfo result = new FileInfo(fullPath);            
            return result;
        }

        protected virtual void CreateImagePaths(Message message, FileInfo saveFileInfo)
        {
            if (PluploadConstants.IMAGE_EXTENSIONS.Contains(saveFileInfo.Extension.ToLower()))
            {
                UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                string url = u.Action("GetThumbnail", "Ressource", new { imageName = saveFileInfo.Name });

                message.ThumbPath = url;
                message.ImagePath = u.Action("GetImageStream", "Ressource", new { imageName = saveFileInfo.Name });
            }

            message.FileName = saveFileInfo.Name;
        }

        protected string Save(HttpPostedFileBase uploadedFile, FileInfo saveFileInfo) 
        {
            if (PluploadConstants.IMAGE_EXTENSIONS.Contains(saveFileInfo.Extension.ToLower()))
            {
                if (this.Configuration.Resize != null && this.Configuration.Resize.height > 0 && this.Configuration.Resize.width.HasValue && this.Configuration.Resize.height.HasValue) 
                {
                    LogWriter.Debug(String.Format("Resize Image '{0}' was successful saved!", saveFileInfo.FullName));

                    Image.GetThumbnailImageAbort imgAbort = new Image.GetThumbnailImageAbort(() =>{ return true; });

                    Image image = Image.FromStream(uploadedFile.InputStream);
                    Image img = image.GetThumbnailImage(this.Configuration.Resize.width.Value, this.Configuration.Resize.height.Value, imgAbort, IntPtr.Zero);
                    img.Save(saveFileInfo.FullName);
                    
                    return saveFileInfo.FullName;
                }
            }

            uploadedFile.SaveAs(saveFileInfo.FullName);

            return saveFileInfo.FullName;
        }
    }
}
