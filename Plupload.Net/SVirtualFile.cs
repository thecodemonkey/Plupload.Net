using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web;
using System.IO;

namespace Plupload.Net
{
    /// <summary>
    /// resolves virtual paths. it will be used for embedded ressources.
    /// </summary>
    public class SVirtualFile : VirtualFile
    {
        private string m_path;
        private static object lockThis = new object();

        public SVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            m_path = VirtualPathUtility.ToAppRelative(virtualPath);
        }

        /// <summary>
        /// gets the stream of the embedded ressource
        /// </summary>
        /// <returns></returns>
        public override System.IO.Stream Open()
        {
            lock (lockThis)
            {
                var parts = m_path.Split('/');
                var assemblyName = parts[1];
                var resourceName = parts[2];
          
                assemblyName = Path.Combine(HttpRuntime.BinDirectory, assemblyName);
                var assembly = System.Reflection.Assembly.LoadFile(assemblyName + ".dll");

                if (assembly != null)
                {
                    return assembly.GetManifestResourceStream(resourceName);
                   
                }
                return null;
            }
        }
    }

}
