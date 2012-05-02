using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using System.Web;

namespace Plupload.Net.Model
{
    public class ImageResult : ActionResult
    {
        public Stream ImageStream { get; private set; }
        public string ContentType { get; private set; }


        public ImageResult(Stream imageStream, string contentType)
        {
            if (imageStream == null)            
                throw new ArgumentNullException("imageStream");
            
             if (contentType == null)
                throw new ArgumentNullException("contentType");
           

             this.ImageStream = imageStream;
             this.ContentType = contentType;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context ");

            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = this.ContentType;


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
