using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Plupload.Net.Model;
using Plupload.Net.Utils;

namespace Plupload.Net.Controllers
{
    public class ControllerBase : Controller
    {
        protected PluploadConfiguration Configuration 
        {
            get 
            {
                return this.PluploadContext.GetConfiguration();
            }
        }

        protected PluploadContext PluploadContext
        {
            get
            {
                return PluploadContext.Instance;
            }
        }

        protected virtual string GetEmbeddedViewPath(string embedViewPath)
        {
            SVirtualPathProvider pathProvider = new SVirtualPathProvider();
            string path = pathProvider.CombineVirtualPaths("~/Plupload.Net/", embedViewPath);
            pathProvider.FileExists(path);
            SVirtualFile file = (SVirtualFile)pathProvider.GetFile(path);

            return file.VirtualPath;
        }

        protected virtual ActionResult EmbeddedPartialView(string embeddedViewPath)
        {
            return PartialView(this.GetEmbeddedViewPath(embeddedViewPath));            
        }

        protected virtual ActionResult EmbeddedPartialView(string embeddedViewPath, object model)
        {
            return PartialView(this.GetEmbeddedViewPath(embeddedViewPath), model);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            LogWriter.Error(filterContext.Exception);
        }
    }
}
