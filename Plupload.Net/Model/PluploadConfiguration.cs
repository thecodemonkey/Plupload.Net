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
    /// <summary>
    /// this configuration class defines all settings needed by plupload.
    /// There are many levels(default/embedded => application => instance), 
    /// where you can configure the plupload.net. 
    /// </summary>
    public class PluploadConfiguration
    {
        private static string _plupload_config;

        /// <summary>
        /// gets or sets the url of the plupload.queue stylesheet file.
        /// </summary>
        public string CSSPluploadQueue { get; set; }
        /// <summary>
        /// gets or sets the url of the plupload.net stylesheet file.
        /// </summary>
        public string CSSPluploadDotNet { get; set; }

        /// <summary>
        /// gets or sets the url of the lightbox stylesheet file.
        /// </summary>
        public string CSSLightBox { get; set; }

        /// <summary>
        /// gets or sets the url of the jQuery script file.
        /// </summary>
        public string JSjQuery { get; set; }
        
        /// <summary>
        /// gets or sets the url of the jQuery.ui script file.
        /// </summary>
        public string JSjQueryUI { get; set; }

        /// <summary>
        /// gets or sets the url of the plupload.full script file.
        /// </summary>
        public string JSPluploadFull { get; set; }

        /// <summary>
        /// gets or sets the url of the plupload.queue script file.
        /// </summary>
        public string JSPluploadQueue { get; set; }
        
        /// <summary>
        /// gets or sets the url of the plupload.net script file.
        /// </summary>        
        public string JSPluploadDotNet { get; set; }

        /// <summary>
        /// gets or sets the url of the plupload.preload script file.
        /// </summary>
        public string JSPluploadPreload { get; set; }

        /// <summary>
        /// gets or sets the url of the plupload.net.dynamicloader.js
        /// needed to loading plupload dynamically using ajax.
        /// </summary>
        public string JSPluploadDynamicLoader { get; set; }

        /// <summary>
        /// gets or sets the url of the lightbox script file.
        /// </summary>
        public string JSLightbox { get; set; }

        /// <summary>
        /// gets or sets the url of the browserplus script file.
        /// </summary>
        public string JSBrowserPlus { get; set; }

        /// <summary>
        /// gets or sets the url of the silverligh file selector file(plupload.silverlight.xap).
        /// </summary>
        public string Silverlight { get; set; }

        /// <summary>
        /// gets or sets the url of the flas file selector file(plupload.flash.swf).
        /// </summary>
        public string Flash { get; set; }

        /// <summary>
        /// gets or sets the flag for using script and style ressources from the CDN(content delivery network)
        /// </summary>
        public bool? UseCDN { get; set; }

        /// <summary>
        /// gets or sets the default Plupload directory path. 
        /// The path can be an application or a physical path.
        /// </summary>
        public string UploadDirectory { get; set; }

        /// <summary>
        /// gets or sets the name of the Directory where are the logfiles should be stored. 
        /// The path can be an application or a physical path.
        /// </summary>
        public string LogDirectory { get; set; }

        /// <summary>
        /// gets or sets the specific saveoptions.
        /// </summary>
        public SaveOptions SaveOptions { get; set; }

        /// <summary>
        /// activate or deactivate the debug mode.
        /// the debug informations will be written only if setting Debug set to tru within the CUSTOM APPLICATION CONFIGURATION!!!
        /// </summary>
        public bool? Debug { get; set; }

        //gets or sets allowed filetypes
        [XmlArrayItem("Filter")]
        public List<FileFilter> FileFilters { get; set; }
        
        /// <summary>
        /// gets or sets available runtimes in specific fallback order
        /// default: html5,gears,flash,silverlight,browserplus
        /// </summary>
        public string Runtimes { get; set; }
        
        /// <summary>
        /// gets or sets the url to the Save Action of the PluploadController
        /// </summary>
        public string PluploadServerURL { get; set; }

        /// <summary>
        /// gets or sets the maximum allowed size of a single file, wich can be uploaded
        /// </summary>
        public string MaxFileSize { get; set; }

        /// <summary>
        /// gets or sets if multipleQueue are activated
        /// </summary>
        public bool? MultipleQueues { get; set; }

        /// <summary>
        /// gets or sets if urlstreamuploa is allowed
        /// </summary>
        public bool? URLStreamUpload { get; set; }

        /// <summary>
        /// gets or sets the Settings for automatically file resizing
        /// </summary>
        public ResizeProperty Resize { get; set; }

        /// <summary>
        /// gets or sets the flag for automatically initialization of the plupload.
        /// this flag is usefull for dynamic loading of plupload using ajax. Just disable AutoInit=false
        /// and call JsFunction 'InitializePlupload()'.
        /// </summary>
        public bool? AutoInit { get; set; }

        /// <summary>
        /// gets or sets the automatically including of all scripts and stylesheets
        /// </summary>
        public bool? IncludeAllScriptsAndStyles { get; set; }

        public string ApplicationPath
        {
            get 
            {
                return HttpContext.Current.Request.ApplicationPath;
            }
            set { } 
        }

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

        /// <summary>
        /// gets the physical path of the application specific plupload.config file.
        /// </summary>
        public static string PluploadConfigPath{ get { return _plupload_config; }}

        /// <summary>
        /// gets the physical path of the upload directory
        /// </summary>
        /// <returns>a physical path</returns>
        public string GetPhysicalUploadPath()
        {
            return this.GetPhysicalPath(this.UploadDirectory);
        }

        /// <summary>
        /// gets the physical path of the log directory
        /// </summary>
        /// <returns>a physical path</returns>
        public string GetPhysicalLogPath()
        {
            return this.GetPhysicalPath(this.LogDirectory);
        }

        private static object lockThis = new object();

        /// <summary>
        /// gets the physical path of specific directory
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
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


            merged.FileFilters = (configuration.FileFilters != null && configuration.FileFilters.Count > 0) ? configuration.FileFilters : this.FileFilters;

            merged.JSBrowserPlus = (configuration.JSBrowserPlus != null) ? configuration.JSBrowserPlus : this.JSBrowserPlus;
            merged.JSjQuery = (configuration.JSjQuery != null) ? configuration.JSjQuery : this.JSjQuery;
            merged.JSjQueryUI = (configuration.JSjQueryUI != null) ? configuration.JSjQueryUI : this.JSjQueryUI;
            merged.JSLightbox = (configuration.JSLightbox != null) ? configuration.JSLightbox : this.JSLightbox;
            merged.JSPluploadDotNet = (configuration.JSPluploadDotNet != null) ? configuration.JSPluploadDotNet : this.JSPluploadDotNet;
            merged.JSPluploadDynamicLoader = (configuration.JSPluploadDynamicLoader != null) ? configuration.JSPluploadDynamicLoader : this.JSPluploadDynamicLoader;

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

            merged.AutoInit = (configuration.AutoInit.HasValue) ? configuration.AutoInit : this.AutoInit;
            merged.IncludeAllScriptsAndStyles = (configuration.IncludeAllScriptsAndStyles.HasValue) ? configuration.IncludeAllScriptsAndStyles : this.IncludeAllScriptsAndStyles;
            
            return merged;
        }
    }
}
