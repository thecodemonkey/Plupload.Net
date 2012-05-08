using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Plupload.Net.Model
{
    /// <summary>
    /// contains constans with the special paths to the embedde asset.
    /// this path is a namespace path to the asset.
    /// </summary>
    public class PluploadConstants
    {
        /// <summary>
        /// a list of all valid Image Extensions
        /// </summary>
        public static readonly String[] IMAGE_EXTENSIONS = new string[] { ".jpg", ".png", ".gif", ".jpeg", ".bmp" };

        /// <summary>
        /// the main plupload view
        /// </summary>
        public const string VIEWS_RAZOR_UPLOAD_INDEX = "Plupload.Net.Views.Plupload.Index.cshtml";
        
        /// <summary>
        /// the alternativ main iframe view
        /// </summary>
        public const string VIEWS_RAZOR_UPLOAD_IFRAME_INDEX = "Plupload.Net.Views.IFrameIndex.cshtml";
        
        /// <summary>
        /// the default script view
        /// </summary>
        public const string VIEWS_RAZOR_UPLOAD_SCRIPT = "Plupload.Net.Views.Script.cshtml";

        /// <summary>
        /// the default style view
        /// </summary>        
        public const string VIEWS_RAZOR_UPLOAD_STYLE = "Plupload.Net.Views.Style.cshtml";

        /// <summary>
        /// view for dynamic loading plupload using ajax
        /// </summary>
        public static string VIEWS_RAZOR_UPLOAD_DYNAMICLOAD = "Plupload.Net.Views.Plupload.PluploadDynamicLoader.cshtml";

        /// <summary>
        /// session key for sessionspecific pluploadconfiguration
        /// </summary>
        public const string REQUEST_CONFIGURATION = "REQUEST_CONFIGURATION";
                
        /// <summary>
        /// path of the embedded default polupload configuration
        /// </summary>
        public const string CONFIGURATION_MAIN = "Plupload.Net.Plupload.config";

        /// <summary>
        /// script for dynamic loading of plupload using usajx
        /// </summary>
        public const string JS_DYNAMICLOADER = "Plupload.Net.Scripts.plupload.net.dynamicloader.js";


        /// <summary>
        /// startup script for dynamic loading of additional scripts
        /// </summary>
        public const string JS_PLUPLOAD_STARTUP = "Plupload.Net.Scripts.plupload.net.startup.js";

    }
}
