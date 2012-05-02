using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;
using System.Web;
using System.IO;

namespace Plupload.Net
{
    public class SVirtualFile : VirtualFile
    {
        private string m_path;
        private static object lockThis = new object();

        public SVirtualFile(string virtualPath)
            : base(virtualPath)
        {
            m_path = VirtualPathUtility.ToAppRelative(virtualPath);
        }

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
