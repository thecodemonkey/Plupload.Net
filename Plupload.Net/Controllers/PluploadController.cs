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
    /// <summary>
    /// the main controller for server side handling of plupload component.
    /// To customize server side handling, just derive from this controller and
    /// override the needed action or another needed method.
    /// </summary>
    public class PluploadController : ControllerBase
    {
        /// <summary>
        /// renders the main plupload.net view, included complete UI.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return this.EmbeddedPartialView(PluploadConstants.VIEWS_RAZOR_UPLOAD_INDEX, this.Configuration);
        }

        /// <summary>
        /// OLD STUFF optionaly IFrame version of the main view.
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult IFrame() 
        {
            return this.EmbeddedPartialView(PluploadConstants.VIEWS_RAZOR_UPLOAD_IFRAME_INDEX);
        }

        /// <summary>
        /// receives and saves a single uploaded file. 
        /// </summary>
        /// <returns>a JsonResult with all informations about saving the file.</returns>
        public virtual ActionResult SaveFile()
        {                                           
            LogWriter.Debug("receive uploaded file...");
            
            Message message = new Message();
            HttpPostedFileBase file = Request.Files["file"];

            if (file != null)
            {                       
                LogWriter.Debug(String.Format("file '{0}' successfull received.", file.FileName));

                FileInfo fi = GetSaveFileInfo(Path.GetFileName(file.FileName));
                SaveFileToSystem(message, file, fi);
                //CreateImagePaths(message, fi);
            }
            else 
            {
                LogWriter.Warning("uploaded file is null!");
            }

            return Json(message);
        }

        /// <summary>
        /// saves the file to the filesystem
        /// </summary>
        /// <param name="message">will be updated with the informations about the saving process</param>
        /// <param name="uploadedFile">uploaded file</param>
        /// <param name="saveFileInfo">fileInfo about saved file. contains the targed as FullPath for the uploaded file.</param>
        /// <returns>the result of the saving operation</returns>
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

        /// <summary>
        /// gets the FileInfo for the uploaded file.
        /// </summary>
        /// <param name="fileName">the name of the uploaded file</param>
        /// <returns>file info contains name and fullpathe of the uploaded file, wich shoul be save to the filesystem.</returns>
        protected virtual FileInfo GetSaveFileInfo(String fileName)
        {
            string path = this.Configuration.GetPhysicalUploadPath();
            string fullPath = Path.Combine(path, fileName);
            FileInfo result = new FileInfo(fullPath);            
            return result;
        }

        /// <summary>
        /// creates an image path. this methods handles only files with image specific extensions,
        /// defined within PluploadConstants.IMAGE_EXTENSIONS
        /// </summary>
        /// <param name="message">informations wich will be returned back to the client.</param>
        /// <param name="saveFileInfo">fileInfo of the uploaded image file</param>
        protected virtual void CreateImagePaths(Message message, FileInfo saveFileInfo)
        {
            if (PluploadConstants.IMAGE_EXTENSIONS.Contains(saveFileInfo.Extension.ToLower()))
            {
                UrlHelper u = new UrlHelper(this.ControllerContext.RequestContext);
                string url = u.Action("UploadedImageThumbnail", "Ressource", new { imageName = saveFileInfo.Name });

                message.ThumbPath = url;
                message.ImagePath = u.Action("UploadedImage", "Ressource", new { imageName = saveFileInfo.Name });
            }

            message.FileName = saveFileInfo.Name;
        }

        /// <summary>
        /// saves the file to the filesystem.
        /// </summary>
        /// <param name="uploadedFile">uploaded file</param>
        /// <param name="saveFileInfo">contains information about the target location where the uploaded file shoul be saved.</param>
        /// <returns>a fullpath of the saved file</returns>
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
