using Plupload.Net;
using Plupload.Net.Model;
using System.IO;
using Plupload.Net.Utils;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Plupload<T>(this HtmlHelper<T> html)
        {
            return Plupload<T>(html, null);
        }

        public static MvcHtmlString Plupload<T>(this HtmlHelper<T> html, PluploadConfiguration configuration) 
        {
            LogWriter.Debug("create plupload...");

            PluploadContext.Instance.SetConfiguration(configuration);
            return html.Action("Index", "Plupload");
        }

        public static MvcHtmlString Script<T>(this HtmlHelper<T> html, string resPath) 
        {
            return html.Action("Script", "Plupload", new { path = resPath });
        }

        public static MvcHtmlString Style<T>(this HtmlHelper<T> html, string resPath)
        {
            return html.Action("Style", "Plupload", new { path = resPath});
        }
    }
}