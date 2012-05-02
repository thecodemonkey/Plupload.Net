using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Plupload.Net.Model;
using Plupload.Net.Utils;

namespace Plupload.Net.Controllers
{

    /// <summary>
    /// the base class for all plupload.net controllers. 
    /// Provides access to the common context and configuration, 
    /// embeddedView handling, common exception logging
    /// All additional controllers should be derived from this class!
    /// </summary>
    public class ControllerBase : Controller
    {
        /// <summary>
        /// gets the merged plupload configuration. 
        /// There are many levels(default => application => instance), 
        /// where you can configure the plupload.net. 
        /// this instance of configuration is merged. You should always working
        /// with the merged configuration.
        /// </summary>
        protected PluploadConfiguration Configuration 
        {
            get 
            {
                return this.PluploadContext.GetConfiguration();
            }
        }

        /// <summary>
        /// gets the plupload context. PluploadContex is the central(singleton) context, wich
        /// contains the common funtionality. For example the configuration of plupload.net.
        /// </summary>
        protected PluploadContext PluploadContext
        {
            get
            {
                return PluploadContext.Instance;
            }
        }

        /// <summary>
        /// gets the embeded viewpath as webpath. 
        /// </summary>
        /// <param name="embedViewPath">a dot separated path of the embedded view</param>
        /// <returns>a virtual webpath where the embedded view located</returns>
        protected virtual string GetEmbeddedViewPath(string embedViewPath)
        {
            SVirtualPathProvider pathProvider = new SVirtualPathProvider();
            string path = pathProvider.CombineVirtualPaths("~/Plupload.Net/", embedViewPath);
            pathProvider.FileExists(path);
            SVirtualFile file = (SVirtualFile)pathProvider.GetFile(path);

            return file.VirtualPath;
        }

        /// <summary>
        /// gets the embedded partial view
        /// </summary>
        /// <param name="embedViewPath">a dot separated path of the embedded view</param>
        /// <returns>an actionresult of the embedded view</returns>
        protected virtual ActionResult EmbeddedPartialView(string embeddedViewPath)
        {
            return PartialView(this.GetEmbeddedViewPath(embeddedViewPath));            
        }

        /// <summary>
        /// gets the embedded partial view
        /// </summary>
        /// <param name="embedViewPath">a dot separated path of the embedded view</param>
        /// <param name="model">a model for the view</param>
        /// <returns>an actionresult of the embedded view</returns>
        protected virtual ActionResult EmbeddedPartialView(string embeddedViewPath, object model)
        {
            return PartialView(this.GetEmbeddedViewPath(embeddedViewPath), model);
        }

        /// <summary>
        /// writes all exceptions, occured within controllers to the log. 
        /// the default logfile you will find here: AppRoot/PluploadLogs/plupload.log
        /// </summary>
        /// <param name="filterContext">filteContext</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            LogWriter.Error(filterContext.Exception);
        }
    }
}
