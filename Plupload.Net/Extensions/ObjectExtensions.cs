using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace System
{
    public static class ObjectExtensions
    {
        public static string ToJSON(this object obj) 
        {
            if (obj == null) return null;

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
    }
}
