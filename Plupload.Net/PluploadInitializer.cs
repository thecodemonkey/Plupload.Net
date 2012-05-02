using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Plupload.Net.Model;

namespace Plupload.Net
{
    internal static class PluploadInitializer
    {
       /// <summary>
       /// Initalizes the Context for the MultipleFileUploader Plugin
       /// </summary>
       /// <param name="uploadPath"></param>
       /// <param name="context"></param>
       /// <param name="saveOptions"></param>
       /// <param name="loadJQuery"></param>
        internal static void Initalize(string uploadPath, System.Web.HttpContext context, SaveOptions saveOptions, bool loadJQuery,MultipleUploaderConfiguration config)
        {
            if ( System.Web.HttpContext.Current.Session[PluploadConstants.UPLOAD_DIRECTORY] == null)
            {
                RegisterVirtualPathProvider();
                InitalizeUploadPath(uploadPath, context);
                context.Session.Add(PluploadConstants.SAVE_OPTIONS_KEY, saveOptions);
                context.Session.Add(PluploadConstants.LOAD_J_QUERY, loadJQuery);
                context.Session.Add(PluploadConstants.CONFIGURATION_SETTINGS, config);

            }
        }

        private static void RegisterVirtualPathProvider()
        {
            System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new SVirtualPathProvider());          
        }

        private static void InitalizeUploadPath(string uploadPath, System.Web.HttpContext context)
        {
            if (String.IsNullOrEmpty(uploadPath))
                uploadPath = "~/Uploads/";

            if (uploadPath.Contains('~')||!(uploadPath.Contains(@":\")||uploadPath.Contains(@"\\")))
            {
                //context.Session.Add(MultipleFileUploaderConstants.UPLOADER_WEB_PATH_KEY, uploadPath);

                uploadPath = context.Server.MapPath(uploadPath);                
            }

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            

            context.Session.Add(PluploadConstants.UPLOAD_DIRECTORY, uploadPath);
        }
    }
}
