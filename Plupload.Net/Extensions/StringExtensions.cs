using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plupload.Net;

namespace System
{
    public static class StringExtensions
    {
        public static string GetAbsoluteWebPath(this string path) 
        {
            if (String.IsNullOrWhiteSpace(path)) return path;

            return path.Replace("~/", PluploadContext.Instance.GetConfiguration().ApplicationPath);
        }

        public static string GetAbsoluteWebPathForRessource(this string ressourcePath) 
        {
            if (String.IsNullOrWhiteSpace(ressourcePath)) return ressourcePath;

            string path = "~/Ressource/EmbeddedJavaScript?scriptPath=" + ressourcePath;
            return path.GetAbsoluteWebPath();       
        }
    }
}
