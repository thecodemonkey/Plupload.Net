using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Drawing.Imaging;

namespace Plupload.Net.Utils
{
    class RessourceHelper
    {

        /// <summary>
        /// extracts an embedded image from assembly
        /// </summary>
        /// <param name="resourcePath">path of the embedded resource</param>
        /// <param name="assembly">source assemby</param>
        /// <returns>initialized Image object</returns>
        public static Image GetImageResource(string resourcePath, Assembly assembly)
        {
            using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
            {
                return new Bitmap(stream);
            }
        }

        /// <summary>
        /// extracts an embedded image from Plupload.Net assembly
        /// </summary>
        /// <param name="resourcePath">path of the embedded resource</param>
        /// <param name="assembly">source assemby</param>
        /// <returns>initialized Image object</returns>
        public static Image GetImageResource(string resourcePath)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(RessourceHelper));
            return GetImageResource(resourcePath, assembly);
        }

        /// <summary>
        ///  extracts an embedded ressource from an assembly
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
  
        public static Stream GetResourceStream(string resourcePath, Assembly assembly)
        {
            return assembly.GetManifestResourceStream(resourcePath);
            
        }
        /// <summary>
        ///  extracts an embedded ressource from Plupload.Net assembly
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Stream GetResourceStream(string resourcePath)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(RessourceHelper));
            return GetResourceStream(resourcePath, assembly);
        }
        
        /// <summary>
        /// gets the content from embedded resource as text, for example content of *.css file or *.js
        /// </summary>
        /// <param name="resourcePath">special path of embedded resource</param>
        /// <param name="assembly">assembly where the resource is included</param>
        /// <returns>the content of embedded resource as text</returns>
        public static string GetTextResource(string resourcePath, Assembly assembly)
        {
          
                using (Stream stream = assembly.GetManifestResourceStream(resourcePath))
                {
                    stream.Position = 0;
                    using (StreamReader sr = new StreamReader(stream))
                    {
                        return sr.ReadToEnd();
                    }
                }
            
        }

        /// <summary>
        /// gets the content from embedded resource as text, for example content of *.css file or *.js
        /// </summary>
        /// <param name="resourcePath">special path of embedded resource</param>
        /// <returns>the content of embedded resource as text</returns>
        public static string GetTextResource(string resourcePath)
        {
            Assembly assembly = Assembly.GetAssembly(typeof(RessourceHelper));
            string result = GetTextResource(resourcePath, assembly);
            return result;
        }

        /// <summary>
        /// Creates a Thumbnail from Image
        /// </summary>
        /// <param name="path">The Physical Path of the Image</param>
        /// <param name="dimensions">dimensions of the Thumbnail</param>
        /// <returns>Stream of Image with the format Jpeg</returns>
        public static  Stream CreateThumbnail(string path, Size dimensions)
        {
            using (var image = Image.FromFile(path))
            {
                using (var thumb = image.GetThumbnailImage(dimensions.Width, dimensions.Height, () => false, IntPtr.Zero))
                {
                    MemoryStream ms = new MemoryStream();
                    thumb.Save(ms, ImageFormat.Jpeg);
                    ms.Position = 0;
                    return ms;
                }
            }
        }
        /// <summary>
        /// Creates a Stream from Image
        /// </summary>
        /// <param name="path">The Physical Path of the Image</param>
        /// <param name="dimensions">dimensions of the Thumbnail</param>
        /// <returns>Stream of Image</returns>
        public static Stream CreateImageStream(string path, ref string context)
        {
            using (var image = Image.FromFile(path))
            {
                MemoryStream ms = new MemoryStream();
                if (image.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    context = "image/jpeg";
                }
                else if (image.RawFormat.Guid == ImageFormat.Png.Guid)
                {
                    image.Save(ms, ImageFormat.Png);
                    context = "image/png";
                }
                else if (image.RawFormat.Guid == ImageFormat.Gif.Guid)
                {
                    image.Save(ms, ImageFormat.Gif);
                    context = "image/gif";
                }
                else if (image.RawFormat.Guid == ImageFormat.Bmp.Guid)
                {
                    image.Save(ms, ImageFormat.Bmp);
                    context = "image/bmp";
                }
                else 
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    context = "image/jpeg";
                }
                ms.Position = 0;
                return ms;
            }
               
        }    
    
    }
}
