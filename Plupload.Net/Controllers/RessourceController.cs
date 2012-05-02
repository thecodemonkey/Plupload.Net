using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using Plupload.Net.Model;
using Plupload.Net.Utils;

namespace Plupload.Net.Controllers
{
    public class RessourceController : ControllerBase
    {
        public JavaScriptResult GetJavaScript(string scriptPath)
        {
            return JavaScript(RessourceHelper.GetTextResource(scriptPath));
        }
      
        public ContentResult GetCSS(string cssPath)
        {
            return Content(RessourceHelper.GetTextResource(cssPath), "text/css");
        }

        public ContentResult GetText(string textPath, string contentType = "application/octet-stream")
        {
            return Content(RessourceHelper.GetTextResource(textPath), contentType);
        }

        public FileContentResult GetStream(string streamPath, string contentType = "application/octet-stream")
        {
              byte[] buffer = new byte[16*1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    using (Stream input = RessourceHelper.GetResourceStream(streamPath))
                    {
                        int read;
                        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        return File(ms.ToArray(), contentType);
                    }
                }

        }

        public ActionResult GetImage(string imagePath)
        {
                  
            return this.Image(RessourceHelper.GetResourceStream(imagePath),"image/" + imagePath.Substring(imagePath.LastIndexOf('.')+1));

        }

        public ActionResult GetThumbnail(string imageName)
        {
            string path = this.Configuration.GetPhysicalUploadPath();
            string imgPath = Path.Combine(path, imageName);
            return this.Image(RessourceHelper.CreateThumbnail(imgPath, new Size(30, 30)), "image/jpeg");
        }

        public ActionResult GetImageStream(string imageName)
        {
            string path = this.Configuration.GetPhysicalUploadPath(); 
            string imgPath = Path.Combine(path, imageName);
            string context = "";
            Stream stream = RessourceHelper.CreateImageStream(imgPath, ref context);
            return this.Image(stream, context);
        }

    }
}
