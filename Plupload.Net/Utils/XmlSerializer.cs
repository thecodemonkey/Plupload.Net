using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text.RegularExpressions;
using Plupload.Net.Model;

namespace Plupload.Net.Utils
{
    public class XMLSerializer<T> where T : new()
    {
        public static T Load()
        {
            if (File.Exists(PluploadConfiguration.PluploadConfigPath))
                return Load(PluploadConfiguration.PluploadConfigPath);

            return default(T);
        }

        public static T Load(string configurationPath)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(configurationPath);

            return Load(xdoc.DocumentElement);
        }

        public static T Load(XmlNode node)
        {
            using (XmlReader xmlReader = new XmlNodeReader(node))
            {
                Type t = typeof(T);

                XmlSerializer ser = new XmlSerializer(typeof(T));
                return (T)ser.Deserialize(xmlReader);
            }
        }

        public static XmlNode ToXMLNode(object obj)
        {
            string xml = ToXML(obj, Encoding.UTF8);

            xml = xml.Replace("\r\n", "&#10;").Replace("\n", "&#10;");

            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);

            return xdoc;
        }

        public static string ToXML(object obj, Encoding encoding)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                XmlWriterSettings xws = new XmlWriterSettings()
                {
                    Encoding = encoding,
                    Indent = false,
                    OmitXmlDeclaration = true,
                    NamespaceHandling = NamespaceHandling.OmitDuplicates,
                    NewLineOnAttributes = true
                };

                using (XmlWriter xw = XmlWriter.Create(ms, xws))
                {
                    XmlSerializer ser = new XmlSerializer(obj.GetType(), string.Empty);
                    ser.Serialize(xw, obj);

                    string result = encoding.GetString(ms.ToArray());

                    return result.Substring(1);  //Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
                }
            }
        }
    }
}
