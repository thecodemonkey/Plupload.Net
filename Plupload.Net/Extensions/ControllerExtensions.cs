using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.IO;
using Plupload.Net.Model;

namespace System.Web.Mvc
{
    /// <summary>
    /// contains controller extension methods
    /// </summary>
    public static class  ControllerExtensions
    {
        /// <summary>
        /// renders an imageResult using a stream
        /// </summary>
        /// <param name="controller">an instance of the controller</param>
        /// <param name="imageStream">stream of the image</param>
        /// <param name="mimeType">mimeType of the images</param>
        /// <returns>an ImageResult wich renders the Image to the output</returns>
        public static ImageResult Image(this Controller controller, Stream imageStream, string mimeType)
        {
            return new ImageResult(imageStream, mimeType);
        }

        /// <summary>
        /// renders an imageResult using byte array
        /// </summary>
        /// <param name="controller">an instance of the controller</param>
        /// <param name="imageBytes">an array of the bytes of the image</param>
        /// <param name="mimeType">mimeType of the images</param>
        /// <returns>an ImageResult wich renders the Image to the output</returns>
        public static ImageResult Image(this Controller controller, byte[] imageBytes, string mimeType)
        {
            return new ImageResult(new MemoryStream(imageBytes), mimeType);
        }
    }
}
