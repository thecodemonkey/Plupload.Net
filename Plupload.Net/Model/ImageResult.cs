using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;

namespace Plupload.Net.Model
{
    /// <summary>
    /// a custom additional ActionResult wich renders Images
    /// </summary>
    public class ImageResult : ActionResult
    {
        /// <summary>
        /// a strem to the image data
        /// </summary>
        public Stream ImageStream { get; private set; }
        
        /// <summary>
        /// mimetype of the image
        /// </summary>
        public string MimeType { get; private set; }

        /// <summary>
        /// creates a new Images result using imageStream as source and specific mimeType
        /// </summary>
        /// <param name="imageStream">a stream of the binary Image</param>
        /// <param name="mimeType"></param>
        public ImageResult(Stream imageStream, string mimeType)
        {
            if (imageStream == null)            
                throw new ArgumentNullException("imageStream");
            
             if (mimeType == null)
                throw new ArgumentNullException("contentType");
           

             this.ImageStream = imageStream;
             this.MimeType = mimeType;
        }

        /// <summary>
        /// executes result. renders an Image to the HttpResponse.OutputStream
        /// </summary>
        /// <param name="context">controllercontext</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context ");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = this.MimeType;

            byte[] buffer = new byte[4096];
            this.ImageStream.Position = 0;
            while (true)
            {
                int read = this.ImageStream.Read(buffer, 0, buffer.Length);
                if (read == 0)
                    break;
                response.OutputStream.Write(buffer, 0, read);
            }

            this.ImageStream.Close();
            this.ImageStream.Dispose();
            response.End();
        }

    }
}
