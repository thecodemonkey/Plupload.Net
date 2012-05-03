using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace Plupload.Net
{
    /// <summary>
    /// provides a custom virtual path
    /// </summary>
    public class SVirtualPathProvider : VirtualPathProvider
    {
        public SVirtualPathProvider()
        {

        }

        /// <summary>
        /// identifies if the given virtualPath a valid virtual path is.
        /// </summary>
        /// <param name="virtualPath">virtual path to be validated</param>
        /// <returns>true if is a valid virtual path othervise false.</returns>
        private bool IsEmbeddedResourcePath(string virtualPath)
        {
            var checkPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return checkPath.StartsWith("~/Plupload.Net/", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// identifies if the file given by virtualPath exists.
        /// </summary>
        /// <param name="virtualPath">virtual path of the embedded ressource</param>
        /// <returns>true if exists otherwise false</returns>
        public override bool FileExists(string virtualPath)
        {
            return IsEmbeddedResourcePath(virtualPath) || base.FileExists(virtualPath);
        }

        /// <summary>
        /// gets the virtual File given by virtualPath. If virtualPath is not a valid virtual file, 
        /// the path will be used as a physical path.
        /// </summary>
        /// <param name="virtualPath">virtual path of the embedded ressource</param>
        /// <returns>a virtual file</returns>
        public override VirtualFile GetFile(string virtualPath)
        {
            if (IsEmbeddedResourcePath(virtualPath))
            {
                return new SVirtualFile(virtualPath);
            }
            else
            {
                return base.GetFile(virtualPath);
            }
        }


        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (IsEmbeddedResourcePath(virtualPath))
            {
                return null;
            }
            else
            {
                return base.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
        }
    }

}
