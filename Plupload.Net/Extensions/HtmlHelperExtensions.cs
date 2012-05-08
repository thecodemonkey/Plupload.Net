using Plupload.Net; 
using Plupload.Net.Model;
using System.IO;            
using Plupload.Net.Utils;       

namespace System.Web.Mvc.Html
{                                   
    /// <summary>                       
    /// contains controller extension methods
    /// </summary>                              
    public static class HtmlHelperExtensions            
    {                                           
        /// <summary>
        /// renders the complete plupload Module(UI) using default settings
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html">instance of the htmlhelper</param>
        /// <returns></returns>                                         
        public static MvcHtmlString Plupload<T>(this HtmlHelper<T> html)
        {
            return Plupload<T>(html, new PluploadConfiguration());
        }

        /// <summary>
        /// renders the complete plupload Module(UI) using custom settings given by configuration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html">the instance of the htmlhelper</param>
        /// <param name="configuration">custom instance settings. This configuration will be merged with the application and default configuration.
        /// fill only settings what are needed by the specific instance, all another settings will be used from application or default configuration.
        /// </param>
        /// <returns>the main view of the plupload modul inclusiv complete UI</returns>
        public static MvcHtmlString Plupload<T>(this HtmlHelper<T> html, PluploadConfiguration configuration) 
        {
            LogWriter.Debug("create plupload...");
            PluploadContext.Instance.SetConfiguration(configuration);
            
            return html.Action("Index", "Plupload");
        }

        public static MvcHtmlString PluploadDynamicPlaceholder<T>(this HtmlHelper<T> html) 
        {
            return PluploadDynamicPlaceholder<T>(html, new PluploadConfiguration());
        }

        public static MvcHtmlString PluploadDynamicPlaceholder<T>(this HtmlHelper<T> html, PluploadConfiguration configuration)
        {                                                           
            LogWriter.Debug("create plupload...");  

            PluploadContext.Instance.SetConfiguration(configuration);
            return html.Action("PluploadDynamicLoader", "Plupload");
        }

        /// <summary>
        /// renders the script tag for the specific embedded script file given by resPath.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html">the instance of the HtmlHelper</param>
        /// <param name="resPath">a dot separated ressource path to the specific embedded script file.</param>
        /// <returns>a script tag <script/> </returns>
        public static MvcHtmlString Script<T>(this HtmlHelper<T> html, string resPath) 
        {
            return html.Action("ScriptTag", "Ressource", new { path = resPath });
        }

        /// <summary>
        /// renders the startup script, using plupload.startup.js.
        /// all placeholders within js template file will be resolved.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html"></param>
        /// <param name="resPath"></param>
        /// <param name="onCompleteFunction"></param>
        /// <returns></returns>
        public static MvcHtmlString ScriptContent<T>(this HtmlHelper<T> html, string resPath, string onCompleteFunction)
        {
            string scriptContent = RessourceHelper.GetTextResource(PluploadConstants.JS_PLUPLOAD_STARTUP);

            PluploadConfiguration config = PluploadContext.Instance.GetConfiguration();

            scriptContent = scriptContent.Replace("[SCRIPT_URL]", resPath.GetAbsoluteWebPathForRessource()).
                                Replace("[ON_COMPLETE_FUNCTION]", onCompleteFunction).
                                Replace("[SCRIPT_JQUERY_URL]", config.JSjQuery.GetAbsoluteWebPath());

            return new MvcHtmlString(scriptContent);
 
            //html.Action("EmbeddedTextRessource", "Ressource", new { textPath = resPath, contentType = "text/javascript" });
        }


        /// <summary>
        /// renders the style tag for the specific embedded css file given by resPath.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="html">the instance of the HtmlHelper</param>
        /// <param name="resPath">a dot separated ressource path to the specific embedded css file.</param>
        /// <returns>a style tag <link /> </returns>
        public static MvcHtmlString Style<T>(this HtmlHelper<T> html, string resPath)
        {
            return html.Action("StyleTag", "Ressource", new { path = resPath });
        }
    }
}