using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace System
{
    /// <summary>
    /// contains object extension methods
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// serialize an object to json string using JavaScriptSerializer
        /// </summary>
        /// <param name="obj">object to be serialized</param>
        /// <returns>json string as result of serialization action</returns>
        public static string ToJSON(this object obj) 
        {
            if (obj == null) return null;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
    }
}
