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
    /// <summary>
    /// a controller for embedded ressources. 
    /// Provides functionality to render embedded ressources.
    /// </summary>
    public class RessourceController : ControllerBase
    {
        /// <summary>
        /// renders the embedded script as script tag <script/>
        /// </summary>
        /// <param name="path">an embedded dot separated path of the script resource.</param>
        /// <returns>a html <script src="[embedde_script_path]" /> tag. </returns>
        public virtual ActionResult ScriptTag(string path)
        {
            this.ViewBag.ResourcePath = path;
            return this.EmbeddedPartialView(PluploadConstants.VIEWS_RAZOR_UPLOAD_SCRIPT);
        }

        /// <summary>
        /// renders the embedded stylesheet as <style/> tag
        /// </summary>
        /// <param name="path">an embedded dot separated path of the stylesheet resource.</param>
        /// <returns>a html <style href="[embedde_style_path]" /> tag. </returns>
        public virtual ActionResult StyleTag(string path)
        {
            this.ViewBag.ResourcePath = path;
            return this.EmbeddedPartialView(PluploadConstants.VIEWS_RAZOR_UPLOAD_STYLE);
        }

        /// <summary>
        /// renders the embedded javascript as JavaScriptResult. This Action can be used as a
        /// location of the script, defined within src attribute of the script tag.
        /// </summary>
        /// <param name="scriptPath">an embedded dot separated path to the script ressource for example: 
        /// ~/Ressource/EmbeddedJavaScript?scriptPath=Plupload.Net.Scripts.plupload.net.js</param>
        /// <returns>the content of embedded javascript ressource</returns>
        public JavaScriptResult EmbeddedJavaScript(string scriptPath)
        {
            return JavaScript(RessourceHelper.GetTextResource(scriptPath));
        }
      
        /// <summary>
        /// renders the embedded stylesheet as contentResult. This Action can be used as a
        /// location of the css file, defined within hre attribute of the style tag.
        /// </summary>
        /// <param name="cssPath">an embedded dot separated path to the stylesheet ressource for example:
        /// ~/Ressource/EmbeddedCSS?cssPath=Plupload.Net.Content.css.plupload.net.css </param>
        /// <returns>the content of embedded css ressource</returns>
        public ContentResult EmbeddedCSS(string cssPath)
        {
            return Content(RessourceHelper.GetTextResource(cssPath), "text/css");
        }

        /// <summary>
        /// gets the content of the embedded text ressource.
        /// </summary>
        /// <param name="textPath">an embedded dot separated path to the ressource. For example: 
        /// Plupload.Net.Content.css.plupload.net.css</param>
        /// <param name="contentType">a type of the content</param>
        /// <returns>a contenResult of the embedded ressource</returns>
        public ContentResult EmbeddedTextRessource(string textPath, string contentType = "application/octet-stream")
        {
            return Content(RessourceHelper.GetTextResource(textPath), contentType);
        }

        /// <summary>
        /// renders the binary embedded ressource.
        /// </summary>
        /// <param name="streamPath">an embedded dot separated path to the ressource. For example:
        /// ~/Ressource/EmbeddedBinaryRessource?streamPath=Plupload.Net.Scripts.plupload.flash.swf</param>
        /// <param name="contentType">a type of the content</param>
        /// <returns>file content result</returns>
        public FileContentResult EmbeddedBinaryRessource(string streamPath, string contentType = "application/octet-stream")
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

        /// <summary>
        /// renders the embedde image. This Action can be used as a
        /// location of the image, defined within src attribute of the <img/> tag.
        /// </summary>
        /// <param name="imagePath">an embedded dot separated path to the ressource. For example: 
        /// Plupload.Net.Content.img.buttons.png</param>
        /// <returns>an ImageResult</returns>
        public ActionResult EmbeddedImage(string imagePath)
        {
                  
            return this.Image(RessourceHelper.GetResourceStream(imagePath),"image/" + imagePath.Substring(imagePath.LastIndexOf('.')+1));

        }

        /// <summary>
        /// renders the thumbnail of the uploaded file using the given imageName and configured uploadDirectory. This Action can be used as a
        /// location of the image, defined within src attribute of the <img/> tag.
        /// </summary>
        /// <param name="imageName">the name of the image File.</param>
        /// <returns>an ImageResult</returns>
        public ActionResult UploadedImageThumbnail(string imageName)
        {
            string path = this.Configuration.GetPhysicalUploadPath();
            string imgPath = Path.Combine(path, imageName);
            return this.Image(RessourceHelper.CreateThumbnail(imgPath, new Size(30, 30)), "image/jpeg");
        }

        /// <summary>
        /// renders the the uploaded image file using the given imageName and configured uploadDirectory. This Action can be used as a
        /// location of the image, defined within src attribute of the <img/> tag.
        /// </summary>
        /// <param name="imageName">the name of the image File.</param>
        /// <returns>an ImageResult</returns>
        public ActionResult UploadedImage(string imageName)
        {
            string path = this.Configuration.GetPhysicalUploadPath(); 
            string imgPath = Path.Combine(path, imageName);
            string context = "";
            Stream stream = RessourceHelper.CreateImageStream(imgPath, ref context);
            return this.Image(stream, context);
        }
    }
}
