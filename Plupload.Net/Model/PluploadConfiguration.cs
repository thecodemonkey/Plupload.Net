using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Web;

namespace Plupload.Net.Model
{
    public class PluploadConfiguration
    {
        private static string _plupload_config;

        public string CSSPluploadQueue { get; set; }
        public string CSSPluploadDotNet { get; set; }
        public string CSSLightBox { get; set; }

        public string JSjQuery { get; set; }
        public string JSjQueryUI { get; set; }
        public string JSPluploadFull { get; set; }
        public string JSPluploadQueue { get; set; }
        public string JSPluploadDotNet { get; set; }
        public string JSPluploadPreload { get; set; }
        public string JSLightbox { get; set; }
        public string JSBrowserPlus { get; set; }

        public string Silverlight { get; set; }
        public string Flash { get; set; }

        public bool? UseCDN { get; set; }
        public string UploadDirectory { get; set; }
        public string LogDirectory { get; set; }
        public SaveOptions SaveOptions { get; set; }
        public bool? Debug { get; set; }

        //plupload js core settings
        [XmlArrayItem("Filter")]
        public List<FileFilter> FileFilters { get; set; }
        public string Runtimes { get; set; }
        public string PluploadServerURL { get; set; }
        public string MaxFileSize { get; set; }
        public bool? MultipleQueues { get; set; }
        public bool? URLStreamUpload { get; set; }
        public ResizeProperty Resize { get; set; }


        static PluploadConfiguration() 
        {
            _plupload_config = Path.Combine(GetApplicationBasePath(), "plupload.config");
        }
        private static string GetApplicationBasePath()
        {
            string path = string.Empty;
            string appPath = Path.GetDirectoryName(new Uri(Assembly.GetCallingAssembly().CodeBase).AbsolutePath);
            appPath = Uri.UnescapeDataString(appPath).ToLower();

            switch (Path.GetFileName(appPath))
            {
                case "debug":
                case "release":
                case "bin":
                    if (!appPath.EndsWith(Path.DirectorySeparatorChar.ToString())) appPath += Path.DirectorySeparatorChar;
                    // TODO: This does not work while debugging if the configuration files are just linked into the Configuration folder!
                    // Linking is common for test projects!
                    path = appPath
                    .Replace("\\bin\\x86\\release\\", "").Replace("\\bin\\x86\\debug\\", "") // JB: Added
                    .Replace("\\bin\\x64\\release\\", "").Replace("\\bin\\x64\\debug\\", "") // JB: Added
                    .Replace("\\bin\\release\\", "").Replace("\\bin\\debug\\", "")
                    .Replace("\\debug\\", "").Replace("\\release\\", "")
                    .Replace("\\bin\\", "");
                    break;
                default:
                    path = appPath;
                    break;
            }
            return path;
        }

        public static string PluploadConfigPath{ get { return _plupload_config; }}

        public string GetPhysicalUploadPath()
        {
            return this.GetPhysicalPath(this.UploadDirectory);
        }

        public string GetPhysicalLogPath()
        {
            return this.GetPhysicalPath(this.LogDirectory);
        }

        private static object lockThis = new object();

        private string GetPhysicalPath(string directory)
        {
            string path = directory;

            if (String.IsNullOrEmpty(directory)) 
                throw new Exception("please define directory name");


            if (directory.Contains('~') || !(directory.Contains(@":\") || directory.Contains(@"\\")))
            {
                path = HttpContext.Current.Server.MapPath(directory);
            }

            if (!Directory.Exists(path))
            {
                lock (lockThis)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
            }

            return path;
        }

        
        /// <summary>
        /// merges an in instance of the plupload configuration with the given configuration
        /// </summary>
        /// <param name="configuration">instance to be merged</param>
        /// <returns>merged plupload configuration</returns>            
        public PluploadConfiguration Merge(PluploadConfiguration configuration) 
        {                                                   
            if (configuration == null) return this;

            PluploadConfiguration merged = new PluploadConfiguration();
            merged.CSSLightBox = (configuration.CSSLightBox != null)? configuration.CSSLightBox : this.CSSLightBox;
            merged.CSSPluploadDotNet = (configuration.CSSPluploadDotNet != null) ? configuration.CSSPluploadDotNet : this.CSSPluploadDotNet;
            merged.CSSPluploadQueue = (configuration.CSSPluploadQueue != null) ? configuration.CSSPluploadQueue : this.CSSPluploadQueue;
            merged.FileFilters = (configuration.FileFilters != null) ? configuration.FileFilters : this.FileFilters;

            merged.JSBrowserPlus = (configuration.JSBrowserPlus != null) ? configuration.JSBrowserPlus : this.JSBrowserPlus;
            merged.JSjQuery = (configuration.JSjQuery != null) ? configuration.JSjQuery : this.JSjQuery;
            merged.JSjQueryUI = (configuration.JSjQueryUI != null) ? configuration.JSjQueryUI : this.JSjQueryUI;
            merged.JSLightbox = (configuration.JSLightbox != null) ? configuration.JSLightbox : this.JSLightbox;
            merged.JSPluploadDotNet = (configuration.JSPluploadDotNet != null) ? configuration.JSPluploadDotNet : this.JSPluploadDotNet;
            merged.JSPluploadFull = (configuration.JSPluploadFull != null)? configuration.JSPluploadFull : this.JSPluploadFull;
            merged.JSPluploadQueue = (configuration.JSPluploadQueue != null)? configuration.JSPluploadQueue : this.JSPluploadQueue;
            merged.MaxFileSize = (configuration.MaxFileSize != null) ? configuration.MaxFileSize : this.MaxFileSize;
            merged.MultipleQueues = (configuration.MultipleQueues.HasValue)? configuration.MultipleQueues : this.MultipleQueues;
            merged.PluploadServerURL = (configuration.PluploadServerURL != null) ? configuration.PluploadServerURL : this.PluploadServerURL;
            merged.Resize = (configuration.Resize != null) ? configuration.Resize : this.Resize;
            merged.Runtimes = (configuration.Runtimes != null) ? configuration.Runtimes : this.Runtimes;
            merged.SaveOptions = (configuration.SaveOptions != SaveOptions.None) ? configuration.SaveOptions : this.SaveOptions;
            merged.UploadDirectory = (configuration.UploadDirectory != null) ? configuration.UploadDirectory : this.UploadDirectory;
            merged.LogDirectory = (configuration.LogDirectory != null) ? configuration.LogDirectory : this.LogDirectory;            
            merged.URLStreamUpload = (configuration.URLStreamUpload.HasValue) ? configuration.URLStreamUpload : this.URLStreamUpload;
            merged.UseCDN = (configuration.UseCDN.HasValue) ? configuration.UseCDN : this.UseCDN;

            merged.Flash = (configuration.Flash != null) ? configuration.Flash : this.Flash;
            merged.Silverlight = (configuration.Silverlight != null) ? configuration.Silverlight : this.Silverlight;

            merged.Debug = (configuration.Debug != null) ? configuration.Debug : this.Debug;
            merged.JSPluploadPreload = (configuration.JSPluploadPreload != null) ? configuration.JSPluploadPreload : this.JSPluploadPreload;

            return merged;
        }
    }
}
